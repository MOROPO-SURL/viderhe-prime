namespace SmartSolucionesCuba.Viderhe.Support.Presentation.Dashboard.Models.View
{
    public class PlacardOverviewViewModel : XFToolKit.Mvvm.Models.BaseBindableModel
    {
        private Presentation.Models.View.PlacardViewModel reference;
        private Presentation.Models.View.PlacardViewModel placard;

        public PlacardOverviewViewModel(Presentation.Models.View.PlacardViewModel reference, bool load = true)
        {
            this.reference = reference;

            if (!load) return;

            LoadPlacardModel();
        }

        public Presentation.Models.View.PlacardViewModel Placard { get => placard; set => SetProperty(ref placard, value); }

        public void LoadPlacardModel()
        {
            Placard = reference;
        }

        public void ReleasePlacardModel()
        {
            Placard = null;
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            if (Placard != null)
            {
                Placard.Dispose();

                ReleasePlacardModel();
            }

            reference.Dispose();

            reference = null;
        }
    }
}
