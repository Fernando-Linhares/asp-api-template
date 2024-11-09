using System.Security.Claims;

namespace Api.App.Features.Jwt;

public interface IStateLessSession
{
   public ClaimsPrincipal GetSessionInfo(string token);

   public Task<Object> CreateUserSession(Guid id, string email, string name, string[] rules);

   public Token ForgotPassword(Guid id, string email, string name);

   public Task<bool> RefreshUserSession(Guid sessionId, string refreshToken);
}