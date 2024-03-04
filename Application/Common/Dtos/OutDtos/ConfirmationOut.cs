using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Dtos.OutDtos;

public class ConfirmationOut
{
    [NotNull]
    public Guid Id { get; set; }
    [NotNull]
    public bool IsConfirm { get; set; }
}
