using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Dtos.InDtos;

public class UpdateInDto
{
    [Required]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Кирим сабаби киритилиши шарт!")]
    [MaxLength(200, ErrorMessage = "Кирим сабаби узунлиги 200 тадан кам болиши шарт!")]
    public string Reason { get; set; }

    [Required(ErrorMessage = "Кирим суммаси киритилиши шарт!")]
    public long? Price { get; set; }
}
