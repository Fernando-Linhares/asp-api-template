using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Api.App.Features.Queue;

public class QueueDispatcher: IDispatcher
{
    public OutputDispatcher Dispatch(string queueName, IOnQueue dispatch)
    {
        var connection = GetConnection();
        var channel = connection.CreateModel();
        string serialized = JsonConvert.SerializeObject(dispatch.Serialize()) ?? string.Empty;
        byte[] bytes = Encoding.UTF8.GetBytes(serialized);
        channel.BasicPublish(
            exchange: "logs",
            routingKey: ConfigApp.Get("rabbitmq.key"),
            body: bytes,
            basicProperties: null);
        var time = DateTimeOffset.UtcNow.TotalOffsetMinutes;
        
        return new OutputDispatcher(queueName, dispatch, time);
    }

    private IConnection GetConnection()
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = ConfigApp.Get("rabbitmq.host"),
            UserName = ConfigApp.Get("rabbitmq.user"),
            Password = ConfigApp.Get("rabbitmq.password"),
        };
        
        return connectionFactory.CreateConnection();
    }
}