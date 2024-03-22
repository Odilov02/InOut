using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Dtos.InDtos;

public class ConfirmationIn
{
    [Required]
    public Guid? Id { get; set; }
    [Required]
    public bool IsConfirmed { get; set; }
}
