using System.ComponentModel.DataAnnotations;

namespace Application.Common.Dtos.SpendDtos;


public class AdminSpendDto
{
    public DateTime Date { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Чиқим суммасини киритиш шарт!")]
    [Range(1, long.MaxValue, ErrorMessage = "Чиқим суммасини киритиш шарт!")]
    public long? Price { get; set; }

    [Required(ErrorMessage = "Чиқим сабабини киритиш шарт!")]
    public string Reason { get; set; }
    [Required(ErrorMessage = "Чиқим турини танлаш шарт!")]
    public Guid? SpendTypeId { get; set; }
    [Required(ErrorMessage = "Пул турини танлаш  шарт!")]
    public bool? IsCash { get; set; }
    [Required]
    public Guid? ConstructionId { get; set; }
}
