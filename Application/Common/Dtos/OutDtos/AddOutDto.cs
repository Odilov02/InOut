using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Dtos.OutDtos;

public class AddOutDto
{
    [NotNull]
    [MaxLength(200)]
    public string Reason { get; set; }
    [NotNull]
    [Range(1, long.MaxValue)]
    public long Price { get; set; }
    [NotNull]
    public Guid OutTypeId { get; set; }
}
