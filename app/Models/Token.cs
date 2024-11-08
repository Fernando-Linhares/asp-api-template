using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using Serilog;

namespace Api.App.Models;

public class Token
{
    [Key]
    public Guid Id { get; init; }
    [ForeignKey("User")]
    public Guid UserId { get; init; }
    public User User { get; init; }
    public string AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime Expires { get; set; }
    public long ExpiredAt => new DateTimeOffset(Expires).ToUnixTimeMilliseconds();
    public DateTime CreatedAt { get; set; }

    public static Token GenerateNewToken(User user)
    {
        string key = ConfigApp.Get("app.key");
        string uid = user.Id.ToString();
        
        var claims = new List<Claim>
        {
            new (ClaimTypes.Email, user.Email),
            new (ClaimTypes.Sid, uid),
            new (ClaimTypes.Name, user.Name),
        };
        
        string[] rules = JsonConvert.DeserializeObject<string[]>(user.Rules) ?? [];
        
        foreach (string rule in rules)
        {
            claims.Add(new Claim(ClaimTypes.Role, rule));
        }
        
        int expiresDefault = Int32.Parse(ConfigApp.Get("app.expiration.token"));
        
        var token = new Token
        {
            User = user,
            AccessToken =  GenerateAccessToken(expiresDefault, claims.ToArray(), user.Id, key),
            RefreshToken = GenerateAccessToken(expiresDefault, claims.ToArray(), user.Id, key),
            Expires = DateTime.Now.AddMinutes(expiresDefault),
            CreatedAt = DateTime.Now
        };

        LogTokenCreation(token);

        return token;
    }

    public static Token Regenerate(Token token, User user)
    {
        string key = ConfigApp.Get("app.key");
        var claims = new List<Claim>
        {
            new (ClaimTypes.Email, user.Email),
            new (ClaimTypes.Sid, user.Id.ToString()),
            new (ClaimTypes.Name, user.Name),
        };
        string[] rules = JsonConvert.DeserializeObject<string[]>(user.Rules) ?? new string[0];
        foreach (string rule in rules)
        {
            claims.Add(new Claim(ClaimTypes.Role, rule));
        }
        int expiresDefault = Int32.Parse(ConfigApp.Get("app.expiration.token"));
        token.AccessToken = Token.GenerateAccessToken(expiresDefault, claims.ToArray(), user.Id, key);
        LogTokenCreation(token);
        return token;
    }

    private static void LogTokenCreation(Token token)
    {
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        Log.Information( $"[INFO|token_creation|{timestamp}] User: "
           + $"id - {token.User.Id}, name - {token.User.Name}"
            + $", email - {token.User.Email}");
    }

    private static string GenerateAccessToken(int expirationMinutes, Claim[] claims, Guid userId, string key)
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

    public static Token GenerateForgotPasswordToken(User user)
    {
        var key = ConfigApp.Get("app.key");

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Sid, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, "forgot_password"),
        };
        
        int expiresDefault = 15;
        string token = GenerateAccessToken(expiresDefault, claims, user.Id, key);

        return new Token
        {
            User = user,
            AccessToken = token,
            Expires = DateTime.Now.AddMinutes(expiresDefault),
            CreatedAt = DateTime.Now
        };
    }

    public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
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
        var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securittokyToken);

        if (securittokyToken is not JwtSecurityToken jwtSecurityToken 
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }
        
        return principal;
    }
}