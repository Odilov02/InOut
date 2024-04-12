namespace Domain.Entities;
#nullable disable

public class Document
{
    public Guid Id { get; set; }
    public string ImgUrl { get; set; }
    public string Name { get; set; }
    public DateTime Time { get; set; }
    public string Description { get; set; }
}
