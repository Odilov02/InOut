using System.ComponentModel.DataAnnotations;

namespace Application.Common.Dtos.InDtos
{
    public class AddFactoryInDto
    {

        [Required(ErrorMessage = "Кирим сабабини киритиш шарт!")]
        [MaxLength(200, ErrorMessage = "Кирим сабаби узунлиги 200 тадан кам болиш шарт!")]
        public string Reason { get; set; }


        [Required(ErrorMessage = "Кирим сумма киритиш шарт!")]
        [Range(1, long.MaxValue, ErrorMessage = "Кирим сумма киритиш шарт!")]
        public long? Price { get; set; }


        [Required(ErrorMessage = "Обьект танлаш шарт!")]
        public Guid? ConstructionId { get; set; }
        
        [Required]
        public Guid? FactoryId { get; set; }
    }
}
