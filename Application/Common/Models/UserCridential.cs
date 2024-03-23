using System.ComponentModel.DataAnnotations;

namespace Application.Common.Models;
#nullable disable
public class UserCridential
{
    [Required(ErrorMessage = "Логин киритилиши шарт!")]
    public string UserName { get; set; }
    [Required(ErrorMessage = "Парол киритилиши шарт!")]
    public string Password { get; set; }
}
