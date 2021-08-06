[assembly: Xamarin.Forms.Dependency(typeof(SmartSolucionesCuba.Viderhe.Terminal.Player.iOS.Providers.HostEnvironmentInfoProvider))]

namespace SmartSolucionesCuba.Viderhe.Terminal.Player.iOS.Providers
{
    public class HostEnvironmentInfoProvider : Player.Providers.Facade.IHostEnvironmentInfoProviderFacade
    {
        public string GetHardwareIdentifier()
        {
            return UIKit.UIDevice.CurrentDevice.IdentifierForVendor.AsString();
        }
    }
}
