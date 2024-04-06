using Application.Common.Dtos.InDtos;

namespace Application.Common.Mappings;

public class InMapping : Profile
{
    public InMapping()
    {
        CreateMap<AddInDto, In>();
        CreateMap<In, UpdateInDto>();
        CreateMap<PersonalIn, In>();
        CreateMap<In, ConfirmationIn>();
    }
}