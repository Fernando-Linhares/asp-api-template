using Api.App.Controllers.Requests.Auth.Profile;
using Api.App.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.App.Controllers.Auth.Profile;

[ApiController]
[Authorize(Roles = "personal")]
[Route("/auth/profile/update-avatar")]
public class UpdateAvatarController(DatabaseContext context): BaseController
{
    [HttpPatch]
    public async Task<IActionResult> UpdateAvatar([FromBody] UpdateAvatar updateAvatar)
    {
        var user = await LoggedUser();
        user.Avatar = updateAvatar.Avatar;
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