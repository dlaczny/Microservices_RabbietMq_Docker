using RabbitMQ.Client.Events;

namespace API.Repository
{
    public interface IMessageBusRepository
    {
        Task AddMessage(string filePath);
    }
}