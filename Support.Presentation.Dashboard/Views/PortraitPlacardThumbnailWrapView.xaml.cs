namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Dashboard.Views
{
    public partial class PortraitPlacardThumbnailWrapView : Xamarin.Forms.ContentView
    {
        public PortraitPlacardThumbnailWrapView(Presentation.Models.View.PlacardViewModel model)
        {
            InitializeComponent();

            BindingContext = model;
        }
    }
}