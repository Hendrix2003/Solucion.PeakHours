
using SolucionPeakHours.Entities;

namespace SolucionPeakHours.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(UserIdentityEntity user);  
    }
}
