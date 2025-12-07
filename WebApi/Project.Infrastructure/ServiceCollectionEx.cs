using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Domain.IRepositories;
using Project.Infrastructure.Data;
using Project.Infrastructure.Repositories;

namespace Project.Infrastructure
{
    public static class ServiceCollectionEx
    {
        public static IServiceCollection AddInfraServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // DB
            services.AddScoped<IProjectDb, ProjectDb>();

            // Repositories
            services.AddScoped<IUserRepository, UsersRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<IPenaltyRepository, PenaltyRepository>();

            return services;
        }
    }
}
