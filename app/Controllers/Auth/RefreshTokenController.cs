using Api.App.Database;
using Api.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.App.Controllers.Auth;

[ApiController]
[Authorize(Roles = "personal")]
[Route("/auth/refresh")]
public class RefreshTokenController(DatabaseContext context) : BaseController
{
    [HttpPut("{refreshToken}")]
    public async Task<IActionResult> Refresh(string refreshToken)
    {
        var user = await LoggedUser(context);
        var token = await CurrentToken(context);
        
        if (user is null)
        {
            return AnswerUnauthorized();
        }

        if (token?.RefreshToken == refreshToken)
        {
            token = Token.Regenerate(token, user);
            context.Tokens.Update(token);
            await context.SaveChangesAsync();
            
            return AnswerSuccess(new
            {
                id = token.Id,
                token = token.AccessToken,
                refreshToken = token.RefreshToken,
                expiresAt = token.ExpiredAt
            });
        }

        return AnswerUnauthorized(new[]{"refresh-token-invalid"});
    }
}