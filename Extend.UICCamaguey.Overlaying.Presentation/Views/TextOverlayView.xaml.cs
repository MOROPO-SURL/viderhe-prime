namespace SmartSolucionesCuba.Viderhe.Extend.UICCamaguey.Overlaying.Presentation.Views
{
    public partial class TextOverlayView : Common.Overlaying.Presentation.Views.BaseOverlayView
    {
        internal static readonly double MotionTranslation = 108;

        public TextOverlayView(Models.View.TextOverlayViewModel model)
        {
            InitializeComponent();

            BindingContext = model;
        }

        public override void ShowIn()
        {
            var animation = new XFToolKit.Presentation.Transform.Animations.TranslateToAnimation
            {
                Target = this,
                EasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicOut,
                TranslateY = 0
            };

            animation.Begin();
        }

        public override void ShowOut()
        {
            var animation = new XFToolKit.Presentation.Transform.Animations.TranslateToAnimation
            {
                Target = this,
                EasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicIn,
                TranslateY = MotionTranslation
            };

            animation.Begin();
        }
    }
}