using Application.Common.CostumeAtribute;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Application.Common.Dtos.InDtos;
#nullable disable
public class AddInDto
{
    [Required(ErrorMessage = "Кирим сабабини киритиш шарт!")]
    [MaxLength(200,ErrorMessage = "Кирим сабаби узунлиги 200 тадан кам болиш шарт!")]
    public string Reason { get; set; }

    [Required(ErrorMessage = "Кирим суммасини киритиш шарт!")]
    public long? Price { get; set; }
    [Required]
    public Guid ConstructionId { get; set; }
}
