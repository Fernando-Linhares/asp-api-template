using RabbitMQ.Client;

namespace Api.App.Features.PubSub;

public class RabbitMqDispatcher
{
    private static readonly IConnection? Connection = new ConnectionFactory
    {
        HostName = ConfigApp.Get("rabbitmq.host"),
        UserName = ConfigApp.Get("rabbitmq.user"),
        Password = ConfigApp.Get("rabbitmq.password"),
    }.CreateConnection();

    public void Dispatch(byte[] bytes, string queueName)
    {
        if (Connection is { IsOpen: true })
        {
            var channel = Connection.CreateModel();

            channel.BasicPublish(
                exchange: "logs",
                routingKey: queueName,
                body: bytes,
                basicProperties: null);
        }
    }
}