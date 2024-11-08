using System.ComponentModel.DataAnnotations;

namespace Api.App.Controllers.Requests.Auth;

public record ForgotPassword([Required][EmailAddress] string Email);