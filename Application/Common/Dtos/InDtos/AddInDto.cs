using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Dtos.InDtos;
#nullable disable
public class AddInDto
{
    [Required(ErrorMessage = "Кирим сабаби киритиши шарт!")]
    [MaxLength(200,ErrorMessage = "Кирим сабаби узунлиги 200 тадан кам болиши шарт!")]
    public string Reason { get; set; }
    [Required(ErrorMessage = "Кирим сумма киритиши шарт!")]
    [Range(1, long.MaxValue)]
    public long Price { get; set; }
    [NotNull]
    public Guid ConstructionId { get; set; }
}
