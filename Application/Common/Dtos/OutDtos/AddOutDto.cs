namespace Application.Common.Dtos.OutDtos;

public class AddOutDto
{
    public string Reason { get; set; }
    public int Price { get; set; }
    public Guid OutTypeId { get; set; }
}
