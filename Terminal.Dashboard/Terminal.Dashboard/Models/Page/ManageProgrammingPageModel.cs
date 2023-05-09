using SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Models.View;

namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Models.Page
{
    public class ManageProgrammingPageModel : XFToolKit.Mvvm.Models.Page.BasePageModel
    {
        private readonly System.Action<ScreenplayViewModel> selectedScreenplayCallbackAction;

        private ScreenplayViewModel[] screenplays;
        private Support.Presentation.Models.View.FrameViewModel[] frames;
        private bool hasMorePlacards;
        private ScreenplayViewModel selectedScreenPlay;
        private bool useExpandedLayout;
        private System.Collections.ObjectModel.ObservableCollection<Support.Presentation.Dashboard.Models.View.PlacardOverviewViewModel> placardOverviews;

        public ManageProgrammingPageModel(System.Action<ScreenplayViewModel> selectedScreenplayCallbackAction)
        {
            this.selectedScreenplayCallbackAction = selectedScreenplayCallbackAction;

            Screenplays = new ScreenplayViewModel[] { };
            Frames = new Support.Presentation.Models.View.FrameViewModel[] { };
            PlacardOverviews = new System.Collections.ObjectModel.ObservableCollection<Support.Presentation.Dashboard.Models.View.PlacardOverviewViewModel>();
        }

        public ScreenplayViewModel[] Screenplays
        {
            get => screenplays;
            set => SetProperty(ref screenplays, value);
        }

        public Support.Presentation.Models.View.FrameViewModel[] Frames
        {
            get => frames;
            set => SetProperty(ref frames, value);
        }

        public System.Collections.ObjectModel.ObservableCollection<Support.Presentation.Dashboard.Models.View.PlacardOverviewViewModel> PlacardOverviews { get => placardOverviews; set => SetProperty(ref placardOverviews, value); }

        public bool HasMorePlacards { get => hasMorePlacards; set => SetProperty(ref hasMorePlacards, value); }

        public ScreenplayViewModel SelectedScreenplay
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

            foreach (var item in PlacardOverviews)
            {
                item.Dispose();
            }

            PlacardOverviews.Clear();

            PlacardOverviews = null;
        }
    }
}
