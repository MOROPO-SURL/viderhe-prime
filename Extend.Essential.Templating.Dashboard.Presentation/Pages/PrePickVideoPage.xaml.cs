using SmartSolucionesCuba.Viderhe.Support.Presentation.Extensions;
using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Dashboard.Presentation.Pages
{
    public partial class PrePickVideoPage : XFToolKit.Mvvm.Pages.BindingContextedContentPage<Support.Presentation.Dashboard.Models.Page.VideoPickPageModel>
    {
        private Common.Templating.Descriptors.ITemplateDescriptor templateDescriptor;
        private System.Action successfulCompleteCallbackAction;
        private System.Action<Support.Presentation.Models.View.VideoViewModel, Common.Templating.Descriptors.ITemplateDescriptor, System.Action> videoPickedCallbackAction;

        private int currentVideosPageNumber;

        public PrePickVideoPage(Common.Templating.Descriptors.ITemplateDescriptor templateDescriptor, System.Action successfulCompleteCallbackAction, System.Action<Support.Presentation.Models.View.VideoViewModel, Common.Templating.Descriptors.ITemplateDescriptor, System.Action> videoPickedCallbackAction)
            : base(new Support.Presentation.Dashboard.Models.Page.VideoPickPageModel { IsBusy = true })
        {
            InitializeComponent();

            this.templateDescriptor = templateDescriptor;
            this.successfulCompleteCallbackAction = successfulCompleteCallbackAction;
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
            var targetImageViewModel = (sender as Xamarin.Forms.BindableObject).BindingContext as Support.Presentation.Models.View.VideoViewModel;

            videoPickedCallbackAction.Invoke(targetImageViewModel, templateDescriptor, OnSuccessfulComplete);
        }

        private void VideoOverviewDetailButton_Clicked(object sender, System.EventArgs e)
        {
            this.NavigationPushPageAsync(new Support.Presentation.Dashboard.Pages.VideoDetailPage((sender as Xamarin.Forms.BindableObject).BindingContext as Support.Presentation.Models.View.VideoViewModel));
        }

        private void OnSuccessfulComplete()
        {
            this.NavigationPopPageAsync();

            successfulCompleteCallbackAction.Invoke();
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
                        Xamarin.Forms.DependencyService.Get<Support.Presentation.Helpers.Facade.IExceptionHandleHelperFacade>().HandleException(task.Exception);
                    }
                    else
                    {
                        if (currentVideosPageNumber == 1)
                        {
                            BindingContextPageModel.Videos.Clear();
                        }

                        foreach (var item in task.Result.List)
                        {
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

            templateDescriptor = null;
            successfulCompleteCallbackAction = null;
            videoPickedCallbackAction = null;
        }
    }
}