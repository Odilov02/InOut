using System.ComponentModel.DataAnnotations;

namespace Application.Common.Dtos.SpendDtos;

public class ConfirmationSpend
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public bool IsConfirmed { get; set; }
}
