using Application.Common.Dtos.SpendDtos;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Models;

public class SpendsConfirming
{
    [Required]
    public Guid ConstructionId { get; set; }
    [Required]
    public List<Spend> Spends { get; set; }
}
