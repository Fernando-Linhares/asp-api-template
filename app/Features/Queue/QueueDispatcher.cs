using System.Text;
using Api.App.Features.PubSub;
using Newtonsoft.Json;

namespace Api.App.Features.Queue;

public class QueueDispatcher
{
    private readonly RabbitMqDispatcher _dispatcher = new ();
    
    public OutputDispatcher Dispatch(string queueName, IOnQueue dispatch)
    {
        string serialized = JsonConvert.SerializeObject(dispatch.Serialize()) ?? string.Empty;
        byte[] bytes = Encoding.UTF8.GetBytes(serialized);
        _dispatcher.Dispatch(bytes, queueName);
        var time = DateTimeOffset.UtcNow.TotalOffsetMinutes;
        return new OutputDispatcher(queueName, dispatch, time);
    }
}