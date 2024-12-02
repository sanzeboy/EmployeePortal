using Microsoft.Extensions.DependencyInjection;

namespace EmployeePortal.Application.Events
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IServiceProvider _serviceProvider;

        public EventPublisher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
        {
            var subscribers = _serviceProvider.GetServices<IEventSubscriber<TEvent>>();
            foreach (var subscriber in subscribers)
            {
                await subscriber.HandleAsync(@event);
            }
        }
    }

}
