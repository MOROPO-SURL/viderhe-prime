using SmartSolucionesCuba.XFToolKit.Presentation.Transform.Animations;
using SmartSolucionesCuba.XFToolKit.Presentation.Transform.Animations.Contract;

namespace SmartSolucionesCuba.Viderhe.Extend.Essential.Transitioning.Presentation.Transitions
{
    public class FadeInTransition : Common.Transitioning.Presentation.Transitions.Contract.AbstractTransition
    {
        private const double MotionSize = 80;

        private Models.Context.FadeInTransitionContextModel context;

        private AbstractAnimation factShowInAnimation;

        public FadeInTransition(Common.Presentation.Views.BasePlacardPresentationView incomingPlacardPresentationView, Common.Presentation.Views.BasePlacardPresentationView outgoingPlacardPresentationView, Models.Context.FadeInTransitionContextModel context, System.Action<Common.Presentation.Views.BasePlacardPresentationView> motionCompletedActionCallback) : base(incomingPlacardPresentationView, outgoingPlacardPresentationView, motionCompletedActionCallback)
        {
            this.context = context;
        }

        protected override AbstractAnimation FactShowInAnimation => factShowInAnimation;

        public override void GetReady(Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider, Xamarin.Forms.Layout<Xamarin.Forms.View> wrapper)
        {
            incomingPlacardPresentationView.Opacity = 0;

            switch (context.MotionDirection)
            {
                case Common.Transitioning.Presentation.Enumerations.VerticalTransitionOrientation.FromTop:
                    incomingPlacardPresentationView.TranslationY = MotionSize * -1;

                    break;
                case Common.Transitioning.Presentation.Enumerations.VerticalTransitionOrientation.FromBottom:
                    incomingPlacardPresentationView.TranslationY = MotionSize;

                    break;
            }
        }

        protected override Storyboard CreateShowInMotionStoryboard()
        {
            var storyboard = new Storyboard();

            if (context.MotionDirection == Common.Transitioning.Presentation.Enumerations.VerticalTransitionOrientation.None)
            {
                factShowInAnimation = new FadeToAnimation
                {
                    Target = incomingPlacardPresentationView,
                    Opacity = 1
                };
            }
            else
            {
                factShowInAnimation = new FadeInAnimation
                {
                    Target = incomingPlacardPresentationView,
                    Direction = context.MotionDirection == Common.Transitioning.Presentation.Enumerations.VerticalTransitionOrientation.FromBottom ? XFToolKit.Presentation.Transform.Enumerations.FadeDirection.Up : XFToolKit.Presentation.Transform.Enumerations.FadeDirection.Down
                };
            }

            storyboard.Animations.Add(factShowInAnimation);

            return storyboard;
        }

        protected override void GarbageCollect()
        {
            factShowInAnimation = null;

            context = null;
        }
    }
}
