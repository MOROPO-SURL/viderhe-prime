namespace SmartSolucionesCuba.Viderhe.Extend.UICCamaguey.Overlaying.Presentation.Models.View
{
    public class TextOverlayViewModel : AbstractOverlayViewModel
    {
        private string text;

        public string Text { get => text; set => SetProperty(ref text, value); }
    }
}