namespace Domain.Entities;
#nullable disable
public class AdminSpend: BaseAuditableEntity
{
    public DateTime CreatedDate { get; set; }=DateTime.Now;
    public long Price { get; set; }
    public string Reason { get; set; }
    public Guid ConstructionId { get; set; }
    public virtual Construction Construction { get; set; }
}
