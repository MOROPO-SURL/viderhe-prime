using SmartSolucionesCuba.Viderhe.Support.Presentation.Extensions;
using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Dashboard.Pages
{
    public partial class ImagePickPage : XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.ImagePickPageModel>
    {
        private System.Action<Presentation.Models.View.ImageViewModel> imagePickedCallbackAction;
        private SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider;
        private Helpers.Facade.IExceptionHandleHelperFacade exceptionHandleHelper;

        private Presentation.Models.View.ImageViewModel except;

        private int currentImagesPageNumber;

        public ImagePickPage(Helpers.Facade.IExceptionHandleHelperFacade exceptionHandleHelper, SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade httpClientHelperProvider, System.Action<Presentation.Models.View.ImageViewModel> imagePickedCallbackAction, Presentation.Models.View.ImageViewModel except = null)
            : base(new Models.Page.ImagePickPageModel
            {
                IsBusy = true
            })
        {
            InitializeComponent();

            this.except = except;
            this.imagePickedCallbackAction = imagePickedCallbackAction;
            this.httpClientHelperProvider = httpClientHelperProvider;
            this.exceptionHandleHelper = exceptionHandleHelper;

            IsEnabled = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (IsEnabled) return;

            LoadNextImagesPage(true);

            IsEnabled = true;
        }

        private void ImageOverviewPickButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            BindingContextPageModel.IsBusy = true;

            var targetImageViewModel = (sender as Xamarin.Forms.BindableObject).BindingContext as Presentation.Models.View.ImageViewModel;

            imagePickedCallbackAction.Invoke(targetImageViewModel);

            this.NavigationPopPageAsync();
        }

        private void ImageOverviewDetailButton_Clicked(object sender, System.EventArgs e)
        {
            this.NavigationPushPageAsync(new ImageDetailPage((sender as Xamarin.Forms.BindableObject).BindingContext as Presentation.Models.View.ImageViewModel));
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
                        if (currentImagesPageNumber == 1)
                        {
                            BindingContextPageModel.Images.Clear();
                        }

                        foreach (var item in task.Result.List)
                        {
                            if (except != null && except.Id == item.Id) continue;

                            BindingContextPageModel.Images.Add(item.ToViewModel());
                        }

                        BindingContextPageModel.HasMoreImages = task.Result.HasNext && task.Result.List.Count > 0;
                    }
                });
            });
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            imagePickedCallbackAction = null;
            httpClientHelperProvider = null;
            exceptionHandleHelper = null;
            except = null;
        }
    }
}