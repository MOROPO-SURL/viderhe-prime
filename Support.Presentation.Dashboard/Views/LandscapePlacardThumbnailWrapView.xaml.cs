namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Dashboard.Views
{
    public partial class LandscapePlacardThumbnailWrapView : Xamarin.Forms.ContentView
    {
        public LandscapePlacardThumbnailWrapView(Presentation.Models.View.PlacardViewModel model)
        {
            InitializeComponent();

            BindingContext = model;
        }
    }
}