using System.ComponentModel.DataAnnotations;
using Api.App.Database;
using Api.App.Exceptions;
using Api.App.Features.Database;
using Microsoft.EntityFrameworkCore;

namespace Api.App.Controllers.Requests.Auth;

public record Register
{
    [Required]
    public string Name { get; init; } = "";
    [Required]
    [EmailAddress]
    public string Email { get; init; } = "";
    [Required]
    public string Password { get; init; } = "";

    public async Task Validate()
    {
        await using DatabaseContext context = DatabaseContextSingleton.GetInstance();
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == Email);
        
        if (user is not null)
        {
            throw new BadRequest($"Email {Email} already exists");
        }
    }
};