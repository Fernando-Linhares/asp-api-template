using System.Text;
using Api.App.Features.Policies;
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
            foreach (var rule in Policies.SelectMany(policy => policy.Rules))
            {
                options.AddPolicy(
                    rule, 
                    configPolicy => configPolicy.RequireClaim("Permission", rule));
            }
        });
    }

    private static byte[] GetJwtToken()
    {
        return Encoding.UTF8.GetBytes(ConfigApp.Get("app.key"));
    }
}