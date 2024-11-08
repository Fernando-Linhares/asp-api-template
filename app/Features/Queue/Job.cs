namespace Api.App.Features.Queue;

public class Job: IOnQueue
{
    public Task Handle()
    {
        return Task.CompletedTask;
    }

    public object Serialize()
    {
        return new { };
    }
}