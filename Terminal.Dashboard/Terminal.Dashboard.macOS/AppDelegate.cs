using AppKit;
using Foundation;

namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.macOS
{
	[Register ("AppDelegate")]
	public class AppDelegate : Xamarin.Forms.Platform.MacOS.FormsApplicationDelegate
	{
        private readonly NSWindow window;

        public AppDelegate()
        {
            var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

            var rect = new CoreGraphics.CGRect(200, 1000, 1024, 600);

            window = new NSWindow(rect, style, NSBackingStore.Buffered, false)
            {
                Title = "VIDERHE Control", // choose your own Title here
                TitleVisibility = NSWindowTitleVisibility.Hidden,
                MinSize = new CoreGraphics.CGSize(1024, 600)
            };
        }

        public override NSWindow MainWindow
        {
            get { return window; }
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            Xamarin.Forms.Forms.Init();

            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            Rg.Plugins.Popup.Popup.Init();

            Helpers.ToastNotificationHelper.Init();

            LoadApplication(new App());

            base.DidFinishLaunching(notification);
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }

        public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender)
        {
            return true;
        }
    }
}

