using SmartSolucionesCuba.XFToolKit.Mvvm.Extensions;

namespace SmartSolucionesCuba.Viderhe.Extend.UICCamaguey.Overlaying.Dashboard.Presentation.Pages
{
    public partial class RSSOverlayConfigurationPage : XFToolKit.Mvvm.Pages.BindingContextedContentPage<Models.Page.RSSOverlayConfigurationPageModel>
    {
        private System.Action<string> successfulCompleteCallbackAction;

        public RSSOverlayConfigurationPage(string serializedContext, System.Action<string> successfulCompleteCallbackAction)
            : base(Factories.RSSOverlayConfigurationPageModelFactory.Build(serializedContext))
        {
            InitializeComponent();

            this.successfulCompleteCallbackAction = successfulCompleteCallbackAction;
        }

        private void ApplyToolbarItem_Clicked(object sender, System.EventArgs eventArgs)
        {
            this.NavigationPopPageAsync();

            successfulCompleteCallbackAction.Invoke(Newtonsoft.Json.JsonConvert.SerializeObject(new Overlaying.Presentation.Models.Context.RSSOverlayContextModel
            {
                Head = BindingContextPageModel.Head,
                Tail = BindingContextPageModel.Tail,
                FeedUrl = new System.Uri(BindingContextPageModel.FeedUrl)
            }));
        }

        private void CancelToolbarItem_Clicked(object sender, System.EventArgs eventArgs)
        {
            this.NavigationPopPageAsync();
        }

        protected override void DisposeUnmanagementResources()
        {
            base.DisposeUnmanagementResources();

            successfulCompleteCallbackAction = null;
        }
    }
}