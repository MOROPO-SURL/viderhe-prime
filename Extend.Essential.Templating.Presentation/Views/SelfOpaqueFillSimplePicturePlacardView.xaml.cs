namespace SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Presentation.Views
{
    public partial class SelfOpaqueFillSimplePicturePlacardView : Common.Templating.Presentation.Views.BaseTemplatedPlacardPresentationView
    {
        public SelfOpaqueFillSimplePicturePlacardView(Models.View.SelfOpaqueFillSimplePicturePlacardViewModel model, Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider)
        {
            InitializeComponent();

            PlacardLayout.BackgroundColor = accountInformationProvider.PrimaryColor;

            BindingContext = model;
        }

        public override Xamarin.Forms.Layout<Xamarin.Forms.View> WrapLayout => Wrapper;

        public override Xamarin.Forms.View PlacardView => PlacardLayout;

        public override void ConfigureLayout(Xamarin.Forms.View view)
        {
            Xamarin.Forms.Grid.SetColumn(view, 0);
            Xamarin.Forms.Grid.SetRow(view, 0);
        }

        public override void PerformPlacardStarup()
        {
            base.PerformPlacardStarup();

            if ((BindingContext as Models.View.SimplePicturePlacardViewModel).ZoomingEnabled)
            {
                var animation = new XFToolKit.Presentation.Transform.Animations.ScaleToAnimation
                {
                    Target = this,
                    Scale = Scale == 0 ? 1.2 : 1,
                    Duration = (BindingContext as Models.View.SimplePicturePlacardViewModel).ScreenPresentationTime.TotalMilliseconds.ToString()
                };

                animation.Begin();
            }
        }
    }
}