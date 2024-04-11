using System.ComponentModel.DataAnnotations;

namespace WebUI.Models;
#nullable disable
public class AddFile
{
    [Required(ErrorMessage="Файл номи киритилиши шарт!")]
    public string Name { get; set; }

    [Required(ErrorMessage="Файл киритилиши шарт!")]
    public IFormFile FormFile { get; set; }

    [Required(ErrorMessage = "Файл хақида маьлумот киритилиши шарт!")]
    [MaxLength(200, ErrorMessage = "Файл хақида маьлумот узунлиги 200 тадан кам болиши шарт!")]
    public string Description { get; set; }
}