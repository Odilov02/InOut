using System.ComponentModel.DataAnnotations;

namespace WebUI.Models;
#nullable disable
public class AddFile
{
    [Required(ErrorMessage="Файл номи киритиш шарт!")]
    public string Name { get; set; }

    [Required(ErrorMessage="Файл киритиш шарт!")]
    public IFormFile FormFile { get; set; }

    [Required(ErrorMessage = "Файл хақида маьлумот киритиш шарт!")]
    [MaxLength(200, ErrorMessage = "Файл хақида маьлумот узунлиги 200 тадан кам болиш шарт!")]
    public string Description { get; set; }
}