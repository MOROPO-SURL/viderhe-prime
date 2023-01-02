namespace SmartSolucionesCuba.Viderhe.Terminal.Player.Pages
{
    public partial class StartingPage : Xamarin.Forms.ContentPage
    {
        public StartingPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            System.Threading.Tasks.Task.Delay(System.TimeSpan.FromSeconds(1)).ContinueWith(_ =>
            {
                while (Width <= 0)
                {
                    System.Threading.Thread.Sleep(System.TimeSpan.FromSeconds(1));

                    continue;
                }

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    Xamarin.Forms.Application.Current.MainPage = new MainPage();
                });
            });
        }
    }
}