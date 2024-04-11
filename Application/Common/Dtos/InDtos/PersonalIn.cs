using System.ComponentModel.DataAnnotations;

namespace Application.Common.Dtos.InDtos;

public class PersonalIn
{
    [Required]
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Кирим суммасини киритиш шарт!")]
    public long? Price { get; set; }

    [Required(ErrorMessage = "Кирим сабабини киритиш шарт!")]
    [MaxLength(200, ErrorMessage = "Кирим сабаби узунлиги 200 тадан кам болиш шарт!")]
    public string Reason { get; set; }
}
