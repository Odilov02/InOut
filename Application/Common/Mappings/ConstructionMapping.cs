using Application.Common.Dtos.ConstructionDtos;

namespace Application.Common.Mappings;

public class ConstructionMapping : Profile
{
    public ConstructionMapping()
    {
        CreateMap<AddConstructionDto, Construction>();
    }
}
