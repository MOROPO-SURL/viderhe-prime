namespace SmartSolucionesCuba.Viderhe.Extend.UICCamaguey.Overlaying.Presentation.Models.View
{
    public abstract class AbstractOverlayViewModel : XFToolKit.Mvvm.Models.BaseBindableModel
    {
        private Xamarin.Forms.Color primaryColor;

        public Xamarin.Forms.Color PrimaryColor { get => primaryColor; set => SetProperty(ref primaryColor, value); }
    }
}
