using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Models;
#nullable disable
public class AllSpend
{
    public Guid? Id { get; set; }
    public string AdminOrUser { get; set; }
    public DateTime Date { get; set; }
    public long Price { get; set; }
    public string SpendType { get; set; }
    public string Reason { get; set; }
}
