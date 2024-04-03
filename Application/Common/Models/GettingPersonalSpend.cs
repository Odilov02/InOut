namespace Application.Common.Models;

public class GettingPersonalSpend
{
    public Construction Construction { get; set; }
    public DateTime Date { get; set; }
    public long Price { get; set; }
    public SpendType SpendType { get; set; }
    public string Reason { get; set; }
}
