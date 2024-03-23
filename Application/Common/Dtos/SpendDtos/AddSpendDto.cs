using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Dtos.SpendDtos;
#nullable disable
public class AddSpendDto
{
    [Required(ErrorMessage = "Чиқим сабаби киритилиши шарт!")]
    [MaxLength(200,ErrorMessage = "Чиқим сабаби узунлиги 200 тадан кам болиши шарт!")]
    public string Reason { get; set; }


    [Required(ErrorMessage = "Чиқим сумма киритилиши шарт!")]
    [Range(1, long.MaxValue,ErrorMessage = "Чиқим сумма киритилиши шарт!")]
    public long? Price { get; set; }


    [Required(ErrorMessage = "Чиқим тури танланишлиги киритилиши шарт!")]
    public Guid? SpendTypeId { get; set; }
}
