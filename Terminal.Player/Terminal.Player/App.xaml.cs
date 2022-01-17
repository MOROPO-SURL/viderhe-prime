namespace SmartSolucionesCuba.Viderhe.Terminal.Player
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            InitializeComponent();

            Xamarin.Forms.DependencyService.Register<Support.Presentation.Helpers.Facade.IImageUriFactoryHelper, Helpers.ImageUriFactoryHelper>();

            FFImageLoading.ImageService.Instance.Initialize(new FFImageLoading.Config.Configuration
            {
                HttpClient = new System.Net.Http.HttpClient(new Handlers.SecureImageHttpClientHandler())
            });

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
