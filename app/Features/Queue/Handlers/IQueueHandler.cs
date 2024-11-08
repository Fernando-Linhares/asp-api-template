namespace Api.App.Features.Queue.Handlers;

public interface IQueueHandler
{
    public Task Execute(string wrapJson);
}