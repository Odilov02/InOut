namespace Domain.Common;
public class BaseAuditableEntity : BaseEntity
{
    public DateTime Created { get; set; }
    public DateTime Lasted { get; set; }
    public string? CreatedBy { get; set; }
    public string? LastedBy { get; set; }
}