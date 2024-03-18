using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Dtos.ConstructionDtos;
#nullable disable
public class AddConstructionDto
{
    [Required(ErrorMessage = "Обьект номи киритиши шарт!")]
    [MaxLength(50,ErrorMessage = "Обьект номи узунлиги 50 тадан кам болиши шарт!")]
    public string FullName { get; set; }
    public Guid UserId { get; set; }
    [Required(ErrorMessage = "Обьект хақида маьлумот киритиши шарт!")]
    [MaxLength(50, ErrorMessage = "Обьект хақида маьлумот узунлиги 200 тадан кам болиши шарт!")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Шартнома суммаси киритиши шарт!")]
    public long ContractPrice { get; set; }
}
