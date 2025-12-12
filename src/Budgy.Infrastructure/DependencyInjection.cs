using Budgy.Application.Interfaces;
using Budgy.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Budgy.Infrastructure
{
    /// <summary>
    /// Provides extension methods for registering infrastructure services in the dependency injection container.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers infrastructure services with the specified service collection.
        /// </summary>
        /// <param name="services">The service collection to which the services will be added.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IExpenseService, ExpenseService>();
            return services;
        }
    }
}