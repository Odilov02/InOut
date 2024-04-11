using Application.Common.Dtos.OutDto;

namespace Application.Common.Mappings;

public class OutMapping : Profile
{
    public OutMapping()
    {
        CreateMap<AddOutDto, Out>();
    }
}
