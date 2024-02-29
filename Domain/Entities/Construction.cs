namespace Domain.Entities;
#nullable disable
public class Construction:BaseEntity
{
    public string  FullName { get; set; }
    public string Description { get; set; }
    public int In { get; set; }
    public int Out { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime OutDate { get; set; }
    public DateTime InDate { get; set; }
    public Guid UserId { get; set; }
    public virtual User User { get; set; }
}
