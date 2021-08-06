[assembly: Xamarin.Forms.Dependency(typeof(SmartSolucionesCuba.Viderhe.Terminal.Player.iOS.Helpers.PlatformControlHelper))]

namespace SmartSolucionesCuba.Viderhe.Terminal.Player.iOS.Helpers
{
    public class PlatformControlHelper : Player.Helpers.Facade.IPlatformControlHelperFacade
    {
        public int AudioLevel
        {
            get
            {
                // TODO Implement expected behavior!!
                return 100;
            }
        }

        public bool IsAudioEnabled
        {
            get
            {
                // TODO Implement expected behavior!!
                return true;
            }
        }

        public void SetAudioEnabled(bool enabled)
        {
            // TODO Implement expected behavior!!
            throw new System.NotImplementedException();
        }

        public void SetAudioLevel(int percent)
        {
            // TODO Implement expected behavior!!
            throw new System.NotImplementedException();
        }

        public System.Threading.Tasks.Task ShutdownAsync(bool restart)
        {
            // TODO Implement expected behavior!!
            throw new System.NotImplementedException();
        }

        public void Close(bool restart)
        {
            // TODO Implement expected behavior!!
            throw new System.NotImplementedException();
        }
    }
}
