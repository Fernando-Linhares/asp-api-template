using Api.App.Controllers.Requests.Auth.Profile;
using Api.App.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.App.Controllers.Auth.Profile;

[ApiController]
[Authorize(Roles = "personal")]
[Route("/auth/profile/account")]
public class UpdateProfileAccountController(DatabaseContext context) : BaseController
{
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Account account)
    {
        var user = await LoggedUser();
        user.Name = account.Name;
        user.Email = account.Email;
        context.Users.Update(user);
        await context.SaveChangesAsync();
        
        return AnswerSuccess(new
        {
            id=user.Id,
            name=user.Name,
            avatar=user.Avatar,
            email=user.Email,
            confirmed=user.Confirmed,
            active=user.Active,
            createdAt=user.CreatedAt
        });
    }
}