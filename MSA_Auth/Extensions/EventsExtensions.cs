using MSA_Auth_API.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace MSA_Auth_API.Extensions
{
    public static class EventsExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var config = new EventBusSettings();
            configuration.Bind("EventBus", config);
            services.AddSingleton(config);

            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = config.HostName,
                UserName = config.User,
                Password = config.Password
            };

            services.AddSingleton(factory);

            return services;
        }
    }
}
