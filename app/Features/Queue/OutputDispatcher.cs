namespace Api.App.Features.Queue;

public record OutputDispatcher(string QueueName, IOnQueue OnQueue, int Timestamp);