using Application.Common.CostumeAtribute;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Models;
#nullable disable
public class UserUpdate
{
    [Required]
    public Guid Id { get; set; }
    [MaxLength(100)]
    [MinLength(3)]
    [NotNull]
    public string FullName { get; set; }

    [MaxLength(100)]
    [MinLength(3)]
    [Required]
    public string UserName { get; set; }

    [MaxLength(13, ErrorMessage = "Telefon Nomer 13 ta bolishi kerak.")]
    [MinLength(13, ErrorMessage = "Telefon Nomer 13 ta bolishi kerak.")]
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Password]
    public string Password { get; set; }
    [Required]
    [Compare("Password", ErrorMessage = "Parollar mos kelmadi.")]
    [DataType(DataType.Password)]
    public string ConfirmedPassword { get; set; }
}
