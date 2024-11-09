using Api.App.Features.PubSub;
using Api.App.Workers;

namespace Api.App.Providers;

public static class QueueServiceProvider
{
    public static void Bind(WebApplicationBuilder builder)
    {
        WorkerList.Define([
            new WorkerItem("mail", new EmailWorker()),
            new WorkerItem("default", new DefaultWorker()),
        ]);
        
        builder.Services.AddHostedService<RabbitMqConsumer>();
    }
}