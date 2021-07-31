namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Dashboard.Models.Page
{
    public class ImagePickPageModel : XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private bool hasMoreImages;

        public ImagePickPageModel()
        {
            Images = new System.Collections.ObjectModel.ObservableCollection<Presentation.Models.View.ImageViewModel>();
        }

        public System.Collections.ObjectModel.ObservableCollection<Presentation.Models.View.ImageViewModel> Images { get; set; }

        public bool HasMoreImages { get => hasMoreImages; set => SetProperty(ref hasMoreImages, value); }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            Images.Clear();

            Images = null;
        }
    }
}
