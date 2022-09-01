namespace SmartSolucionesCuba.Viderhe.Terminal.Player.Views
{
    public partial class ConfigurationStepStartingDetailView : XFToolKit.Mvvm.Views.ContentView
    {
        private const string SERVER_SECURITY_SSL_OPTION = "Protegidas por SSL (https)";
        private const string SERVER_SECURITY_CLEARTEXT_OPTION = "Texto plano (http)";

        public ConfigurationStepStartingDetailView()
        {
            InitializeComponent();

            PresentServerAddress();
        }

        public void PresentFallbackOptions()
        {
            FallbackOptionsLayout.IsVisible = true;
        }

        private void ChangeServerButton_Clicked(object sender, System.EventArgs eventArgs)
        {
            Xamarin.Forms.Application.Current.MainPage.DisplayPromptAsync("Nombre del Servidor", "Especifique la dirección de red del servidor VIDERHE Cloud:", "Aceptar", "Cancelar", "Ej. cloud.viderhe.net o 192.168.0.2").ContinueWith(async promptTask =>
            {
                if (string.IsNullOrEmpty(promptTask.Result) || string.IsNullOrWhiteSpace(promptTask.Result)) return;

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    Xamarin.Forms.Application.Current.MainPage.DisplayActionSheet("Indique el modo de conexión con el servidor VIDERHE Cloud:", "Cancelar", null, new string[] { SERVER_SECURITY_SSL_OPTION, SERVER_SECURITY_CLEARTEXT_OPTION }).ContinueWith(actionSheetTask =>
                    {
                        switch (actionSheetTask.Result)
                        {
                            case SERVER_SECURITY_SSL_OPTION:
                                Support.Presentation.Managers.SessionMemoryManager.Instance.Server = promptTask.Result;
                                Support.Presentation.Managers.SessionMemoryManager.Instance.UseSSL = true;

                                Xamarin.Forms.Device.BeginInvokeOnMainThread(PresentServerAddress);

                                break;

                            case SERVER_SECURITY_CLEARTEXT_OPTION:
                                Support.Presentation.Managers.SessionMemoryManager.Instance.Server = promptTask.Result;
                                Support.Presentation.Managers.SessionMemoryManager.Instance.UseSSL = false;

                                Xamarin.Forms.Device.BeginInvokeOnMainThread(PresentServerAddress);

                                break;
                        }                        
                    });
                });                
            });
        }

        private void OpenSystemSettingsButton_Clicked(object sender, System.EventArgs eventArgs) 
        {
            Xamarin.Forms.DependencyService.Get<Helpers.Facade.IPlatformControlHelperFacade>().OpenSystemSettings();
        }

        private void PresentServerAddress()
        {
            ServerAddressLabel.Text = Support.Presentation.Managers.SessionMemoryManager.Instance.Server;

            FallbackOptionsLayout.IsVisible = false;
        }
    }
}