using SmartSolucionesCuba.Viderhe.Support.Presentation.Extensions;
using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Pages
{
    public partial class ManageImagesPage : XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.ManageImagesPageModel>
    {
        private int currentItemsPageNumber;

        public ManageImagesPage() : base(new Models.Page.ManageImagesPageModel { IsBusy = true })
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

        private void ImageOverviewDetailButton_Clicked(object sender, System.EventArgs e)
        {
            this.NavigationPushPageAsync(new Support.Presentation.Dashboard.Pages.ImageDetailPage((sender as Xamarin.Forms.BindableObject).BindingContext as Support.Presentation.Models.View.ImageViewModel, model =>
            {
                this.NavigationPushPageAsync(new EditImagePage(model));
            }));
        }

        private void ImageOverviewDeleteButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            DisplayAlert("Confirmar", "Está seguro que desea eliminar esta imagen?", "Aceptar", "Cancelar").ContinueWith(async task =>
            {
                if (!task.Result) return;

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    BindingContextPageModel.IsBusy = true;
                });

                var client = new Cloud.Api.Connect.Clients.ImagesApiConnectClient(Managers.SessionManager.Instance.HttpClientHelper);

                System.Exception failException = null;

                try
                {
                    await client.DeleteAsync(((sender as Xamarin.Forms.BindableObject).BindingContext as Support.Presentation.Models.View.ImageViewModel).Id);
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
                        Presenters.NotificationPresenter.Instance.PresentNotification("Información", "Imagen eliminada con éxito.");

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
                                fileSelector = new string[] { "image/*" };

                                break;

                            case Xamarin.Forms.Device.UWP:
                                fileSelector = new string[] { ".jpg", ".jpeg", ".png", ".bmp" };

                                break;

                            case Xamarin.Forms.Device.WPF:
                            default:
                                fileSelector = new string[] { "Imágenes Soportadas|*.jpg;*.jpeg;*.png;*.bmp" };

                                break;
                        }

                        var targetFileData = await Plugin.FilePicker.CrossFilePicker.Current.PickFile(fileSelector);

                        if (targetFileData != null)
                        {
                            await this.NavigationPushPageAsync(new AddImagePage(targetFileData, RequiredReload_Callback));
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

            var imagesApiConnectClient = new Cloud.Api.Connect.Clients.ImagesApiConnectClient(Managers.SessionManager.Instance.HttpClientHelper);

            imagesApiConnectClient.GetPageAsync(++currentItemsPageNumber).ContinueWith(task =>
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