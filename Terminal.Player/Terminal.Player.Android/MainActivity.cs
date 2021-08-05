
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

namespace SmartSolucionesCuba.Viderhe.Terminal.Player.Droid
{
    [Activity(
        Label = "Viderhe Player", 
        Icon = "@mipmap/ic_launcher",
        Theme = "@style/AppTheme.NoActionBar", 
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(
        actions: new string[] { "android.intent.action.MAIN" },
        Categories = new string[] 
        {
            "android.intent.category.HOME",
            "android.intent.category.DEFAULT",
            "android.intent.category.LEANBACK_LAUNCHER"
        })]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Window.AddFlags(Android.Views.WindowManagerFlags.Fullscreen | Android.Views.WindowManagerFlags.KeepScreenOn);

            Xamarin.Forms.Forms.SetFlags("Shapes_Experimental");

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);

            Rg.Plugins.Popup.Popup.Init(this);

            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            
            LoadApplication(new App());

            var a = new Views.Renderer.ExoVideoPlayerViewRenderer(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);

            var uiOptions = (int)Window.DecorView.SystemUiVisibility;

            uiOptions |= (int)Android.Views.SystemUiFlags.ImmersiveSticky;
            uiOptions |= (int)Android.Views.SystemUiFlags.LayoutStable;
            uiOptions |= (int)Android.Views.SystemUiFlags.LayoutHideNavigation;
            uiOptions |= (int)Android.Views.SystemUiFlags.LayoutFullscreen;
            uiOptions |= (int)Android.Views.SystemUiFlags.Fullscreen;
            uiOptions |= (int)Android.Views.SystemUiFlags.HideNavigation;

            Window.DecorView.SystemUiVisibility = (Android.Views.StatusBarVisibility)uiOptions;
        }
    }
}