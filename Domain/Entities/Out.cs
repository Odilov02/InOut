namespace Domain.Entities;
#nullable disable
public class Out : BaseEntity
{
    public long Price { get; set; }
    public string Reason { get; set; }
    public DateTime Date { get; set; }
    public Guid ConstructionId { get; set; }
    public virtual Construction Construction { get; set; }
}
