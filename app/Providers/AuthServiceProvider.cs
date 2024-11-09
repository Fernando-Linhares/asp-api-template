using System.Text;
using Api.App.Features.Database;
using Api.App.Features.Jwt;
using Api.App.Features.Policies;
using Api.App.Models;
using Api.App.Policies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Api.App.Providers;

public static class AuthServiceProvider
{
    private static readonly List<IPolicy> Policies = [
        new AuthenticationPolicy(),
        new ModerationPolicy()
    ];

    public static void Bind(WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(GetJwtToken()),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        
        builder.Services.AddAuthorization(options =>
        {
            using var context = DatabaseContextSingleton.GetInstance();
            
            foreach (Rule rule in context.Rules.ToList())
            {
                options.AddPolicy(
                    rule.Name, 
                    configPolicy => configPolicy.RequireClaim("Permission", rule.Name));
            }
        });

        builder.Services.AddScoped<IStateLessSession, StateLessSession>();
    }

    private static byte[] GetJwtToken()
    {
        return Encoding.UTF8.GetBytes(ConfigApp.Get("app.key"));
    }
}