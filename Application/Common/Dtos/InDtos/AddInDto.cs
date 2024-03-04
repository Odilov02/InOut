using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Dtos.InDtos;

public class AddInDto
{
    [NotNull]
    [MaxLength(200)]
    public string Reason { get; set; }
    [NotNull]
    [Range(1, long.MinValue)]
    public long Price { get; set; }
}
