namespace Api.App.Features.PubSub;

public static class WorkerList
{
    private static readonly List<WorkerItem> Workers = new();
    public static List<WorkerItem> GetWorkers() => Workers;

    public static void Define(List<WorkerItem> workers)
    {
        foreach (var worker in workers)
        {
            GetWorkers().Add(worker);   
        }
    }

    public static IWorker GetWorker(string queue)
    {
        IWorker? worker = null;
        
        foreach (var item in Workers)
        {
            if (item.QueueName == queue)
            {
                return item.Worker;
            }
        }

        if (worker == null)
        {
            throw new System.Exception($"Queue {queue} not found");
        }
        
        return worker;
    }
}