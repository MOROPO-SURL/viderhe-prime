using SmartSolucionesCuba.Viderhe.Support.Presentation.Extensions;
using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Dashboard.Pages
{
    public partial class ImagesPickPage : XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.ImagesPickPageModel>
    {
        private Presentation.Models.View.ImageViewModel[] exclusionImages;
        private System.Action<Presentation.Models.View.ImageViewModel[]> imagesSelectedCallbackAction;
        private SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider;
        private Helpers.Facade.IExceptionHandleHelperFacade exceptionHandleHelper;

        private int currentImagesPageNumber;

        public ImagesPickPage(Presentation.Models.View.ImageViewModel[] exclusionImages, Helpers.Facade.IExceptionHandleHelperFacade exceptionHandleHelper, SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider, System.Action<Presentation.Models.View.ImageViewModel[]> imagesSelectedCallbackAction)
            : base(new Models.Page.ImagesPickPageModel { IsBusy = true })
        {
            InitializeComponent();

            this.exclusionImages = exclusionImages;
            this.httpClientHelperProvider = httpClientHelperProvider;
            this.exceptionHandleHelper = exceptionHandleHelper;
            this.imagesSelectedCallbackAction = imagesSelectedCallbackAction;

            IsEnabled = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (IsEnabled) return;

            LoadNextImagesPage(true);

            IsEnabled = true;
        }

        private void ApplyToolbarItem_Activated(object sender, System.EventArgs e)
        {
            imagesSelectedCallbackAction.Invoke(BindingContextPageModel.SelectableImages.Where(m => m.Selected).Select(m => m.Model).ToArray());

            this.NavigationPopPageAsync();
        }

        private void CancelToolbarItem_Activated(object sender, System.EventArgs e)
        {
            this.NavigationPopPageAsync();
        }

        private void SelectionToggleLabelTapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            ((sender as Xamarin.Forms.BindableObject).BindingContext as Models.View.SelectableModelViewModel<Presentation.Models.View.ImageViewModel>).Selected = !((sender as Xamarin.Forms.BindableObject).BindingContext as Models.View.SelectableModelViewModel<Presentation.Models.View.ImageViewModel>).Selected;
        }

        private void ImageOverviewDetailButton_Clicked(object sender, System.EventArgs e)
        {
            this.NavigationPushPageAsync(new ImageDetailPage(((sender as Xamarin.Forms.BindableObject).BindingContext as Models.View.SelectableModelViewModel<Presentation.Models.View.ImageViewModel>).Model));
        }

        private void LoadNextPageButtton_Clicked(object sender, System.EventArgs eventArgs)
        {
            LoadNextImagesPage();
        }

        private void LoadNextImagesPage(bool restart = false)
        {
            BindingContextPageModel.IsBusy = true;

            if (restart) currentImagesPageNumber = 0;

            var imagesApiConnectClient = new Cloud.Api.Connect.Clients.ImagesApiConnectClient(httpClientHelperProvider.HttpClientHelper);

            imagesApiConnectClient.GetPageAsync(++currentImagesPageNumber).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = false;

                    if (task.Exception != null)
                    {
                        exceptionHandleHelper.HandleException(task.Exception);
                    }
                    else
                    {
                        var relevantImages = task.Result.List.Select(m => m.ToViewModel()).ToArray().Except(exclusionImages).Select(item => new Models.View.SelectableModelViewModel<Presentation.Models.View.ImageViewModel>
                        {
                            Model = item
                        });

                        if (currentImagesPageNumber == 1)
                        {
                            BindingContextPageModel.SelectableImages.Clear();
                        }

                        foreach (var item in relevantImages)
                        {
                            BindingContextPageModel.SelectableImages.Add(item);
                        }

                        BindingContextPageModel.HasMoreImages = task.Result.HasNext && task.Result.List.Count > 0;
                    }
                });
            });
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            exclusionImages = null;
            httpClientHelperProvider = null;
            exceptionHandleHelper = null;
            imagesSelectedCallbackAction = null;
        }
    }
}