using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.App.Controllers.Auth;

[ApiController]
[Authorize(Roles = "personal.me")]
[Route("/auth/[controller]")]
public class MeController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> UserData()
    {
        var user = await LoggedUser();

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