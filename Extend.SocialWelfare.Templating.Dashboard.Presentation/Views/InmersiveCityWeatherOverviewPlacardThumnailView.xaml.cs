namespace SmartSolucionesCuba.Viderhe.Extend.SocialWelfare.Templating.Dashboard.Presentation.Views
{
    public partial class InmersiveCityWeatherOverviewPlacardThumnailView : XFToolKit.Mvvm.Views.ContentView
    {
        public InmersiveCityWeatherOverviewPlacardThumnailView(Support.Presentation.Models.View.PlacardViewModel model, Support.Presentation.Providers.Facade.ICloudServerProviderFacade cloudServerProvider)
        {
            InitializeComponent();

            var context = Newtonsoft.Json.JsonConvert.DeserializeObject<Templating.Presentation.Models.Context.InmersiveCityWeatherOverviewContextModel>(model.SerializedContext);

            BackgroundImage.Source = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ThumbnailUriFactory.BuildThumbnailUri(cloudServerProvider, context.BackgroundCityImage.ThumbnailId));

            BindingContext = model;
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            BackgroundImage.Source = null;
        }
    }
}