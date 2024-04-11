using System.ComponentModel.DataAnnotations;

namespace Application.Common.Dtos.FactoryDtos;
#nullable disable
public class AddFactoryDto
{
    [Required(ErrorMessage = "Завод номини киритиш шарт!")]
    [MaxLength(50, ErrorMessage = "Завод номи узунлиги 50 тадан кам болиш шарт!")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Завод хақида маьлумот киритиш шарт!")]
    [MaxLength(200, ErrorMessage = "Завод хақида маьлумот узунлиги 200 тадан кам болиш шарт!")]
    public string Description { get; set; }
}
