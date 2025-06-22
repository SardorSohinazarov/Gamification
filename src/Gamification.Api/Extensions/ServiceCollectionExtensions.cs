namespace Gamification.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(c => c.CustomSchemaIds(type => type.ToString().Replace('+', '.')));

            return services;
        }
    }
}
