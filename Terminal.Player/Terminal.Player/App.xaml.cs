namespace SmartSolucionesCuba.Viderhe.Terminal.Player
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            InitializeComponent();

            Xamarin.Forms.DependencyService.Register<Extend.Common.Presentation.Providers.Facade.IAccountInformationProviderFacade, Providers.AccountInformationProvider>();
            Xamarin.Forms.DependencyService.RegisterSingleton<Support.Presentation.Providers.Facade.ICloudServerProviderFacade>(Support.Presentation.Managers.SessionMemoryManager.Instance);
            Xamarin.Forms.DependencyService.RegisterSingleton<SAPRESSC.Core.Api.Connect.Providers.Facade.IHttpClientHelperProviderFacade>(Managers.SessionManager.Instance);

            Support.Presentation.Helpers.ImageUriFactoryHelper.Instance.Initialize();

            MainPage = new Pages.MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
