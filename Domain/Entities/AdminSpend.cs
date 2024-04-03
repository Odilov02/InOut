using System.ComponentModel.DataAnnotations;
namespace Domain.Entities;
#nullable disable
public class AdminSpend : BaseAuditableEntity
{
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Чиқим суммаси киритиши шарт!")]
    [Range(1, long.MaxValue, ErrorMessage = "Чиқим суммаси киритиши шарт!")]
    public long? Price { get; set; }

    [Required(ErrorMessage = "Чиқим сабаби киритиши шарт!")]
    public string Reason { get; set; }
    [Required(ErrorMessage = "Чиқим тури танланишлиги шарт!")]
    public Guid? SpendTypeId { get; set; }
    public virtual SpendType SpendType { get; set; }
    [Required(ErrorMessage = "Пул тури танланишлиги  шарт!")]
    public bool? IsCash { get; set; }
    public Guid ConstructionId { get; set; }
    public virtual Construction Construction { get; set; }
}
