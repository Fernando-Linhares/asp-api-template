using Api.App.Providers.Services;

namespace Api.App.Providers;

public static class QueueServiceProvider
{
    public static void Bind(WebApplicationBuilder builder)
    {
        builder.Services.AddHostedService<QueueConsumerService>();
    }
}