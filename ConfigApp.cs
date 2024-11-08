using Api.App.Features.Dotenv;

namespace Api;

public static class ConfigApp
{
    private static Dictionary<string, string>? _defaults;

    private static void Load()
    {
        _defaults = new Dictionary<string, string>
        {
            ["app.name"] = GetterEnv.Get("APP_NAME") ?? String.Empty,
            ["app.version"] = GetterEnv.Get("APP_VERSION") ?? String.Empty,
            ["app.env"] = GetterEnv.Get("APP_ENV") ?? String.Empty,
            ["app.url"] =  GetterEnv.Get("APP_URL") ?? String.Empty,
            ["app.frontend"] =  GetterEnv.Get("APP_FRONTEND") ?? String.Empty,
            ["app.key"] = GetterEnv.Get("APP_KEY") ?? String.Empty,
            ["app.expiration.token"] = GetterEnv.Get("SESSION_EXPIRATION") ?? String.Empty,
            ["db.connection"] = GetConnectionString(),
            ["smtp.host"] = GetSmtpHost() ?? String.Empty,
            ["smtp.username"] = GetterEnv.Get("SMTP_USER") ?? String.Empty,
            ["smtp.email"] = GetterEnv.Get("SMTP_EMAIL") ?? String.Empty,
            ["smtp.password"] = GetterEnv.Get("SMTP_PASSWORD") ?? String.Empty,
            ["smtp.port"] = GetterEnv.Get("SMTP_PORT") ?? String.Empty,
            ["rabbitmq.host"] = GetRabbitMqHost()?? String.Empty,
            ["rabbitmq.user"] = GetterEnv.Get("RABBITMQ_USER") ?? String.Empty,
            ["rabbitmq.password"] = GetterEnv.Get("RABBITMQ_PASSWORD") ?? String.Empty,
            ["rabbitmq.key"] = GetterEnv.Get("RABBITMQ_KEY") ?? String.Empty,
            ["admin.name"] = GetterEnv.Get("ADMIN_NAME") ?? String.Empty,
            ["admin.email"] = GetterEnv.Get("ADMIN_EMAIL") ?? String.Empty,
            ["admin.password"] = GetterEnv.Get("ADMIN_PASSWORD") ?? String.Empty,
        };
    }

    public static string Get(string key)
    {
        if (_defaults == null)
        {
            Load();
        }
        
        return _defaults[key];
    }

    private static string? GetSmtpHost()
    {
        return GetterEnv.Get("APP_ENV") == "local"
            ? GetterEnv.Get("SMTP_HOST")
            : GetterEnv.Get("SMTP_CONTAINER");
    }

    private static string? GetRabbitMqHost()
    {
        return GetterEnv.Get("APP_ENV") == "local"
            ? GetterEnv.Get("RABBITMQ_HOST")
            : GetterEnv.Get("RABBITMQ_CONTAINER");
    }


    private static string GetConnectionString()
    {
        string? host = GetHostByEnv();
        string? user = GetterEnv.Get("DB_USER");
        string? password = GetterEnv.Get("DB_PASSWORD");
        string? database = GetterEnv.Get("DB_DATABASE");

        return $"Data Source={host},1433;User Id={user};"
               + $"Password={password};Database={database};"
               + "Encrypt=True;TrustServerCertificate=True;";
    }

    private static string? GetHostByEnv()
    {
        return GetterEnv.Get("APP_ENV") == "local"
            ? GetterEnv.Get("DB_HOST")
            : GetterEnv.Get("DB_CONTAINER");
    }
}