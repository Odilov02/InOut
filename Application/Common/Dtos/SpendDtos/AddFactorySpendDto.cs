﻿using System.ComponentModel.DataAnnotations;

namespace Application.Common.Dtos.SpendDtos;

public class AddFactorySpendDto
{
    [Required(ErrorMessage = "Чиқим сабабини киритиш шарт!")]
    [MaxLength(200, ErrorMessage = "Чиқим сабаби узунлиги 200 тадан кам болиш шарт!")]
    public string Reason { get; set; }


    [Required(ErrorMessage = "Чиқим суммани киритиш шарт!")]
    [Range(1, long.MaxValue, ErrorMessage = "Чиқим сумма киритиш шарт!")]
    public long? Price { get; set; }


    [Required(ErrorMessage = "Обьект танлаш шарт!")]
    public Guid? ConstructionId { get; set; }

    [Required]
    public Guid? FactoryId { get; set; }
    [Required(ErrorMessage = "Чиқим турини танлаш шарт!")]
    public Guid? SpendTypeId { get; set; }
}
