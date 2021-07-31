namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Dashboard.Models.Page
{
    public class ImagesPickPageModel : XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private bool hasMoreImages;

        public ImagesPickPageModel()
        {
            SelectableImages = new System.Collections.ObjectModel.ObservableCollection<View.SelectableModelViewModel<Presentation.Models.View.ImageViewModel>>();
        }

        public System.Collections.ObjectModel.ObservableCollection<View.SelectableModelViewModel<Presentation.Models.View.ImageViewModel>> SelectableImages { get; set; }

        public bool HasMoreImages { get => hasMoreImages; set => SetProperty(ref hasMoreImages, value); }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            SelectableImages.Clear();

            SelectableImages = null;
        }
    }
}
