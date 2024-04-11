using System.ComponentModel.DataAnnotations;

namespace Application.Common.Dtos.OutDto;

public class AddOutDto
{

    [Required(ErrorMessage = "Чиқим сабаби киритиш шарт!")]
    [MaxLength(200, ErrorMessage = "Чиқим сабаби узунлиги 200 тадан кам болиш шарт!")]
    public string Reason { get; set; }


    [Required(ErrorMessage = "Чиқим суммаси киритиш шарт!")]
    [Range(1, long.MaxValue, ErrorMessage = "Чиқим суммасини киритиш шарт!")]
    public long? Price { get; set; }


    [Required(ErrorMessage = "Обьект  танлаш  шарт!")]
    public Guid? ConstructionId { get; set; }
}
