using AutoMapper;
using SolucionPeakHours.Models;
using SolucionPeakHours.Shared.Common;
using SolucionPeakHours.Shared.ProgHours;

namespace SolucionPeakHours.Mappings
{
    public class ProgHourProfile : Profile
    {
        public ProgHourProfile()
        {
            CreateMap<ProgHourEntity, ProgHourDTO>().ReverseMap(); 
            CreateMap<PaginationResult<ProgHourEntity>, PaginationResult<ProgHourDTO>>().ReverseMap(); 

        }
    }
}
