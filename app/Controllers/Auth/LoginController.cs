using Api.App.Controllers.Requests.Auth;
using Api.App.Database;
using Api.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.App.Controllers.Auth;

[ApiController]
[Route("/auth/[controller]")]
public class LoginController(DatabaseContext context) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] Login login)
    {
        User? user = await context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);

        if (user == null)
        {
            return AnswerNotFound(login.Email);
        }
        
        if (BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
        {
            var token = Token.GenerateNewToken(user);
            context.Tokens.Add(token);
            await context.SaveChangesAsync();
            
            return AnswerSuccess(new
            {
                id = token.Id,
                token = token.AccessToken,
                refreshToken = token.RefreshToken,
                expiresAt = token.ExpiredAt
            });
        }

        return AnswerUnauthorized(new { fields = new [] {"Email", "Password"}});
    }
}