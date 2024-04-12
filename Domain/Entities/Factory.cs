namespace Domain.Entities;
#nullable disable

public class Factory:BaseAuditableEntity
{
    public string FullName { get; set; }
    public string Description { get; set; }
    public long In { get; set; }
    public long Spend { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime SpendDate { get; set; }
    public DateTime InDate { get; set; }
    public virtual ICollection<In?> Ins { get; set; }
    public virtual ICollection<Spend?> Spends { get; set; }
}
