using AutoMapper;
using Contracts.Dtos;
using Contracts.MessageBus.AirportService;

namespace PublicApi.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GetAirportInfoResponse, GeoPointDto>()
                .ForMember(x => x.Latitude, cfg => {
                    cfg.PreCondition(x => x.Latitude.HasValue);
                    cfg.MapFrom(x => x.Latitude);
                })
                .ForMember(x => x.Longtitude, cfg => {
                    cfg.PreCondition(x => x.Longtitude.HasValue);
                    cfg.MapFrom(x => x.Longtitude);
                });
            CreateMap<GetAirportInfoResponse, AirportInfoDto>()
                .ForMember(x => x.GeoPoint, cfg => cfg.MapFrom(x => x));
        }
    }
}