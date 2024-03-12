namespace Domain.Entities;

public class SpendType:BaseEntity
{
    public string Name { get; set; }
    public string Descraption { get; set; }
    public virtual ICollection<Spend?> Spends { get; set; }
}
