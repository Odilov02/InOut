using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Dtos.InDtos;

public class ConfirmationIn
{
    [NotNull]
    public Guid Id { get; set; }
    [NotNull]
    public bool IsConfirmed { get; set; }
}
