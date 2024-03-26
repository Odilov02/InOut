using Application.Common.Dtos.SpendDtos;

namespace Application.Common.Mappings;

public class SpendMapping : Profile
{
    public SpendMapping()
    {
        CreateMap<AddSpendDto, Spend>().ForMember(dest => dest.SpendType, opt => opt.MapFrom(src => new SpendType() { Id=src.SpendTypeId??new Guid()}));
        CreateMap<ConfirmationSpend,Spend>().ReverseMap();
    }
}
