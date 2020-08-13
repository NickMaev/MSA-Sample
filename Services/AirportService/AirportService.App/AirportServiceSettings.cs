using Components.CTeleport;
using Shared.Configuration;

namespace AirportService.App
{
    public class AirportServiceSettings : AppSettingsBase
    {
        public CTeleportDataProviderSettings CTeleportDataProvider { get; set; }
    }
}