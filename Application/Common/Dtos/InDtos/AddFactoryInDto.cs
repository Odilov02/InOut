using System.ComponentModel.DataAnnotations;

namespace Application.Common.Dtos.InDtos
{
    public class AddFactoryInDto
    {

        [Required(ErrorMessage = "Чиқим сабаби киритилиши шарт!")]
        [MaxLength(200, ErrorMessage = "Чиқим сабаби узунлиги 200 тадан кам болиши шарт!")]
        public string Reason { get; set; }


        [Required(ErrorMessage = "Чиқим сумма киритилиши шарт!")]
        [Range(1, long.MaxValue, ErrorMessage = "Чиқим сумма киритилиши шарт!")]
        public long? Price { get; set; }


        [Required(ErrorMessage = "Обьект танланишлиги шарт!")]
        public Guid? ConstructionId { get; set; }
        
        [Required]
        public Guid? FactoryId { get; set; }
    }
}
