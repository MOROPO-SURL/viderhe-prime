using SmartSolucionesCuba.Viderhe.Extend.OHCC.RailwayMuseum.Templating.Descriptors;
using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace SmartSolucionesCuba.Viderhe.Extend.OHCC.RailwayMuseum.Templating.Dashboard.Presentation.Providers
{
    public class PlacardConfigurationPageProvider : Common.Templating.Dashboard.Presentation.Providers.Facade.IPlacardConfigurationPageProviderFacade
    {
        public Xamarin.Forms.Page GetCreationEntryPointPageByDescriptor(Common.Templating.Descriptors.ITemplateDescriptor templateDescriptor, Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade notificationPresenter, SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider, Support.Presentation.Helpers.Facade.IExceptionHandleHelperFacade exceptionHandleHelper, System.Action completedActionCallback)
        {
            if (templateDescriptor is InfinityVideoTemplateDescriptor)
            {
                return new Essential.Templating.Dashboard.Presentation.Pages.PrePickVideoPage(exceptionHandleHelper, templateDescriptor, completedActionCallback, (Support.Presentation.Models.View.VideoViewModel model, Extend.Common.Templating.Descriptors.ITemplateDescriptor _templateDescriptor, System.Action completeCallbackAction) =>
                {
                    Xamarin.Forms.Application.Current.MainPage.NavigationPushPageAsync(new Pages.InfinityVideoPlacardCreatePage(model, _templateDescriptor as InfinityVideoTemplateDescriptor, httpClientHelperProvider, exceptionHandleHelper, notificationPresenter, completeCallbackAction));
                }, httpClientHelperProvider);
            }

            if (templateDescriptor is CornerTileVideoPresentingVideoTemplateDescriptor)
            {
                return new Pages.CornerTileVideoPresentingVideoPlacardConfigurationPage(httpClientHelperProvider, exceptionHandleHelper, notificationPresenter, completedActionCallback);
            }

            return null;
        }

        public Xamarin.Forms.Page GetEditionPageByDescriptor(Support.Presentation.Models.View.PlacardViewModel placardViewModel, Support.Presentation.Providers.Facade.ICloudServerProviderFacade cloudServerProvider, Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade notificationPresenter, SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider, Support.Presentation.Helpers.Facade.IExceptionHandleHelperFacade exceptionHandleHelper, System.Action completedActionCallback)
        {
            if (placardViewModel.Template.Id == InfinityVideoTemplateDescriptor.ID)
            {
                return new Pages.InfinityVideoPlacardEditPage(placardViewModel, cloudServerProvider, httpClientHelperProvider, exceptionHandleHelper, notificationPresenter, completedActionCallback);
            }

            if (placardViewModel.Template.Id == CornerTileVideoPresentingVideoTemplateDescriptor.ID)
            {
                return new Pages.CornerTileVideoPresentingVideoPlacardConfigurationPage(placardViewModel, httpClientHelperProvider, exceptionHandleHelper, notificationPresenter, completedActionCallback);
            }

            return null;
        }
    }
}
