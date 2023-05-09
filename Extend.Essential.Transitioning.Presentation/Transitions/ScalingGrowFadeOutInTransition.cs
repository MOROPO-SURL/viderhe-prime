using SmartSolucionesCuba.XFToolKit.Presentation.Transform.Animations;
using SmartSolucionesCuba.XFToolKit.Presentation.Transform.Animations.Contract;

namespace SmartSolucionesCuba.Viderhe.Extend.Essential.Transitioning.Presentation.Transitions
{
    public class ScalingGrowFadeOutInTransition : Common.Transitioning.Presentation.Transitions.Contract.AbstractTransition
    {
        private AbstractAnimation factShowInAnimation;

        public ScalingGrowFadeOutInTransition(Common.Presentation.Views.BasePlacardPresentationView incomingPlacardPresentationView, Common.Presentation.Views.BasePlacardPresentationView outgoingPlacardPresentationView, System.Action<Common.Presentation.Views.BasePlacardPresentationView> motionCompletedActionCallback) : base(incomingPlacardPresentationView, outgoingPlacardPresentationView, motionCompletedActionCallback)
        {
        }

        protected override AbstractAnimation FactShowInAnimation => factShowInAnimation;

        public override void GetReady(Xamarin.Forms.Layout<Xamarin.Forms.View> wrapper)
        {
            incomingPlacardPresentationView.Opacity = 0;

            wrapper.RaiseChild(outgoingPlacardPresentationView);
        }

        protected override Storyboard CreateShowInMotionStoryboard()
        {
            var storyboard = new Storyboard
            {
                IsParallel = true
            };

            if (outgoingPlacardPresentationView != null)
            {
                storyboard.Animations.Add(new ScaleToAnimation
                {
                    Target = outgoingPlacardPresentationView,
                    EasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicInOut,
                    Scale = 1.2,
                    Duration = "3000"
                });
            }

            storyboard.Animations.Add(factShowInAnimation = new ScaleToAnimation
            {
                Target = incomingPlacardPresentationView,
                EasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicInOut,
                Scale = 1.2,
                Duration = "3000"
            });
            storyboard.Animations.Add(new FadeToAnimation
            {
                Target = incomingPlacardPresentationView,
                Opacity = 1,
                Delay = "1000",
                Duration = "2000"
            });

            return storyboard;
        }

        protected override void GarbageCollect()
        {
            factShowInAnimation = null;
        }
    }
}
