using System.ComponentModel.DataAnnotations;

namespace Application.Common.Dtos.ConstructionDtos;
#nullable disable
public class AddConstructionDto
{
    [Required(ErrorMessage = "Обьект номини киритиш шарт!")]
    [MaxLength(50, ErrorMessage = "Обьект номи узунлиги 50 тадан кам болиш шарт!")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Шартнома суммасини киритиш шарт!")]
    public long? ContractPrice { get; set; }
    [Required(ErrorMessage = "Масьул шахс танлаш шарт!")]
    public Guid? UserId { get; set; }

    [Required(ErrorMessage = "Обьект хақида маьлумот киритиш шарт!")]
    [MaxLength(200, ErrorMessage = "Обьект хақида маьлумот узунлиги 200 тадан кам болиш шарт!")]
    public string Description { get; set; }
}
