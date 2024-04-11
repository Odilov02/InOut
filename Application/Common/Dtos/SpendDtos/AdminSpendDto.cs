using System.ComponentModel.DataAnnotations;

namespace Application.Common.Dtos.SpendDtos;


public class AdminSpendDto
{
    public DateTime Date { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Чиқим суммаси киритиши шарт!")]
    [Range(1, long.MaxValue, ErrorMessage = "Чиқим суммаси киритиши шарт!")]
    public long? Price { get; set; }

    [Required(ErrorMessage = "Чиқим сабаби киритиши шарт!")]
    public string Reason { get; set; }
    [Required(ErrorMessage = "Чиқим тури танланишлиги шарт!")]
    public Guid? SpendTypeId { get; set; }
    [Required(ErrorMessage = "Пул тури танланишлиги  шарт!")]
    public bool? IsCash { get; set; }
    [Required]
    public Guid? ConstructionId { get; set; }
}
