namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Models.Page
{
    public class ManageProgrammingPageModel : XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private readonly System.Action<View.ScreenplayViewModel> selectedScreenplayCallbackAction;

        private View.ScreenplayViewModel[] screenplays;
        private Support.Presentation.Models.View.FrameViewModel[] frames;
        private bool hasMorePlacards;
        private View.ScreenplayViewModel selectedScreenPlay;
        private bool useExpandedLayout;

        public ManageProgrammingPageModel(System.Action<View.ScreenplayViewModel> selectedScreenplayCallbackAction)
        {
            this.selectedScreenplayCallbackAction = selectedScreenplayCallbackAction;

            Screenplays = new View.ScreenplayViewModel[] { };
            Frames = new Support.Presentation.Models.View.FrameViewModel[] { };
            Placards = new System.Collections.ObjectModel.ObservableCollection<Support.Presentation.Models.View.PlacardViewModel>();
        }

        public View.ScreenplayViewModel[] Screenplays
        {
            get => screenplays;
            set => SetProperty(ref screenplays, value);
        }

        public Support.Presentation.Models.View.FrameViewModel[] Frames
        {
            get => frames;
            set => SetProperty(ref frames, value);
        }

        public System.Collections.ObjectModel.ObservableCollection<Support.Presentation.Models.View.PlacardViewModel> Placards { get; set; }

        public bool HasMorePlacards { get => hasMorePlacards; set => SetProperty(ref hasMorePlacards, value); }

        public View.ScreenplayViewModel SelectedScreenplay
        {
            get => selectedScreenPlay;
            set => SetProperty(ref selectedScreenPlay, value, () =>
            {
                selectedScreenplayCallbackAction?.Invoke(value);
            });
        }

        public bool UseExpandedLayout { get => useExpandedLayout; set => SetProperty(ref useExpandedLayout, value); }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            SelectedScreenplay = null;

            Screenplays = null;

            Frames = null;

            foreach (var item in Placards)
            {
                item.Dispose();
            }

            Placards.Clear();

            Placards = null;
        }
    }
}
