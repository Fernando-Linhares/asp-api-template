using Serilog;

namespace Api.App.Providers;

public static class LogServiceProvider
{
    public static void Bind(WebApplicationBuilder builder)
    {
        string logInfoPath = LogPathInfo();
        string logErrorPath = LogPathError();
        Log.Logger = new LoggerConfiguration()
            .WriteTo
            .File(logInfoPath, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
            .WriteTo
            .File(logErrorPath, rollingInterval: RollingInterval.Day,  restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error)
            .CreateLogger();
    }

    private static string LogPathError()
    {
        return Path.Combine(
            Directory.GetCurrentDirectory(),
            "tmp",
            "logs",
            "errors",
            "logs");
    }
    
    private static string LogPathInfo()
    {
        return Path.Combine(
            Directory.GetCurrentDirectory(),
            "tmp",
            "logs",
            "errors",
            "logs");
    }
}