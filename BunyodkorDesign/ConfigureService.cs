using Infrastructure.Persistance;

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
        return services;
    }
}