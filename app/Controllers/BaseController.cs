using System.Security.Claims;
using Api.App.Database;
using Api.App.Features.Database;
using Api.App.Features.Pagination;
using Api.App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.App.Controllers;

[ApiController]
[Route("/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected async Task<User> LoggedUser()
    {
        await using var context = DatabaseContextSingleton.GetInstance();
        string userId = User.FindFirst(ClaimTypes.Sid)?.Value ?? throw new Exception($"Not available claim id in provided token");
        var id = new Guid(userId);
        return await context.Users.FindAsync(id) ?? throw new Exception($"Not found user id {userId}");
    }
    
    protected async Task<User?> LoggedUser(DatabaseContext context)
    {
        string userId = User.FindFirst(ClaimTypes.Sid)?.Value ?? throw new Exception($"Not available claim id in provided token");
        var id = new Guid(userId);
        return await context.Users.FindAsync(id);
    }

    protected async Task<Token?> CurrentToken(DatabaseContext context)
    {
        string userId = User.FindFirst(ClaimTypes.Sid)?.Value ?? throw new Exception($"Not available claim id in provided token");
        var id = new Guid(userId);
        return await context.Tokens.OrderByDescending(t => t.CreatedAt).FirstOrDefaultAsync(t => t.User.Id == id);
    }

    protected IActionResult AnswerSuccess(object value)
    {
        return Ok(new { data = value });
    }

    protected IActionResult AnswerPagination<T>(PaginationResult<T> value)
    {
        return Ok(new { data = value.Data, pagination = value.Pagination });
    }

    protected IActionResult AnswerInternalError()
    {
        return StatusCode(500, new { errors = "internalError" });
    }

    protected IActionResult AnswerInternalError(string errors)
    {
        return StatusCode(500, new {  errors });
    }

    protected IActionResult AnswerNotFound()
    {
        return NotFound();
    }

    protected IActionResult AnswerNotFound(object errors)
    {
        return NotFound(new { errors });
    }

    protected IActionResult AnswerUnauthorized()
    {
        return Unauthorized();
    }
    
    protected IActionResult AnswerUnauthorized(object errors)
    {
        return Unauthorized(new { errors });
    }
}