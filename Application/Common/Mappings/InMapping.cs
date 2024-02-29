using Application.Common.Dtos.InDtos;

namespace Application.Common.Mappings;

public class InMapping : Profile
{
    public InMapping()
    {
        CreateMap<AddInDto, In>();
    }
}