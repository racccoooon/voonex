using Microsoft.Extensions.DependencyInjection;

namespace voonex.eventhub
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddEventHub(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IEventHub, EventHub>();
            return serviceCollection;
        }
    }
}