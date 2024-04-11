using Application.Common.CostumeAtribute;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Models;

public class UserUpdateAdmin
{
    [Required]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Логин киритиш шарт!")]
    [MaxLength(50, ErrorMessage = "Логин узунлиги 50 тадан кам болиши шарт!")]
    public string Login { get; set; }

    [Required(ErrorMessage = "Парол киритиш шарт!")]
    [Password()]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Парол қайта киритиш шарт!")]
    [Compare("Password", ErrorMessage = "Пароллар 1 хил болиш шарт!")]
    [DataType(DataType.Password)]
    public string ConfirmedPassword { get; set; }
}
