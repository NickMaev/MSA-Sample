using AutoMapper;
using Contracts.Dtos;

namespace Components.CTeleport
{
    public class CTeleportMappingProfile : Profile
	{
		public CTeleportMappingProfile()
		{
			CreateMap<CTeleportAirportLocationDto, GeoPointDto>();

			CreateMap<CTeleportAirportInfoDto, AirportInfoDto>()
				.ForMember(
                    x => x.GeoPoint,
                    cfg =>
                    {
                        cfg.PreCondition(x => x.Location != null);
                        cfg.MapFrom(x => x.Location);
                    }
                );
        }
	}
}