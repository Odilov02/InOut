using Application.Common.CostumeAtribute;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Models;
#nullable disable
public class UserUpdate
{
    [Required]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Исм-фамилия киритиши шарт!")]
    [MaxLength(50, ErrorMessage = "Исм-фамилия узунлиги 50 тадан кам болиши шарт!")]
    public string FullName { get; set; }


    [Required(ErrorMessage = "Логин киритиши шарт!")]
    [MaxLength(50, ErrorMessage = "Логин узунлиги 50 тадан кам болиши шарт!")]
    public string UserName { get; set; }


    [Required(ErrorMessage = "Телефон номер киритиши шарт!")]
    [MaxLength(13, ErrorMessage = "Телефон номер 13 та болиши шарт!")]
    [MinLength(13, ErrorMessage = "Телефон номер 13 та болиши шарт!")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Парол киритиши шарт!")]
    [Password()]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Парол қайта киритилиши шарт!")]
    [Compare("Password", ErrorMessage = "Пароллар 1 хил болиши шарт!")]
    [DataType(DataType.Password)]
    public string ConfirmedPassword { get; set; }
}
