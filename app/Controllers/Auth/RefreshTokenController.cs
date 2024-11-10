using Api.App.Features.Jwt;
using Api.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.App.Controllers.Auth;

[ApiController]
[Authorize(Roles = "personal.session.refresh")]
[Route("/auth/refresh")]
public class RefreshTokenController(DatabaseContext context, IStateLessSession stateLessSession) : BaseController
{
    [HttpPut("{sessionId}/{refreshToken}")]
    public async Task<IActionResult> Refresh(Guid sessionId, string refreshToken)
    {
        var user = await LoggedUser(context);
        
        if (user is null)
        {
            return AnswerUnauthorized();
        }
        
        if (await stateLessSession.RefreshUserSession(sessionId, refreshToken))
        {
            var rules = JsonConvert.DeserializeObject<string[]>(user.Rules);
            if (rules == null)
            {
                return AnswerInternalError("user-rules-not-found");
            }
            var session = await stateLessSession.CreateUserSession(user.Id, user.Email, user.Name, rules);
            return AnswerSuccess(session);
        }

        return AnswerUnauthorized(new[]{"refresh-token-invalid"});
    }
}