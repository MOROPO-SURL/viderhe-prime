using SmartSolucionesCuba.Viderhe.Support.Presentation.Extensions;
using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Pages
{
    public partial class ManageVideosPage : XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.ManageVideosPageModel>
    {
        private int currentItemsPageNumber;

        public ManageVideosPage() : base(new Models.Page.ManageVideosPageModel { IsBusy = true })
        {
            InitializeComponent();

            IsEnabled = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (IsEnabled) return;

            RequiredReload_Callback();

            IsEnabled = true;
        }

        private void VideoOverviewDetailButton_Clicked(object sender, System.EventArgs e)
        {
            this.NavigationPushPageAsync(new Support.Presentation.Dashboard.Pages.VideoDetailPage((sender as Xamarin.Forms.BindableObject).BindingContext as Support.Presentation.Models.View.VideoViewModel, model =>
            {
                this.NavigationPushPageAsync(new EditVideoPage(model));
            }));
        }

        private void VideoOverviewDeleteButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            DisplayAlert("Confirmar", "Está seguro que desea eliminar este video?", "Aceptar", "Cancelar").ContinueWith(async task =>
            {
                if (!task.Result) return;

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = true;
                });

                var client = new Cloud.Api.Connect.Clients.VideosApiConnectClient(Managers.SessionManager.Instance.HttpClientHelper);

                System.Exception failException = null;

                try
                {
                    await client.DeleteAsync(((sender as Xamarin.Forms.BindableObject).BindingContext as Support.Presentation.Models.View.VideoViewModel).Id);
                }
                catch (System.Exception ex)
                {
                    failException = ex;
                }

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = false;

                    if (failException == null)
                    {
                        Presenters.NotificationPresenter.Instance.PresentNotification("Información", "Video eliminado con éxito.");

                        RequiredReload_Callback();
                    }
                    else
                    {
                        (Xamarin.Forms.Application.Current as Support.Presentation.Helpers.Facade.IExceptionHandleHelperFacade).HandleException(failException);
                    }
                });
            });
        }

        private void AddToolbarItem_Activated(object sender, System.EventArgs e)
        {
            if (BindingContextPageModel.IsBusy) return;

            BindingContextPageModel.IsBusy = true;

            System.Threading.Tasks.Task.Run(() =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        string[] fileSelector = null;

                        switch (Xamarin.Forms.Device.RuntimePlatform)
                        {
                            case Xamarin.Forms.Device.Android:
                                fileSelector = new string[] { "video/*" };

                                break;

                            case Xamarin.Forms.Device.UWP:
                                fileSelector = new string[] { ".mp4", ".mkv", ".avi", ".mpg", ".flv" };

                                break;

                            case Xamarin.Forms.Device.WPF:
                            default:
                                fileSelector = new string[] { "Videos Soportados|*.mp4;*.mkv;*.avi;*.mpg", "*.flv" };

                                break;
                        }

                        var targetFileData = await Plugin.FilePicker.CrossFilePicker.Current.PickFile(fileSelector);

                        if (targetFileData != null)
                        {
                            await this.NavigationPushPageAsync(new AddVideoPage(targetFileData, RequiredReload_Callback));
                        }
                    }
                    finally
                    {
                        BindingContextPageModel.IsBusy = false;
                    }
                });
            });
        }

        private void ReloadToolbarItem_Clicked(object sender, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel.IsBusy) return;

            RequiredReload_Callback();
        }

        private void LoadNextPageButtton_Clicked(object sender, System.EventArgs eventArgs)
        {
            LoadNextItemsPage();
        }

        private void RequiredReload_Callback()
        {
            LoadNextItemsPage(true);
        }

        private void LoadNextItemsPage(bool restart = false)
        {
            BindingContextPageModel.IsBusy = true;

            if (restart) currentItemsPageNumber = 0;

            var videosApiConnectClient = new Cloud.Api.Connect.Clients.VideosApiConnectClient(Managers.SessionManager.Instance.HttpClientHelper);

            videosApiConnectClient.GetPageAsync(++currentItemsPageNumber).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = false;

                    if (task.Exception != null)
                    {
                        (Xamarin.Forms.Application.Current as Support.Presentation.Helpers.Facade.IExceptionHandleHelperFacade).HandleException(task.Exception);
                    }
                    else
                    {
                        if (currentItemsPageNumber == 1)
                        {
                            BindingContextPageModel.Items.Clear();
                        }

                        foreach (var item in task.Result.List)
                        {
                            BindingContextPageModel.Items.Add(item.ToViewModel());
                        }

                        BindingContextPageModel.HasMoreItems = task.Result.HasNext && task.Result.List.Count > 0;
                    }
                });
            });
        }
    }
}