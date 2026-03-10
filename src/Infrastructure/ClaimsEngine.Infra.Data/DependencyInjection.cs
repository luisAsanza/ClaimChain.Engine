using ClaimsEngine.Infra.Data.Interceptors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ClaimsEngine.Infra.Data.Persistence;
using ClaimsEngine.Infra.Data.Configuration;

namespace ClaimsEngine.Infra.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Options for Database Settings
            services.AddOptions<DatabaseSettings>()
                .BindConfiguration(DatabaseSettings.ClaimsDbSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            // Register the interceptor as a singleton
            services.AddSingleton<InsertOutboxMessagesInterceptor>();

            // Register the DbContext with singleton lifetime and wire the interceptor
            services.AddDbContext<Persistence.ClaimDbContext>(
                (provider, options) =>
                {
                    var interceptor = provider.GetRequiredService<InsertOutboxMessagesInterceptor>();
                    options.AddInterceptors(interceptor);
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        sqlOptions => sqlOptions.MigrationsAssembly(typeof(ClaimDbContext).Assembly.GetName().Name)
                    );
                });

            return services;
        }
    }
}
