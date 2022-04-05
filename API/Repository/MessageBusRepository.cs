using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository
{
    public class MessageBusRepository : IMessageBusRepository
    {
        private readonly IModel queue;

        public MessageBusRepository()
        {
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

            queue = channel;
        }

        public void AddMessage(string fileName)
        {
            var body = Encoding.UTF8.GetBytes(fileName);

            queue.BasicPublish("", "queue", null, body);
        }
    }
}