using Application.Common.DTOs.UserDTOs;

namespace Application.Common.Mappings;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<User, UserRegisterDto>().ReverseMap();
    }
}
