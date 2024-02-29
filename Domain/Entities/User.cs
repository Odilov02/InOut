using Microsoft.AspNetCore.Identity;
namespace Domain.Entities;
public class User : IdentityUser<Guid>
{
    public string FullName { get; set; }
    public string Password { get; set; }
    public virtual ICollection<In?> Ins { get; set; }
    public virtual ICollection<Out?> Outs { get; set; }
    public virtual Construction? Construction { get; set; }
    public int Residual { get; set; }
}
