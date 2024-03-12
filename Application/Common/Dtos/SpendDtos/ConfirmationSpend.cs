using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Dtos.SpendDtos;

public class ConfirmationSpend
{
    [NotNull]
    public Guid Id { get; set; }
    [NotNull]
    public bool IsConfirmed { get; set; }
}
