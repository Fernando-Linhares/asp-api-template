namespace Api.App.Features.Queue;

public interface IDispatcher
{
    public OutputDispatcher Dispatch(string queueName, IOnQueue onQueue);
}