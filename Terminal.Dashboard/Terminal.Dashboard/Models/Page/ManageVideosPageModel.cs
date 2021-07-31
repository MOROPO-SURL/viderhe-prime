namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Models.Page
{
    public class ManageVideosPageModel : XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private bool hasMoreItems;

        public ManageVideosPageModel()
        {
            Items = new System.Collections.ObjectModel.ObservableCollection<Support.Presentation.Models.View.VideoViewModel>();
        }

        public System.Collections.ObjectModel.ObservableCollection<Support.Presentation.Models.View.VideoViewModel> Items { get; set; }

        public bool HasMoreItems { get => hasMoreItems; set => SetProperty(ref hasMoreItems, value); }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            Items.Clear();

            Items = null;
        }
    }
}
