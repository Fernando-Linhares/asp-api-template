using Api.App.Controllers.Requests.Auth;
using Api.App.Features.Jwt;
using Api.Database;
using Api.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Api.App.Controllers.Auth;

[ApiController]
[Route("/auth/[controller]")]
public class LoginController(DatabaseContext context, IStateLessSession stateLessSession) : BaseController
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
            var rules = JsonConvert.DeserializeObject<string[]>(user.Rules);
            if (rules == null)
            {
                return AnswerNotFound(new { error = "not found rules" });
            }
            var session = await stateLessSession.CreateUserSession(user.Id, user.Email, user.Name, rules);
            return AnswerSuccess(session);
        }

        return AnswerUnauthorized(new { fields = new [] {"Email", "Password"}});
    }
}