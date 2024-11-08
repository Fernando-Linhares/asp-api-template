using Api.App.Database;
using Api.App.Features.Pagination;
using Api.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.App.Controllers.Moderation.Users;

[ApiController]
[Authorize(Roles="moderation.users")]
[Route("/moderation/users/pagination")]
public class UserPaginationController(DatabaseContext context,  IPagination<User> pagination): BaseController
{
    [HttpGet]
    public async Task<IActionResult> Paginate(
        [FromQuery(Name = "search")] string? search = null,
        [FromQuery(Name = "page")] int page = 1,
        [FromQuery(Name = "perPage")] int perPage = 10)
    {
        var user = await LoggedUser();
        
        var inputPagination = new PaginationInput<User>(
            page,
            perPage,
            context.Users.Count(u => (search != null)
                ? (u.Email.Contains(search) || u.Name.Contains(search)) && u.DeletedAt == null && user.Id != u.Id
                : u.DeletedAt == null && user.Id != u.Id),
            context.Users.Where(u => (search != null)
                ? (u.Email.Contains(search) || u.Name.Contains(search)) && u.DeletedAt == null && user.Id != u.Id
                : u.DeletedAt == null && user.Id != u.Id
                )
        );
        
        var paginationData = await pagination.Paginate(inputPagination);
        return AnswerPagination(paginationData);
    }
}