using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Dtos.InDtos;

public class AddInDto
{
    [NotNull]
    [MaxLength(200)]
    public string Reason { get; set; }
    [NotNull]
    [Range(1, long.MaxValue)]
    public long Price { get; set; }
    [NotNull]
    public Guid ConstructionId { get; set; }
}
