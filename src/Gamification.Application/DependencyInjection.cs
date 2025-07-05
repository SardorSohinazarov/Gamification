using Common.ServiceAttribute;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Gamification.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCustomServices();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
