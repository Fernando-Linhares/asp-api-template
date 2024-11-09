using Api.App.Features.PubSub;

namespace Api.App.Workers;

public class DefaultWorker: IWorker
{
    public Message? Message { get; set; }
    public Task Execute()
    {
        return Task.CompletedTask;
    }
}