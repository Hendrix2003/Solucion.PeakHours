using AutoMapper;
using SolucionPeakHours.Entities;
using SolucionPeakHours.Shared.UserAccount;

namespace SolucionPeakHours.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDTO, UserIdentityEntity>().ReverseMap();
            CreateMap<LoginDTO, UserIdentityEntity>().ReverseMap();
            CreateMap<AuthResponse, UserIdentityEntity>().ReverseMap();
            CreateMap<UserIdentityEntity, UserDTO>().ReverseMap();
        }
    }
}
