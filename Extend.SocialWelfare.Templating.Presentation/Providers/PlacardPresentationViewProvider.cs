using SmartSolucionesCuba.Viderhe.Extend.SocialWelfare.Templating.Descriptors;
using SmartSolucionesCuba.Viderhe.Extend.SocialWelfare.Templating.Presentation.Models.Context;
using SmartSolucionesCuba.Viderhe.Extend.SocialWelfare.Templating.Presentation.Views;

namespace SmartSolucionesCuba.Viderhe.Extend.SocialWelfare.Templating.Presentation.Providers
{
    public class PlacardPresentationViewProvider : Common.Templating.Presentation.Providers.Facade.IPlacardPresentationViewProviderFacade
    {
        public Common.Presentation.Views.BasePlacardPresentationView GetFor(Support.Presentation.Models.View.PlacardViewModel model, Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider, Support.Presentation.Providers.Facade.ICloudServerProviderFacade cloudServerProvider)
        {
            if (model.Template.Id == InmersiveCityWeatherOverviewTemplateDescriptor.ID)
            {
                var context = Newtonsoft.Json.JsonConvert.DeserializeObject<InmersiveCityWeatherOverviewContextModel>(model.SerializedContext);

                var protocol = context.IsWeatherCacheMicroServerRequireSecureConnection ? "https" : "http";

                return new InmersiveCityWeatherOverviewView(new Models.View.InmersiveCityWeatherOverviewViewModel
                {
                    RequireUnobstructedPresetation = context.RequireUnobstructedPresetation,
                    ScreenPresentationTime = context.ScreenPresentationTime,
                    BackgroundCityImageSource = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(cloudServerProvider, context.BackgroundCityImage.Id)),
                    Service = context.Service,
                    City = context.City,
                    ServiceBaseUri = new System.Uri(string.Format("{0}://{1}/api/", protocol, context.WeatherCacheMicroServerHost))
                });
            }

            return null;
        }
    }
}
