namespace Domain.Entities;
#nullable disable

public class In : BaseAuditableEntity
{
    public string Reason { get; set; }
    public long Price { get; set; }
    public bool IsConfirmed { get; set; }
    public DateTime Date { get; set; }
    public Guid? UserId { get; set; }
    public virtual User? User { get; set; }
    public Guid? FactoryId { get; set; }
    public virtual Factory? Factory   { get; set; }
    public Guid? ConstructionId { get; set; }
    public virtual Construction? Construction { get; set; }
}
