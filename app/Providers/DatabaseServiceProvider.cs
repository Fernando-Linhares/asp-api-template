using Api.App.Database;

namespace Api.App.Providers;

public static class DatabaseServiceProvider
{
    public static void Bind(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DatabaseContext>();
    }
}