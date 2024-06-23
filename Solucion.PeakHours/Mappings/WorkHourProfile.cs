using AutoMapper;
using SolucionPeakHours.Models;
using SolucionPeakHours.Shared.Common;
using SolucionPeakHours.Shared.CompletedHours;

namespace SolucionPeakHours.Mappings
{
    public class WorkHourProfile : Profile
    {
        public WorkHourProfile()
        {
            CreateMap<WorkHourEntity, WorkHourDTO>().ReverseMap();
            CreateMap<PaginationResult<WorkHourEntity>, PaginationResult<WorkHourDTO>>().ReverseMap();
        }
    }
}
