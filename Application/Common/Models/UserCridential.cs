using System.ComponentModel.DataAnnotations;

namespace Application.Common.Models;
#nullable disable
public class UserCridential
{
    [Required(ErrorMessage = "Логин киритиш шарт!")]
    public string Login { get; set; }
    [Required(ErrorMessage = "Парол киритиш шарт!")]
    public string Password { get; set; }
}
