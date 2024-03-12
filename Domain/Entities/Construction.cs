namespace Domain.Entities;
#nullable disable
public class Construction:BaseEntity
{
    public string  FullName { get; set; }
    public string Description { get; set; }
    public long In { get; set; }
    public long Spend { get; set; }
    public DateTime CreatedDate { get; set; }=DateTime.Now;
    public DateTime SpendDate { get; set; } = DateTime.Now;
    public DateTime InDate { get; set; }= DateTime.Now;
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<AdminSpend?> AdminSpends { get; set; }
}
