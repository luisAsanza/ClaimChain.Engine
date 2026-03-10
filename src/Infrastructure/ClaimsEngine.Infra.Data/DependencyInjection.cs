using ClaimsEngine.Infra.Data.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClaimsEngine.Infra.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the interceptor as a singleton
            services.AddSingleton<InsertOutboxMessagesInterceptor>();

            // Register the DbContext with singleton lifetime and wire the interceptor
            services.AddDbContext<Persistence.ClaimDbContext>(
                (provider, options) =>
                {
                    var interceptor = provider.GetRequiredService<InsertOutboxMessagesInterceptor>();
                    options.AddInterceptors(interceptor);
                });

            return services;
        }
    }
}
