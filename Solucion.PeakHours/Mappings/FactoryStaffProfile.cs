using AutoMapper;
using SolucionPeakHours.Models;
using SolucionPeakHours.Shared.FactoryStaff;

namespace SolucionPeakHours.Mappings
{
    public class FactoryStaffProfile : Profile
    {
        public FactoryStaffProfile()
        {
            CreateMap<FactoryStaffEntity, FactoryStaffDTO>().ReverseMap(); 
        }
    }
}
