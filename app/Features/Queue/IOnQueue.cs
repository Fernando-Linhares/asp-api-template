namespace Api.App.Features.Queue;

public interface IOnQueue
{
    public Task Handle();

    public object Serialize();
}