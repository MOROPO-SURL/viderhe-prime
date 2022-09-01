[assembly: Xamarin.Forms.Dependency(typeof(SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Droid.Managers.ApplicationManager))]

namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Droid.Managers
{
    public class ApplicationManager : Dashboard.Managers.Facade.IApplicationManagerFacade
    {
        public void Quit()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }

        public void RequestRequiredPermissions()
        {
        }
        
        public string GetUnhandleExceptionMessage(System.Exception exception)
        {
            if (exception is Java.Net.UnknownHostException) return "No se puede acceder al servidor. Compruebe si la dirección especificada es correcta.";

            return null;
        }
    }
}