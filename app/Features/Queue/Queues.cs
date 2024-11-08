using Api.App.Features.Mailer;
using Api.App.Features.Queue.Handlers;

namespace Api.App.Features.Queue;

public static class Queues
{
    private static readonly Dictionary<string, IQueueHandler> Alias = new Dictionary<string, IQueueHandler>
    {
        ["mail"] = new EmailQueueHandler(),
        ["job"] = new JobQueueHandler(),
    };
    

    public static void AddQueue(string queue, IQueueHandler handler)
    {
        Alias[queue] = handler;
    }
    
    public static Dictionary<string, IQueueHandler> GetQueues() => Alias;
    
    public static IQueueHandler GetHandler(string queue) => Alias[queue];
}