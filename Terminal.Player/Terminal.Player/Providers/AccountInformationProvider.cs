namespace SmartSolucionesCuba.Viderhe.Terminal.Player.Providers
{
    public class AccountInformationProvider : Extend.Common.Presentation.Providers.Facade.IAccountInformationProviderFacade
    {
        public System.Guid Id => Managers.SessionManager.Instance.Account.Id;

        public string Name => Managers.SessionManager.Instance.Account.Name;

        public Xamarin.Forms.ImageSource LogoImageSource => Managers.SessionManager.Instance.Account.LogoImageSource;

        public Xamarin.Forms.ImageSource HorizontalSplashImageSource => Managers.SessionManager.Instance.Account.HorizontalSplashImageSource;

        public Xamarin.Forms.ImageSource VerticalSplashImageSource => Managers.SessionManager.Instance.Account.VerticalSplashImageSource;

        public Xamarin.Forms.Color PrimaryColor => Managers.SessionManager.Instance.Account.PrimaryColor;

        public Xamarin.Forms.Color SecoundaryColor => Managers.SessionManager.Instance.Account.SecoundaryColor;

        public System.Guid? AccountDefaultOverlayId => Managers.SessionManager.Instance.Account.DefaultOverlayId;

        public string AccountSerializedOverlayContext => Managers.SessionManager.Instance.Account.SerializedOverlayContext;
    }
}
