using System.ComponentModel.DataAnnotations;

namespace Application.Common.Dtos.SpendDtos;

public class UpdateSpendDto
{
    [Required]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Чиқим сабабини киритиш шарт!")]
    [MaxLength(200, ErrorMessage = "Чиқим сабаби узунлиги 200 тадан кам болиши шарт!")]
    public string Reason { get; set; }


    [Required(ErrorMessage = "Чиқим суммани киритиш шарт!")]
    [Range(1, long.MaxValue, ErrorMessage = "Чиқим суммани киритиш шарт!")]
    public long? Price { get; set; }


    [Required(ErrorMessage = "Чиқим турини танлаш шарт!")]
    public Guid? SpendTypeId { get; set; }
}
