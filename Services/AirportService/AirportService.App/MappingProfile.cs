using AutoMapper;
using Contracts.Dtos;
using Contracts.MessageBus.AirportService;

namespace AirportService.App
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AirportInfoDto, GetAirportInfoResponse>()
                .ForMember(
                    x => x.Latitude, 
                    cfg => {
                        cfg.PreCondition(x => x.GeoPoint != null);
                        cfg.MapFrom(x => x.GeoPoint.Latitude);
                    }
                )
                .ForMember(
                    x => x.Longtitude,
                    cfg => {
                        cfg.PreCondition(x => x.GeoPoint != null);
                        cfg.MapFrom(x => x.GeoPoint.Longtitude);
                    }
                );
        }
    }
}