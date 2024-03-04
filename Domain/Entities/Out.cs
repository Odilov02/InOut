namespace Domain.Entities;

public class Out : BaseAuditableEntity
{
    public string Reason { get; set; }
    public long Price { get; set; }
    public bool IsConfirmed { get; set; }
    public DateTime Date { get; set; }
    public virtual User User { get; set; }
    public virtual OutType OutType { get; set; }
}
