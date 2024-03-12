namespace Domain.Entities;

public class Spend : BaseAuditableEntity
{
    public string Reason { get; set; }
    public long Price { get; set; }
    public bool IsConfirmed { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
    public virtual SpendType SpendType { get; set; }
}
