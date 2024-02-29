using Microsoft.AspNetCore.Identity;
namespace Application.Common.Interfaces;

public interface IUserManigerService<TUser>
{
    Task<IdentityResult> CreateAsync(TUser user);
}

