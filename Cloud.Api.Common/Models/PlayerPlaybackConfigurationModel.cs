namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class PlayerPlaybackConfigurationModel
    {
        public PlacardDisplayModel[] Placards { get; set; }

        public ScheduledRelaunchDisplayModel[] ScheduledRelaunches { get; set; }

        public string AuthenticationType { get; set; }

        public string AuthenticationToken { get; set; }
    }
}
