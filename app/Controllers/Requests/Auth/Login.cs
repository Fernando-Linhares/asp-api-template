using System.ComponentModel.DataAnnotations;

namespace Api.App.Controllers.Requests.Auth;

public record Login([Required] string Email, [Required]string Password);