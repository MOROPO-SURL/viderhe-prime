using SmartSolucionesCuba.Viderhe.Support.Presentation.Extensions;
using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Dashboard.Pages
{
    public partial class VideoPickPage : XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.VideoPickPageModel>
    {
        private System.Action<Presentation.Models.View.VideoViewModel> videoPickedCallbackAction;

        private Presentation.Models.View.VideoViewModel except;

        private int currentVideosPageNumber;

        public VideoPickPage(System.Action<Presentation.Models.View.VideoViewModel> videoPickedCallbackAction, Presentation.Models.View.VideoViewModel except = null)
            : base(new Models.Page.VideoPickPageModel { IsBusy = true })
        {
            InitializeComponent();

            this.except = except;
            this.videoPickedCallbackAction = videoPickedCallbackAction;

            IsEnabled = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (IsEnabled) return;

            LoadNextVideosPage(true);

            IsEnabled = true;
        }

        private void VideoOverviewPickButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            var targetImageViewModel = (sender as Xamarin.Forms.BindableObject).BindingContext as Presentation.Models.View.VideoViewModel;

            videoPickedCallbackAction.Invoke(targetImageViewModel);

            this.NavigationPopPageAsync();
        }

        private void VideoOverviewDetailButton_Clicked(object sender, System.EventArgs e)
        {
            this.NavigationPushPageAsync(new VideoDetailPage((sender as Xamarin.Forms.BindableObject).BindingContext as Presentation.Models.View.VideoViewModel));
        }

        private void LoadNextPageButtton_Clicked(object sender, System.EventArgs eventArgs)
        {
            LoadNextVideosPage();
        }

        private void LoadNextVideosPage(bool restart = false)
        {
            BindingContextPageModel.IsBusy = true;

            if (restart) currentVideosPageNumber = 0;

            var videosApiConnectClient = new Cloud.Api.Connect.Clients.VideosApiConnectClient(Xamarin.Forms.DependencyService.Get<MOROPO.SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade>().HttpClientHelper);

            videosApiConnectClient.GetPageAsync(++currentVideosPageNumber).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = false;

                    if (task.Exception != null)
                    {
                        Xamarin.Forms.DependencyService.Get<Presentation.Helpers.Facade.IExceptionHandleHelperFacade>().HandleException(task.Exception);
                    }
                    else
                    {
                        if (currentVideosPageNumber == 1)
                        {
                            BindingContextPageModel.Videos.Clear();
                        }

                        foreach (var item in task.Result.List)
                        {
                            if (except != null && except.Id == item.Id) continue;

                            BindingContextPageModel.Videos.Add(item.ToViewModel());
                        }

                        BindingContextPageModel.HasMoreVideos = task.Result.HasNext && task.Result.List.Count > 0;
                    }
                });
            });
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            except = null;
            videoPickedCallbackAction = null;
        }
    }
}