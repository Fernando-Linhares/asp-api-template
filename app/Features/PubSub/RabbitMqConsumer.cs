using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace Api.App.Features.PubSub;

public class RabbitMqConsumer: BackgroundService
{
    private readonly IConnection _connection;
    private readonly List<IModel> _channels;

    public RabbitMqConsumer()
    {
        var factory = new ConnectionFactory()
        {
            HostName = ConfigApp.Get("rabbitmq.host"),
            UserName = ConfigApp.Get("rabbitmq.user"),
            Password = ConfigApp.Get("rabbitmq.password")
        };
        _connection = factory.CreateConnection();
        _channels = new List<IModel>();
        foreach (var workerItem in WorkerList.GetWorkers())
        {
            IModel channel = _connection.CreateModel();
            channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);
            channel.QueueDeclare(workerItem.QueueName);
            channel.QueueBind(queue: workerItem.QueueName, exchange: "logs", routingKey: ConfigApp.Get("rabbitmq.key"));
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
                var handler = WorkerList.GetWorker(queue);
                var body = ea.Body.ToArray();
                var text = Encoding.UTF8.GetString(body);
                var message = new Message(text);
                handler.Message = message;
                await handler.Execute();
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
            channel.Close();
        }
        _connection.Close();
        base.Dispose();
    }
}