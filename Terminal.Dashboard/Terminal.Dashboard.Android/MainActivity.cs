
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Droid
{
    [Activity(Label = "VIDERHE Control", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            //var isTablet = false;
            
            //if (!isTablet)
            //{
            //    RequestedOrientation = ScreenOrientation.Landscape;
            //}

            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            ImageCircle.Forms.Plugin.Droid.ImageCircleRenderer.Init();

            Rg.Plugins.Popup.Popup.Init(this);

            Xamarin.Forms.DependencyService.Register<Plugin.Toasts.ToastNotification>();

            Plugin.Toasts.ToastNotification.Init(this);

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}