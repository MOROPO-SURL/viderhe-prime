using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Pages
{
    public partial class AccountIdentityConfigurationPage : 
        XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.AccountIdentityConfigurationPageModel>, 
        Support.Presentation.Dashboard.Pages.Contract.IValidableFormPage,
        Contract.IOverlayContextConfigurationPage
    {
        private readonly System.Collections.Generic.List<Support.Presentation.Dashboard.Behaviors.Contract.IFormFieldValidationBehavior> validationBehaviors;

        private bool pageModelHasChanges;
        private Extend.Common.Overlaying.Descriptors.IOverlayDescriptor recoverableOverlay;
        private string recoverableSerializedOverlayContext;

        private Plugin.FilePicker.Abstractions.FileData newLogoFileData;
        private Plugin.FilePicker.Abstractions.FileData newHorizontalSplashFileData;
        private Plugin.FilePicker.Abstractions.FileData newVerticalSplashFileData;

        public AccountIdentityConfigurationPage() : base(new Models.Page.AccountIdentityConfigurationPageModel
        {
            IsBusy = true
        })
        {
            validationBehaviors = new System.Collections.Generic.List<Support.Presentation.Dashboard.Behaviors.Contract.IFormFieldValidationBehavior>();

            InitializeComponent();

            BindingContextPageModel.PropertyChanged += PageModelPropertyChanged;

            IsEnabled = false;

            recoverableOverlay = BindingContextPageModel.DefaultOverlay;
            recoverableSerializedOverlayContext = BindingContextPageModel.SerializedOverlayContext;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!IsEnabled)
            {
                var accountApiClient = new Cloud.Api.Connect.Clients.AccountApiConnectClient(Managers.SessionManager.Instance);

                accountApiClient.GetCurrentAsync().ContinueWith(task =>
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        if (task.Exception != null)
                        {
                            this.NavigationPopPageAsync();

                            (Xamarin.Forms.Application.Current as Support.Presentation.Helpers.Facade.IExceptionHandleHelperFacade).HandleException(task.Exception);
                        }
                        else
                        {
                            BindingContextPageModel.Name = task.Result.Name;
                            BindingContextPageModel.PrimaryColor = Xamarin.Forms.Color.FromHex(task.Result.PrimaryColor);
                            BindingContextPageModel.SecoundaryColor = Xamarin.Forms.Color.FromHex(task.Result.SecoundaryColor);
                            BindingContextPageModel.LogoImageSource = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(task.Result.LogoImageId));
                            BindingContextPageModel.HorizontalSplashImageSource = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(task.Result.HorizontalSplashImageId));
                            BindingContextPageModel.VerticalSplashImageSource = Xamarin.Forms.ImageSource.FromUri(Support.Presentation.Factories.ImageUriFactory.BuildImageUri(task.Result.VerticalSplashImageId));
                            BindingContextPageModel.DefaultOverlay = task.Result.DefaultOverlayId == null ? null : Support.Extend.Overlaying.Providers.OverlayDescriptorProvider.Instance.GetDescriptorById(task.Result.DefaultOverlayId.Value);
                            BindingContextPageModel.SerializedOverlayContext = task.Result.SerializedOverlayContext;

                            var found = false;
                            var index = -1;

                            foreach (var item in BindingContextPageModel.AvailableOverlays)
                            {
                                if (!found)
                                {
                                    index++;

                                    found = BindingContextPageModel.DefaultOverlay != null && item.Id == BindingContextPageModel.DefaultOverlay.Id;
                                }
                            }

                            if (found) OverlayPicker.SelectedIndex = index;

                            BindingContextPageModel.IsBusy = false;

                            Managers.SessionManager.Instance.SyncronizeRequiredAccountInformation(BindingContextPageModel.Name, BindingContextPageModel.PrimaryColor, BindingContextPageModel.SecoundaryColor, task.Result.LogoImageId, task.Result.HorizontalSplashImageId, task.Result.VerticalSplashImageId, BindingContextPageModel.DefaultOverlay, BindingContextPageModel.SerializedOverlayContext);

                            pageModelHasChanges = false;

                            IsEnabled = true;
                        }
                    });                    
                });
            }
        }

        private void PageModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            pageModelHasChanges = true;
        }

        private void OverlayPicker_SelectedIndexChanged(object target, System.EventArgs eventArgs)
        {
            if (BindingContextPageModel == null) return;

            BindingContextPageModel.DefaultOverlay = (target as Xamarin.Forms.Picker).SelectedItem as Extend.Common.Overlaying.Descriptors.IOverlayDescriptor;
            BindingContextPageModel.SerializedOverlayContext = null;

            if (recoverableOverlay != null && BindingContextPageModel.DefaultOverlay.Id == recoverableOverlay.Id)
            {
                BindingContextPageModel.SerializedOverlayContext = recoverableSerializedOverlayContext;
            }

            if (BindingContextPageModel.SerializedOverlayContext == null && BindingContextPageModel.DefaultOverlay != null)
            {
                BindingContextPageModel.SerializedOverlayContext = Providers.OverlayDefaultSerializedContextProvider.Instance.GetOverlayDefaultContextModel(BindingContextPageModel.DefaultOverlay);
            }
        }

        private void SelectedOverlayClearButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            OverlayPicker.SelectedIndex = -1;
        }

        private void OverlayConfigureButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            var page = Providers.OverlayConfigurationPageProvider.Instance.GetConfigurationPageByDescriptor(BindingContextPageModel.DefaultOverlay, BindingContextPageModel.SerializedOverlayContext, OverlayConfigurationSuccesfulComplete_Callback);
            
            if (page != null)
            {
                this.NavigationPushPageAsync(page);
            }
            else
            {
                Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Aviso", "La Caratula seleccionada no require configuración.");
            }
        }

        private void ApplyToolbarItem_Activated(object sender, System.EventArgs eventArgs)
        {
            if (!pageModelHasChanges)
            {
                this.NavigationPopPageAsync();

                return;
            }

            BindingContextPageModel.ResetValidationState();

            foreach (var validationBehavior in validationBehaviors)
            {
                var result = validationBehavior.Evaluate();

                if (!BindingContextPageModel.IsValid) continue;

                if (!result) BindingContextPageModel.IsValid = false;
            }

            if (!BindingContextPageModel.IsValid) return;

            BindingContextPageModel.IsBusy = true;

            Managers.SessionManager.Instance.UpdateAccountInformationAsync(BindingContextPageModel.Name, BindingContextPageModel.PrimaryColor, BindingContextPageModel.SecoundaryColor, newLogoFileData, newHorizontalSplashFileData, newVerticalSplashFileData, BindingContextPageModel.DefaultOverlay, BindingContextPageModel.SerializedOverlayContext).ContinueWith(task =>
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    if (task.Exception == null)
                    {
                        Xamarin.Forms.DependencyService.Get<Support.Presentation.Dashboard.Presenters.Facade.INotificationPresenterFacade>().PresentNotification("Información", "Cambios aplicados con éxito.");

                        this.NavigationPopPageAsync();
                    }
                    else
                    {
                        BindingContextPageModel.IsBusy = false;

                        (Xamarin.Forms.Application.Current as Support.Presentation.Helpers.Facade.IExceptionHandleHelperFacade).HandleException(task.Exception);
                    }
                });
            });
        }

        private void CancelToolbarItem_Activated(object sender, System.EventArgs eventArgs)
        {
            if (!pageModelHasChanges)
            {
                this.NavigationPopPageAsync();

                return;
            }

            DisplayAlert("Confirmar", "Está seguro que desea regresar sin guardar los cambios?", "Regresar", "Cancelar").ContinueWith(task =>
            {
                if (!task.Result) return;

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    this.NavigationPopPageAsync();
                });
            });
        }

        private void PickLogoImageButton_Clicked(object sender, System.EventArgs eventArgs)
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

            Plugin.FilePicker.CrossFilePicker.Current.PickFile(fileSelector).ContinueWith(task =>
            {
                if (task.Result == null) return;

                newLogoFileData = task.Result;

                var imageFormat = System.IO.Path.GetExtension(task.Result.FilePath).Substring(1).ToUpper();

                if (imageFormat.Equals("jpg", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    imageFormat = "JPEG";
                }

                using (var imageFileStream = task.Result.GetStream())
                {
                    var imageData = new byte[imageFileStream.Length];

                    imageFileStream.Read(imageData, 0, imageData.Length);

                    BindingContextPageModel.LogoImageSource = Xamarin.Forms.ImageSource.FromStream(() => new System.IO.MemoryStream(imageData));
                }
            });
        }

        private void PickHorizontalSplashImageButton_Clicked(object sender, System.EventArgs eventArgs)
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

            Plugin.FilePicker.CrossFilePicker.Current.PickFile(fileSelector).ContinueWith(task =>
            {
                if (task.Result == null) return;

                newHorizontalSplashFileData = task.Result;

                var imageFormat = System.IO.Path.GetExtension(task.Result.FilePath).Substring(1).ToUpper();

                if (imageFormat.Equals("jpg", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    imageFormat = "JPEG";
                }

                using (var imageFileStream = task.Result.GetStream())
                {
                    var imageData = new byte[imageFileStream.Length];

                    imageFileStream.Read(imageData, 0, imageData.Length);

                    BindingContextPageModel.HorizontalSplashImageSource = Xamarin.Forms.ImageSource.FromStream(() => new System.IO.MemoryStream(imageData));
                }
            });
        }

        private void PickVerticalSplashImageButton_Clicked(object sender, System.EventArgs eventArgs)
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

            Plugin.FilePicker.CrossFilePicker.Current.PickFile(fileSelector).ContinueWith(task =>
            {
                if (task.Result == null) return;

                newVerticalSplashFileData = task.Result;

                var imageFormat = System.IO.Path.GetExtension(task.Result.FilePath).Substring(1).ToUpper();

                if (imageFormat.Equals("jpg", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    imageFormat = "JPEG";
                }

                using (var imageFileStream = task.Result.GetStream())
                {
                    var imageData = new byte[imageFileStream.Length];

                    imageFileStream.Read(imageData, 0, imageData.Length);

                    BindingContextPageModel.VerticalSplashImageSource = Xamarin.Forms.ImageSource.FromStream(() => new System.IO.MemoryStream(imageData));
                }
            });
        }

        private void OverlayConfigurationSuccesfulComplete_Callback(string serializedContext)
        {
            BindingContextPageModel.SerializedOverlayContext = serializedContext;
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            BindingContextPageModel.PropertyChanged -= PageModelPropertyChanged;
        }

        #region IValidableFormPage implementation

        public void AddValidationBehavior(Support.Presentation.Dashboard.Behaviors.Contract.IFormFieldValidationBehavior validationBehavior)
        {
            validationBehaviors.Add(validationBehavior);
        }

        public void RemoveValidationBehavior(Support.Presentation.Dashboard.Behaviors.Contract.IFormFieldValidationBehavior validationBehavior)
        {
            validationBehaviors.Remove(validationBehavior);
        }

        public void AddValidationMessage(string label, string message)
        {
            BindingContextPageModel.ValidationMessages.Add(new Support.Presentation.Dashboard.Models.View.ValidationMessageViewModel
            {
                Caption = label,
                Message = message
            });
        }

        #endregion

        #region IOverlayContextConfigurationPage implementation

        public string GetCurrentSerializedOverlayContext()
        {
            return BindingContextPageModel.SerializedOverlayContext;
        }

        #endregion
    }
}