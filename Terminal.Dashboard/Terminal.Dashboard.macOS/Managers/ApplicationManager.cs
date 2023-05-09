[assembly: Xamarin.Forms.Dependency(typeof(SmartSolucionesCuba.Viderhe.Terminal.Dashboard.macOS.Managers.ApplicationManager))]

namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.macOS.Managers
{
	internal class ApplicationManager : Dashboard.Managers.Facade.IApplicationManagerFacade
    {
        public string GetUnhandleExceptionMessage(System.Exception exception)
        {
            return null;
        }

        public void Quit()
        {
            AppKit.NSApplication.SharedApplication.Terminate(null);
        }

        public void RequestRequiredPermissions()
        {
        }
    }
}

