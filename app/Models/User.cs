using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.App.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Avatar { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set;  }
    [Required]
    [MaxLength(5)]
    public string ConfirmationCode {get; set;}
    public bool Confirmed { get; set; }
    public bool Active { get; set; }
    public string Rules { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}