﻿using System.ComponentModel.DataAnnotations;

namespace Application.Common.Dtos.ConstructionDtos;
#nullable disable
public class AddConstructionDto
{
    [Required(ErrorMessage = "Обьект номи киритилиши шарт!")]
    [MaxLength(50, ErrorMessage = "Обьект номи узунлиги 50 тадан кам болиши шарт!")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Шартнома суммаси киритилиши шарт!")]
    public long? ContractPrice { get; set; }
    [Required(ErrorMessage = "Масьул шахс танланишлиги шарт!")]
    public Guid? UserId { get; set; }

    [Required(ErrorMessage = "Обьект хақида маьлумот киритилиши шарт!")]
    [MaxLength(200, ErrorMessage = "Обьект хақида маьлумот узунлиги 200 тадан кам болиши шарт!")]
    public string Description { get; set; }
}
