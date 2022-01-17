namespace SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Presentation.Models.View
{
    public class SelfOpaqueFillSimplePicturePlacardViewModel : SimplePicturePlacardViewModel
    {
        public Xamarin.Forms.ImageSource ThumnailSource { get; set; }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            ThumnailSource = null;
        }
    }
}
