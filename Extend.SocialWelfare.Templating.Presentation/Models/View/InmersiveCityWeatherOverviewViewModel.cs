namespace SmartSolucionesCuba.Viderhe.Extend.SocialWelfare.Templating.Presentation.Models.View
{
    public class InmersiveCityWeatherOverviewViewModel : Common.Templating.Presentation.Models.View.AbstractScreenTimedPlacardViewModel
    {
        private bool isBusy;

        private Weather.Api.Common.Models.WeatherOverviewModel weatherOverview;

        public InmersiveCityWeatherOverviewViewModel()
        {
            Now = System.DateTime.Now;
        }

        public System.Uri ServiceBaseUri { get; set; }

        public System.DateTime Now { get; set; }

        public Enumerations.WeatherDataProvisionService Service { get; set; }

        public Weather.Api.Common.Models.CityModel City { get; set; }

        public Weather.Api.Common.Models.WeatherOverviewModel WeatherOverview { get => weatherOverview; set => SetProperty(ref weatherOverview, value); }

        public Xamarin.Forms.ImageSource BackgroundCityImageSource { get; set; }

        public bool IsBusy { get => isBusy; set => SetProperty(ref isBusy, value); }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            BackgroundCityImageSource = null;
            WeatherOverview = null;
        }
    }
}
