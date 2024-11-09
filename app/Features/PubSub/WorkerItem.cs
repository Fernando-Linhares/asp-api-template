namespace Api.App.Features.PubSub;

public record WorkerItem(string QueueName, IWorker Worker);