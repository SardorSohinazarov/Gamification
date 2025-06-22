using Microsoft.Extensions.DependencyInjection;

namespace Gamification.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<GamificationDb>();

            return services;
        }
    }
}
