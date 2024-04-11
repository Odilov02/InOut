using Microsoft.AspNetCore.Identity;
namespace Domain.Entities;
#nullable disable
public class User : IdentityUser<Guid>
{
    public string FullName { get; set; }
    public string Password { get; set; }
    public string Login { get; set; }
    public virtual ICollection<In?> Ins { get; set; }
    public virtual ICollection<Spend?> Spends { get; set; }
    public virtual Construction? Construction { get; set; }
    public  long Residual { get; set; }
}
