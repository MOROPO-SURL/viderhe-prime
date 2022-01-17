using SmartSolucionesCuba.XFToolKit.Presentation.Transform.Animations;
using SmartSolucionesCuba.XFToolKit.Presentation.Transform.Animations.Contract;

namespace SmartSolucionesCuba.Viderhe.Extend.Essential.Transitioning.Presentation.Transitions
{
    public class ArriveTransition : Common.Transitioning.Presentation.Transitions.Contract.AbstractTransition
    {
        private Models.Context.ArriveTransitionContextModel context;

        private AbstractAnimation factShowInAnimation;

        public ArriveTransition(Common.Presentation.Views.BasePlacardPresentationView incomingPlacardPresentationView, Common.Presentation.Views.BasePlacardPresentationView outgoingPlacardPresentationView, Models.Context.ArriveTransitionContextModel context, System.Action<Common.Presentation.Views.BasePlacardPresentationView> motionCompletedActionCallback) : base(incomingPlacardPresentationView, outgoingPlacardPresentationView, motionCompletedActionCallback)
        {
            this.context = context;
        }

        protected override AbstractAnimation FactShowInAnimation => factShowInAnimation;

        public override void GetReady(Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider, Xamarin.Forms.Layout<Xamarin.Forms.View> wrapper)
        {
            switch (context.Direction)
            {
                case Common.Transitioning.Presentation.Enumerations.StandardTransitionDirection.LeftToRight:
                    incomingPlacardPresentationView.TranslationX = wrapper.Width * -1;

                    break;
                case Common.Transitioning.Presentation.Enumerations.StandardTransitionDirection.RightToLeft:
                    incomingPlacardPresentationView.TranslationX = wrapper.Width;

                    break;
                case Common.Transitioning.Presentation.Enumerations.StandardTransitionDirection.TopToBottom:
                    incomingPlacardPresentationView.TranslationY = wrapper.Height * -1;

                    break;
                case Common.Transitioning.Presentation.Enumerations.StandardTransitionDirection.BottomToTop:
                default:
                    incomingPlacardPresentationView.TranslationY = wrapper.Height;

                    break;
            }
        }

        protected override Storyboard CreateShowInMotionStoryboard()
        {
            var storyboard = new Storyboard();

            factShowInAnimation = new TranslateToAnimation
            {
                Target = incomingPlacardPresentationView,
                EasingType = context.MotionEasingType,
                TranslateX = 0,
                TranslateY = 0
            };

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
