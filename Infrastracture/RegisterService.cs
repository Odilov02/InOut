using Application.Common.Interfaces;
using Infrastructure.Persistance;
using Infrastructure.Persistance.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class RegisterService
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            options.UseLazyLoadingProxies();
        });

        services.AddScoped<IAppDbContext, AppDbContext>();
        services.AddScoped<IDateTimeService, DateTimeService>();
        return services;

    }
}
