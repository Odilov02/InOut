namespace Application.Common.Models;
#nullable disable
public class UserUpdate
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string ConfirmedPassword { get; set; }
}
