namespace SmartSolucionesCuba.Viderhe.Terminal.Player.Tizen.TV
{
    public class Program : Xamarin.Forms.Platform.Tizen.FormsApplication
    {
        protected override void OnCreate()
        {
            base.OnCreate();

            MainWindow.BackgroundColor = ElmSharp.Color.Black;
            MainWindow.NoBlank = true;

            Xamarin.Essentials.Platform.Init(MainWindow);

            ElmSharp.Utility.AppendGlobalFontPath(DirectoryInfo.Resource);

            LoadApplication(new App());

            _ = System.Reflection.Assembly.GetAssembly(typeof(Extend.Essential.Templating.Descriptors.SimplePictureTemplateDescriptor));
            _ = System.Reflection.Assembly.GetAssembly(typeof(Extend.Essential.Templating.Presentation.Providers.PlacardPresentationViewProvider));
            _ = System.Reflection.Assembly.GetAssembly(typeof(Extend.Essential.Overlaying.Descriptors.SimpleHighlightsOverlayDescriptor));
            _ = System.Reflection.Assembly.GetAssembly(typeof(Extend.Essential.Overlaying.Presentation.Providers.OverlayPresentationViewProvider));
            _ = System.Reflection.Assembly.GetAssembly(typeof(Extend.Essential.Transitioning.Transitions.ArriveTransitionDescriptor));
            _ = System.Reflection.Assembly.GetAssembly(typeof(Extend.Essential.Transitioning.Presentation.Providers.TransitionProvider));
        }

        static void Main(string[] args)
        {
            var app = new Program();

            var option = new Xamarin.Forms.InitializationOptions(app) 
            { 
                UseDeviceIndependentPixel = true
            };

            Xamarin.Forms.Forms.Init(option);

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(app);

            app.Run(args);
        }
    }
}
