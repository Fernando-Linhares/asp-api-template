using Api.App.Controllers.Requests.Auth;
using Api.Database;
using Api.App.Features.FileSystem;
using Api.App.Features.Jwt;
using Api.App.Mails;
using Api.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RandomString4Net;

namespace Api.App.Controllers.Auth;

[ApiController]
[Route("/auth/[controller]")]
public class RegisterController(DatabaseContext context, IStateLessSession stateLessSession) : BaseController
{
   [HttpPost]
   public async Task<IActionResult> Register([FromBody] Register request)
   {
      await request.Validate();

      var rules = GetRules();

      var user = new User
      {
         Id = Guid.NewGuid(),
         Avatar = GetAvatar(),
         Name = request.Name,
         Email = request.Email,
         Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
         CreatedAt = DateTime.Now,
         UpdatedAt = DateTime.Now,
         Rules = JsonConvert.SerializeObject(rules),
         ConfirmationCode = RandomString.GetString(Types.ALPHABET_UPPERCASE, 5).ToUpper(),
         Active = true,
         Confirmed = false
      };

      context.Users.Add(user);
      await context.SaveChangesAsync();
      SendAccountConfirmationEmail(user);

      var session = await stateLessSession.CreateUserSession(
         user.Id,
         user.Email,
         user.Name,
         rules
      );

      return AnswerSuccess(session);
   }

   private string GetAvatar()
   {
      return new Manager("profile").Url("avatar-m.png");
   }

   private string[] GetRules()
   {
      return (
         from Rule r in context.Rules
         join Group g in context.Groups on r.GroupId equals g.Id
         where g.Name == "default"
         select r.Name
      ).ToArray();
   }

   private void SendAccountConfirmationEmail(User user)
   {
      var accountConfirmationEmail = new AccountConfirmationMailBase
      {
         Code = user.ConfirmationCode,
         To = user.Email
      };
      accountConfirmationEmail.DispatchOnQueue();
   }
}