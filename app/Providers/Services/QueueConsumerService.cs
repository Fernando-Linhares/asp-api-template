using System.Text;
using Api.App.Features.Queue;
using Api.App.Features.Queue.Handlers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace Api.App.Providers.Services;

public class QueueConsumerService: BackgroundService
{
    private readonly IConnection _connection;
    private readonly List<IModel> _channels;

    public QueueConsumerService()
    {
        var factory = new ConnectionFactory()
        {
            HostName = ConfigApp.Get("rabbitmq.host"),
            UserName = ConfigApp.Get("rabbitmq.user"),
            Password = ConfigApp.Get("rabbitmq.password")
        };
        _connection = factory.CreateConnection();
        _channels = new List<IModel>();
        foreach (KeyValuePair<string, IQueueHandler?> queue in Queues.GetQueues())
        {
            IModel channel = _connection.CreateModel();
            channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);
            channel.QueueDeclare(queue.Key);
            channel.QueueBind(queue: queue.Key, exchange: "logs", routingKey: ConfigApp.Get("rabbitmq.key"));
            _channels.Add(channel);
        }
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (IModel channel in _channels)
        {
            string queue = channel.CurrentQueue;
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received +=  async (model, ea) =>
            {
                var handler = Queues.GetHandler(queue);
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await handler.Execute(message);
                var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                Log.Information($"[x] queue {queue} executed at {time}......");
            };

            channel.BasicConsume(queue: queue,
                autoAck: true,
                consumer: consumer);
        }
        
        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        foreach (IModel channel in _channels)
        {
            channel?.Close();
        }
        _connection?.Close();
        base.Dispose();
    }
}