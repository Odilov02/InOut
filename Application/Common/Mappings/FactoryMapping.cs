using Application.Common.Dtos.FactoryDtos;

namespace Application.Common.Mappings;

public class FactoryMapping : Profile
{
    public FactoryMapping()
    {
        CreateMap<AddFactoryDto, Factory>();
    }
}