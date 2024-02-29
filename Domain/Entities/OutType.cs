namespace Domain.Entities;

public class OutType:BaseEntity
{
    public string Name { get; set; }
    public string Descraption { get; set; }
    public virtual ICollection<Out?> Outs { get; set; }
}
