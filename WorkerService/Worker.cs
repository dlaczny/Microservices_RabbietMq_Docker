using WorkerService.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

using WorkerService.Repository;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var service = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IVisitRepository>();

#if DEBUG
            var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
#else
                var factory = new ConnectionFactory()
            {
                Uri = new Uri("amqp://guest:guest@rabbitmq:5672")
            };
#endif

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare("queue",
                true, false, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                service.ProcessFileAsync(message);
            };
            channel.BasicConsume("queue", true, consumer);
        }
    }
}