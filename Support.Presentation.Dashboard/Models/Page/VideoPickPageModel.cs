namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Dashboard.Models.Page
{
    public class VideoPickPageModel : XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private bool hasMoreVideos;

        public VideoPickPageModel()
        {
            Videos = new System.Collections.ObjectModel.ObservableCollection<Presentation.Models.View.VideoViewModel>();
        }

        public System.Collections.ObjectModel.ObservableCollection<Presentation.Models.View.VideoViewModel> Videos { get; set; }

        public bool HasMoreVideos { get => hasMoreVideos; set => SetProperty(ref hasMoreVideos, value); }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            Videos.Clear();

            Videos = null;
        }
    }
}
