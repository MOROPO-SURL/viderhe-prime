namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Dashboard.Models.Page
{
    public class PlacardPickPageModel : XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private System.Collections.ObjectModel.ObservableCollection<View.PlacardOverviewViewModel> items;
        private bool hasMoreItems;

        public System.Collections.ObjectModel.ObservableCollection<View.PlacardOverviewViewModel> Items { get => items; set => SetProperty(ref items, value); }

        public bool HasMoreItems { get => hasMoreItems; set => SetProperty(ref hasMoreItems, value); }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            foreach (var item in Items)
            {
                item.Dispose();
            }

            Items.Clear();

            Items = null;
        }
    }
}