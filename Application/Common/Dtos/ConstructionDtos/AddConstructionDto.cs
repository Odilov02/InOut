﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Dtos.ConstructionDtos;
#nullable disable
public class AddConstructionDto
{
    [NotNull]
    [MaxLength(50)]
    public string FullName { get; set; }
    [NotNull]
    public Guid UserId { get; set; }
    [NotNull]
    [MaxLength (200)]
    public string Description { get; set; }
    public long ContractPrice { get; set; }
}
