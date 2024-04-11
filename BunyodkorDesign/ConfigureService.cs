using Infrastructure.Persistance;
using WebUI.Services;
using WebUI.Services.Interfaces;

namespace WebUI;

public static class ConfigureService
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>()
                 .AddUserManager<UserManager<User>>()
                .AddRoleManager<RoleManager<Role>>()
                .AddEntityFrameworkStores<AppDbContext>()
        .AddSignInManager();
        services.AddScoped<IFileService, FileService>();
        return services;
    }
}