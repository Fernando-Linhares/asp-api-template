using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Api.App.Features.Pagination;

namespace Api.App.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Avatar { get; set; }
    public string ConfirmationCode { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set;  }
    public bool Confirmed { get; set; }
    public bool Active { get; set; }
    public string Rules { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Collection<Token> Tokens { get; set; }

    public Token? GetCurrentToken()
    {
        return Tokens.LastOrDefault();
    }
}