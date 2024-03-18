using System.ComponentModel.DataAnnotations;

namespace Application.Common.Models;
#nullable disable
public class UserCridential
{
    [Required(ErrorMessage = "Логин киритиши шарт!")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "Парол киритиши шарт!")]
    public string Password { get; set; }
}
