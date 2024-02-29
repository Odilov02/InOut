using Application.Common.Dtos.OutDtos;

namespace Application.Common.Mappings;

public class OutMapping : Profile
{
    public OutMapping()
    {
        CreateMap<AddOutDto, Out>().ForMember(dest => dest.OutType, opt => opt.MapFrom(src => new OutType() { Id=src.OutTypeId}));
    }
}
