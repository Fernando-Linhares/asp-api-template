using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Api.App.Features.Jwt;

public class TokenBuilder
{
    public Token GenerateAccessToken(InputToken inputToken)
    {
        var claims = new Claim[]
        {
            new (ClaimTypes.Name, inputToken.Name),
            new (ClaimTypes.Email, inputToken.Email),
            new (ClaimTypes.Sid, inputToken.Id.ToString()),
        }
        .ToList();

        foreach (string rule in inputToken.Rules)
        {
            claims.Add(new Claim(ClaimTypes.Role, rule));
        }

        string key = ConfigApp.Get("app.key");
        
        string token = GenerateJwtToken(
            inputToken.Timestamp,
            claims.ToArray(),
            key
        );
        
        return new Token(token);
    }

    public Token GenerateRefreshToken(InputToken inputToken)
    {
        var claims = new Claim[]
            {
                new (ClaimTypes.Sid, inputToken.Id.ToString()),
                new (ClaimTypes.Role, "refresh_token"),
            }
            .ToList();

        string key = ConfigApp.Get("app.key");

        string token = GenerateJwtToken(
            inputToken.Timestamp,
            claims.ToArray(),
            key
        );

        return new Token(token);
    }

    private string GenerateJwtToken(int expirationMinutes, Claim[] claims, string key)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        DateTime expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiration,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var securityToken = jwtHandler.CreateToken(tokenDescriptor);

        return jwtHandler.WriteToken(securityToken);
    }

    public void LogToken(string status, Token token)
    {
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        Log.Information($"[{timestamp}][{status.ToUpper()}] {token.AccessToken}");
    }
    
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigApp.Get("app.key"))),
            ValidateLifetime = false
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken 
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }
        
        return principal;
    }
}