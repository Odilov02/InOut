namespace Application.Common.Models;
#nullable disable
public class AllSpend
{
    public string AdminOrUser { get; set; }
    public DateTime Date { get; set; }
    public long Price { get; set; }
    public string SpendType { get; set; }
    public string Reason { get; set; }
}
