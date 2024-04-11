using Application.Common.CostumeAtribute;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.DTOs.UserDTOs;
#nullable disable
public class UserRegisterDto
{
    [Required(ErrorMessage = "Исм-фамилия киритиш шарт!")]
    [MaxLength(50, ErrorMessage = "Исм-фамилия узунлиги 50 тадан кам болиш шарт!")]
    public string FullName { get; set; }


    [Required(ErrorMessage = "Логин киритиш шарт!")]
    [MaxLength(50, ErrorMessage = "Логин узунлиги 50 тадан кам болиш шарт!")]
    public string Login { get; set; }

    [Required(ErrorMessage = "Телефон номер киритиш шарт!")]
    [MaxLength(13, ErrorMessage = "Телефон номер 13 та болиш шарт!")]
    [MinLength(13, ErrorMessage = "Телефон номер 13 та болиш шарт!")]
    public string PhoneNumber { get; set; }
    [Required(ErrorMessage = "Парол киритиш шарт!")]
    [DataType(DataType.Password)]
    [Password()]
    public string Password { get; set; }
    [Required(ErrorMessage = "Парол қайта киритиш шарт!")]
    [Compare("Password", ErrorMessage = "Пароллар 1 хил болиш шарт!")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}
