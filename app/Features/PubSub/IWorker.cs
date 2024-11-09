namespace Api.App.Features.PubSub;

public interface IWorker
{
    public Message? Message { get; set; }
    public Task Execute();
}