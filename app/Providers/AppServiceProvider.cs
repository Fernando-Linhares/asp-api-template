using Api.App.Features.Dotenv;
using Microsoft.AspNetCore.Diagnostics;
using Api.App.Features.Exception;

namespace Api.App.Providers;

public static class AppServiceProvider
{
    public static void Bind(WebApplicationBuilder builder)
    {
        GetterEnv.Build();
        builder.Services.AddControllers();
        CorsServiceProvider.Bind(builder);
        DatabaseServiceProvider.Bind(builder);
        QueueServiceProvider.Bind(builder);
        AuthServiceProvider.Bind(builder);
        LogServiceProvider.Bind(builder);
        ModerationServiceProvider.Bind(builder);
        CacheServiceProvider.Bind(builder);
        builder.Services.AddSingleton<IExceptionHandler, Handling>();
    }
}