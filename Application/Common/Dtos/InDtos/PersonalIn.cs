using System.ComponentModel.DataAnnotations;

namespace Application.Common.Dtos.InDtos;

public class PersonalIn
{
    [Required]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Кирим суммаси киритилиши шарт!")]
    public long? Price { get; set; }

    [Required(ErrorMessage = "Кирим сабаби киритилиши шарт!")]
    [MaxLength(200, ErrorMessage = "Кирим сабаби узунлиги 200 тадан кам болиши шарт!")]
    public string Reason { get; set; }
}
