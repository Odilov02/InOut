using Application.Common.CostumeAtribute;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Models;

public class UserUpdateAdmin
{
    [Required]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Логин киритилиши шарт!")]
    [MaxLength(50, ErrorMessage = "Логин узунлиги 50 тадан кам болиши шарт!")]
    public string Login { get; set; }

    [Required(ErrorMessage = "Парол киритилиши шарт!")]
    [Password()]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Парол қайта киритилиши шарт!")]
    [Compare("Password", ErrorMessage = "Пароллар 1 хил болиши шарт!")]
    [DataType(DataType.Password)]
    public string ConfirmedPassword { get; set; }
}
