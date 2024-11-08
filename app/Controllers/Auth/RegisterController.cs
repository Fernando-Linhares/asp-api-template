using Api.App.Controllers.Requests.Auth;
using Api.App.Database;
using Api.App.Features.FileSystem;
using Api.App.Mails;
using Api.App.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RandomString4Net;

namespace Api.App.Controllers.Auth;

[ApiController]
[Route("/auth/[controller]")]
public class RegisterController(DatabaseContext context) : BaseController
{
   [HttpPost]
   public async Task<IActionResult> Register([FromBody] Register request)
   {
      await request.Validate();
      
      string code = RandomString.GetString(Types.ALPHABET_UPPERCASE, 5);
      string defaultAvatar = "avatar-m.png";
      var fileSystem = new Manager("profile");
      
      var user = new User
      {
         Avatar = fileSystem.Url(defaultAvatar),
         Name = request.Name,
         Email = request.Email,
         Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
         CreatedAt = DateTime.Now,
         UpdatedAt = DateTime.Now,
         Rules = JsonConvert.SerializeObject(new []{
            "personal",
         }),
         Active = true,
         Confirmed = false,
         ConfirmationCode = code
      };

      context.Users.Add(user);
      await context.SaveChangesAsync();

      var token = Token.GenerateNewToken(user);
      context.Tokens.Add(token);
      await context.SaveChangesAsync();
      
      var accountConfirmationEmail = new AccountConfirmationMailBase(code)
      {
         To = user.Email
      };
      accountConfirmationEmail.DispatchOnQueue();

      return AnswerSuccess(new
      {
         id = token.Id,
         token = token.AccessToken,
         refreshToken = token.RefreshToken,
         expiresAt = token.ExpiredAt
      });
   }
}