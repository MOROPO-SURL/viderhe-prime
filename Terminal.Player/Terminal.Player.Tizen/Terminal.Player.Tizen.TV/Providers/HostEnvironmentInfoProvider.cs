[assembly: Xamarin.Forms.Dependency(typeof(SmartSolucionesCuba.Viderhe.Terminal.Player.Tizen.TV.Providers.HostEnvironmentInfoProvider))]

namespace SmartSolucionesCuba.Viderhe.Terminal.Player.Tizen.TV.Providers
{
    public class HostEnvironmentInfoProvider : Player.Providers.Facade.IHostEnvironmentInfoProviderFacade
    {
        public string GetHardwareIdentifier()
        {
            return global::Tizen.Network.Connection.ConnectionManager.GetMacAddress(global::Tizen.Network.Connection.ConnectionType.Ethernet);
        }
    }
}
