namespace SmartSolucionesCuba.Viderhe.Terminal.Player.Models.View
{
    public class AccountViewModel
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public Xamarin.Forms.ImageSource LogoImageSource { get; set; }
        public Xamarin.Forms.ImageSource HorizontalSplashImageSource { get; set; }
        public Xamarin.Forms.ImageSource VerticalSplashImageSource { get; set; }
        public Xamarin.Forms.Color PrimaryColor { get; set; }
        public Xamarin.Forms.Color SecoundaryColor { get; set; }
        public System.Guid? DefaultOverlayId { get; set; }
        public string SerializedOverlayContext { get; set; }
        public System.Collections.Generic.List<Cloud.Api.Common.Models.ExtensionDisplayModel> Extensions { get; set; }
    }
}
