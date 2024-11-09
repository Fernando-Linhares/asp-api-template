using Api.App.Controllers.Requests.Auth.Profile;
using Api.Database;
using Api.App.Features.FileSystem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.App.Controllers.Auth.Profile;

[ApiController]
[Authorize(Roles = "profile.upload.avatar")]
[Route("/auth/profile/upload-avatar")]
public class UploadAvatarController(DatabaseContext context) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> UpdateAvatar([FromForm] UploadAvatar request)
    {
        Manager.SaveFile("public/profile", request.Avatar);
        string filename = Manager.GetUrl("profile", request.Avatar.FileName);
        
        var user = await LoggedUser();
        user.Avatar = filename;
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