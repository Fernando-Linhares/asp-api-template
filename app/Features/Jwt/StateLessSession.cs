using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;

namespace Api.App.Features.Jwt;

public class StateLessSession(IDistributedCache cache): IStateLessSession
{
    private readonly TokenBuilder _tokenBuilder = new();
    
    public ClaimsPrincipal GetSessionInfo(string token)
    {
        return _tokenBuilder.GetPrincipalFromExpiredToken(token);
    }

    public async Task<Object> CreateUserSession(Guid id, string email, string name, string[] rules)
    {
        int minutes = Int32.Parse(ConfigApp.Get("app.expiration.token"));
        var input = new InputToken(id, name, email, minutes, rules);
        var accessToken = _tokenBuilder.GenerateAccessToken(input);
        var refreshToken = _tokenBuilder.GenerateRefreshToken(input);
        var session = Guid.NewGuid();
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(minutes + 20)
        };
        _tokenBuilder.LogToken("created",  accessToken);
        await cache.SetAsync(session + "_access", Encoding.UTF8.GetBytes(accessToken.AccessToken), options);
        await cache.SetAsync(session + "_refresh", Encoding.UTF8.GetBytes(refreshToken.AccessToken), options);

        return new
        {
            id = session.ToString(),
            accessToken = accessToken.AccessToken,
            refreshToken = refreshToken.AccessToken,
            expiresAt = TimeSpan.FromMinutes(minutes).TotalSeconds
        };
    }

    public async Task<bool> RefreshUserSession(Guid sessionId, string refreshToken)
    {
        string session = sessionId + "_refresh";
        var token = await cache.GetStringAsync(session);
        return token != null && token == refreshToken;
    }

    public Token ForgotPassword(Guid id, string email, string name)
    {
        int minutes = 10;
        var input = new InputToken(id, name, email, minutes, ["forgot_password"]);
        var accessToken = _tokenBuilder.GenerateAccessToken(input);
        _tokenBuilder.LogToken("forgot_password",  accessToken);
        return accessToken;
    }
}