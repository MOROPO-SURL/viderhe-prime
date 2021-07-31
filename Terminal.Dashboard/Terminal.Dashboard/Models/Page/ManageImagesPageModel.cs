namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Models.Page
{
    public class ManageImagesPageModel : XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private bool hasMoreItems;

        public ManageImagesPageModel()
        {
            Items = new System.Collections.ObjectModel.ObservableCollection<Support.Presentation.Models.View.ImageViewModel>();
        }

        public System.Collections.ObjectModel.ObservableCollection<Support.Presentation.Models.View.ImageViewModel> Items { get; set; }

        public bool HasMoreItems { get => hasMoreItems; set => SetProperty(ref hasMoreItems, value); }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            Items.Clear();

            Items = null;
        }
    }
}
