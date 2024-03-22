using Application.Common.CostumeAtribute;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Application.Common.Dtos.InDtos;
#nullable disable
public class AddInDto
{
    [Required(ErrorMessage = "Кирим сабаби киритиши шарт!")]
    [MaxLength(200,ErrorMessage = "Кирим сабаби узунлиги 200 тадан кам болиши шарт!")]
    public string Reason { get; set; }

    [Required(ErrorMessage = "Кирим суммаси киритили шарт!")]
    public long? Price { get; set; }
    [Required]
    public Guid ConstructionId { get; set; }
}
