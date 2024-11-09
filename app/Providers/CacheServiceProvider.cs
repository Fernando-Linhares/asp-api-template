namespace Api.App.Providers;

public static class CacheServiceProvider
{
    public static void Bind(WebApplicationBuilder builder)
    {
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            string host = ConfigApp.Get("redis.host");
            string port = ConfigApp.Get("redis.port");
            string db = ConfigApp.Get("redis.db");
            options.Configuration = $"{host}:{port}";
            options.InstanceName = db;
        });
    }
}