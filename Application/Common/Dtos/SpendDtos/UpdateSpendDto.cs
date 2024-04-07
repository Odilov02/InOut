using System.ComponentModel.DataAnnotations;

namespace Application.Common.Dtos.SpendDtos;

public class UpdateSpendDto
{
    [Required]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Чиқим сабаби киритилиши шарт!")]
    [MaxLength(200, ErrorMessage = "Чиқим сабаби узунлиги 200 тадан кам болиши шарт!")]
    public string Reason { get; set; }


    [Required(ErrorMessage = "Чиқим сумма киритилиши шарт!")]
    [Range(1, long.MaxValue, ErrorMessage = "Чиқим сумма киритилиши шарт!")]
    public long? Price { get; set; }


    [Required(ErrorMessage = "Чиқим тури танланишлиги киритилиши шарт!")]
    public Guid? SpendTypeId { get; set; }
}
