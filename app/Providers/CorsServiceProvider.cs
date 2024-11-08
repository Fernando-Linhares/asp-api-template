namespace Api.App.Providers;

public static class CorsServiceProvider
{
    public static void Bind(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "Default",
                b =>
                {
                    b.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    }
}