using Application.Common.Interfaces;
using Infrastructure.Persistance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class RegisterService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            //  options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            //  options.UseInMemoryDatabase("MyTestDB");

            options.UseNpgsql(configuration.GetConnectionString("Default"));
            options.UseLazyLoadingProxies();
        });

        services.AddScoped<IAppDbContext, AppDbContext>();
        return services;

    }
}
