namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Dashboard.Models.Page
{
    public class PlacardsPickPageModel : XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private bool hasMoreImages;

        public PlacardsPickPageModel()
        {
            SelectablePlacards = new System.Collections.ObjectModel.ObservableCollection<View.SelectableModelViewModel<View.PlacardOverviewViewModel>>();
        }

        public System.Collections.ObjectModel.ObservableCollection<View.SelectableModelViewModel<View.PlacardOverviewViewModel>> SelectablePlacards { get; set; }

        public bool HasMoreImages { get => hasMoreImages; set => SetProperty(ref hasMoreImages, value); }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            SelectablePlacards.Clear();

            SelectablePlacards = null;
        }
    }
}
