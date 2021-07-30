namespace SmartSolucionesCuba.Viderhe.Extend.SocialWelfare.Templating.Dashboard.Presentation.Providers
{
    public class PlacardConfigurationPageProvider : Common.Templating.Dashboard.Presentation.Providers.Facade.IPlacardConfigurationPageProviderFacade
    {
        public Xamarin.Forms.Page GetCreationEntryPointPageByDescriptor(Common.Templating.Descriptors.ITemplateDescriptor templateDescriptor, Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade notificationPresenter, SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider, Support.Presentation.Helpers.Facade.IExceptionHandleHelperFacade exceptionHandleHelper, System.Action completedActionCallback)
        {
            if (templateDescriptor is Descriptors.InmersiveCityWeatherOverviewTemplateDescriptor)
            {
                return new Pages.InmersiveCityWeatherOverviewConfigurationPage(httpClientHelperProvider, exceptionHandleHelper, notificationPresenter, completedActionCallback);
            }

            return null;
        }

        public Xamarin.Forms.Page GetEditionPageByDescriptor(Support.Presentation.Models.View.PlacardViewModel placardViewModel, Support.Presentation.Providers.Facade.ICloudServerProviderFacade cloudServerProvider, Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade notificationPresenter, SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider, Support.Presentation.Helpers.Facade.IExceptionHandleHelperFacade exceptionHandleHelper, System.Action completedActionCallback)
        {
            if (placardViewModel.Template.Id == Descriptors.InmersiveCityWeatherOverviewTemplateDescriptor.ID)
            {
                return new Pages.InmersiveCityWeatherOverviewConfigurationPage(placardViewModel, httpClientHelperProvider, exceptionHandleHelper, notificationPresenter, completedActionCallback);
            }

            return null;
        }
    }
}
