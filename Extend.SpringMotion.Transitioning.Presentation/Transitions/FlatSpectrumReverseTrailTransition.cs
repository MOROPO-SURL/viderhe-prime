using SmartSolucionesCuba.Viderhe.Extend.Common.Transitioning.Presentation.Enumerations;
using SmartSolucionesCuba.XFToolKit.Presentation.Transform.Animations;

namespace SmartSolucionesCuba.Viderhe.Extend.SpringMotion.Transitioning.Presentation.Transitions
{
    public class FlatSpectrumReverseTrailTransition : Common.Transitioning.Presentation.Transitions.AbstractTransition
    {
        private Xamarin.Forms.BoxView flatSpectrumHandFanPrimaryBoxView;
        private Xamarin.Forms.BoxView flatSpectrumHandFanSecoundaryBoxView;
        private Xamarin.Forms.BoxView flatSpectrumHandFanNeutralBoxView;
        private Xamarin.Forms.BoxView flatSpectrumHandFanPrimaryOpaqueBoxView;
        private Xamarin.Forms.BoxView flatSpectrumHandFanSecoundaryOpaqueBoxView;
        private Xamarin.Forms.BoxView flatSpectrumHandFanNeutralOpaqueBoxView;

        private Models.Context.FlatSpectrumInverseGothTransitionContextModel context;

        private AbstractAnimation factShowInAnimation;

        public FlatSpectrumReverseTrailTransition(Common.Presentation.Views.BasePlacardPresentationView incomingPlacardPresentationView, Common.Presentation.Views.BasePlacardPresentationView outgoingPlacardPresentationView, Models.Context.FlatSpectrumInverseGothTransitionContextModel context, System.Action<Common.Presentation.Views.BasePlacardPresentationView> motionCompletedActionCallback) : base(incomingPlacardPresentationView, outgoingPlacardPresentationView, motionCompletedActionCallback)
        {
            if (context.VerticalTransitionOrientation == VerticalTransitionOrientation.None && context.HorizontalTransitionOrientation == HorizontalTransitionOrientation.None) throw new System.InvalidOperationException();

            this.context = context;
        }

        protected override AbstractAnimation FactShowInAnimation => factShowInAnimation;

        public override void GetReady(Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider, Xamarin.Forms.Layout<Xamarin.Forms.View> wrapper)
        {
            flatSpectrumHandFanPrimaryBoxView = new Xamarin.Forms.BoxView
            {
                Color = accountInformationProvider.PrimaryColor
            };

            ConfigureFlatHandFanItemPositionByContext(flatSpectrumHandFanPrimaryBoxView, wrapper);

            incomingPlacardPresentationView.ConfigureLayout(flatSpectrumHandFanPrimaryBoxView);

            flatSpectrumHandFanSecoundaryBoxView = new Xamarin.Forms.BoxView
            {
                Color = accountInformationProvider.SecoundaryColor
            };

            ConfigureFlatHandFanItemPositionByContext(flatSpectrumHandFanSecoundaryBoxView, wrapper);

            incomingPlacardPresentationView.ConfigureLayout(flatSpectrumHandFanSecoundaryBoxView);

            if (context.UseOpaqueFlatSpectrumItems)
            {
                flatSpectrumHandFanPrimaryOpaqueBoxView = new Xamarin.Forms.BoxView
                {
                    Color = accountInformationProvider.PrimaryColor.MultiplyAlpha(0.5)
                };

                ConfigureFlatHandFanItemPositionByContext(flatSpectrumHandFanPrimaryOpaqueBoxView, wrapper);

                incomingPlacardPresentationView.ConfigureLayout(flatSpectrumHandFanPrimaryOpaqueBoxView);

                flatSpectrumHandFanSecoundaryOpaqueBoxView = new Xamarin.Forms.BoxView
                {
                    Color = accountInformationProvider.SecoundaryColor.MultiplyAlpha(0.5)
                };

                ConfigureFlatHandFanItemPositionByContext(flatSpectrumHandFanSecoundaryOpaqueBoxView, wrapper);

                incomingPlacardPresentationView.ConfigureLayout(flatSpectrumHandFanSecoundaryOpaqueBoxView);
            }

            if (context.UseNeutralFlatSpectrumItems)
            {
                flatSpectrumHandFanNeutralBoxView = new Xamarin.Forms.BoxView
                {
                    Color = Xamarin.Forms.Color.White
                };

                ConfigureFlatHandFanItemPositionByContext(flatSpectrumHandFanNeutralBoxView, wrapper);

                incomingPlacardPresentationView.ConfigureLayout(flatSpectrumHandFanNeutralBoxView);

                if (context.UseOpaqueFlatSpectrumItems)
                {
                    flatSpectrumHandFanNeutralOpaqueBoxView = new Xamarin.Forms.BoxView
                    {
                        Color = Xamarin.Forms.Color.White.MultiplyAlpha(0.5)
                    };

                    ConfigureFlatHandFanItemPositionByContext(flatSpectrumHandFanNeutralOpaqueBoxView, wrapper);

                    incomingPlacardPresentationView.ConfigureLayout(flatSpectrumHandFanNeutralOpaqueBoxView);
                }
            }

            ConfigureFlatHandFanItemPositionByContext(incomingPlacardPresentationView.PlacardView, wrapper);

            if (context.UseOpaqueFlatSpectrumItems)
            {
                if (context.UseNeutralFlatSpectrumItems)
                {
                    incomingPlacardPresentationView.WrapLayout.Children.Insert(0, flatSpectrumHandFanNeutralOpaqueBoxView);
                }

                incomingPlacardPresentationView.WrapLayout.Children.Insert(0, flatSpectrumHandFanSecoundaryOpaqueBoxView);
                incomingPlacardPresentationView.WrapLayout.Children.Insert(0, flatSpectrumHandFanPrimaryOpaqueBoxView);
            }

            if (context.UseNeutralFlatSpectrumItems)
            {
                incomingPlacardPresentationView.WrapLayout.Children.Add(flatSpectrumHandFanNeutralBoxView);
            }

            incomingPlacardPresentationView.WrapLayout.Children.Add(flatSpectrumHandFanSecoundaryBoxView);
            incomingPlacardPresentationView.WrapLayout.Children.Add(flatSpectrumHandFanPrimaryBoxView);

            incomingPlacardPresentationView.WrapLayout.RaiseChild(incomingPlacardPresentationView.PlacardView);
        }

        protected override void GarbageCollect()
        {
            if (context.UseOpaqueFlatSpectrumItems)
            {
                if (context.UseNeutralFlatSpectrumItems)
                {
                    incomingPlacardPresentationView.WrapLayout.Children.Remove(flatSpectrumHandFanNeutralOpaqueBoxView);
                }

                incomingPlacardPresentationView.WrapLayout.Children.Remove(flatSpectrumHandFanSecoundaryOpaqueBoxView);
                incomingPlacardPresentationView.WrapLayout.Children.Remove(flatSpectrumHandFanPrimaryOpaqueBoxView);
            }

            if (context.UseNeutralFlatSpectrumItems)
            {
                incomingPlacardPresentationView.WrapLayout.Children.Remove(flatSpectrumHandFanNeutralBoxView);
            }

            incomingPlacardPresentationView.WrapLayout.Children.Remove(flatSpectrumHandFanSecoundaryBoxView);
            incomingPlacardPresentationView.WrapLayout.Children.Remove(flatSpectrumHandFanPrimaryBoxView);

            flatSpectrumHandFanNeutralOpaqueBoxView = null;
            flatSpectrumHandFanNeutralBoxView = null;
            flatSpectrumHandFanSecoundaryOpaqueBoxView = null;
            flatSpectrumHandFanPrimaryOpaqueBoxView = null;
            flatSpectrumHandFanSecoundaryBoxView = null;
            flatSpectrumHandFanPrimaryBoxView = null;

            factShowInAnimation = null;

            context = null;
        }

        protected override Storyboard CreateShowInMotionStoryboard()
        {
            var storyboard = new Storyboard();

            int flatSpectrumItemCount = 0;

            if (flatSpectrumHandFanNeutralOpaqueBoxView != null) storyboard.Animations.Add(CreateFlatHandFanItemAnimation(flatSpectrumHandFanNeutralOpaqueBoxView, 200 * flatSpectrumItemCount++));

            if (flatSpectrumHandFanSecoundaryOpaqueBoxView != null) storyboard.Animations.Add(CreateFlatHandFanItemAnimation(flatSpectrumHandFanSecoundaryOpaqueBoxView, 200 * flatSpectrumItemCount++));

            if (flatSpectrumHandFanPrimaryOpaqueBoxView != null) storyboard.Animations.Add(CreateFlatHandFanItemAnimation(flatSpectrumHandFanPrimaryOpaqueBoxView, 200 * flatSpectrumItemCount++));

            if (flatSpectrumHandFanNeutralBoxView != null) storyboard.Animations.Add(CreateFlatHandFanItemAnimation(flatSpectrumHandFanNeutralBoxView, 200 * flatSpectrumItemCount++));

            storyboard.Animations.Add(CreateFlatHandFanItemAnimation(flatSpectrumHandFanSecoundaryBoxView, 200 * flatSpectrumItemCount++));

            storyboard.Animations.Add(CreateFlatHandFanItemAnimation(flatSpectrumHandFanPrimaryBoxView, 200 * flatSpectrumItemCount++));

            factShowInAnimation = CreateFlatHandFanItemAnimation(incomingPlacardPresentationView.PlacardView, 200 * flatSpectrumItemCount);

            storyboard.Animations.Add(factShowInAnimation);

            if (factShowInAnimation is Storyboard)
            {
                factShowInAnimation = (factShowInAnimation as Storyboard).Animations[1];
            }

            return storyboard;
        }

        private void ConfigureFlatHandFanItemPositionByContext(Xamarin.Forms.VisualElement target, Xamarin.Forms.Layout<Xamarin.Forms.View> wrapper)
        {
            target.WidthRequest = wrapper.Width;
            target.HeightRequest = wrapper.Height;

            switch (context.HorizontalTransitionOrientation)
            {
                case HorizontalTransitionOrientation.FromLeft:
                    target.TranslationX = wrapper.Width * -1;

                    break;
                case HorizontalTransitionOrientation.FromRight:
                    target.TranslationX = wrapper.Width;

                    break;
                default:
                    target.TranslationX = 0;

                    break;
            }

            switch (context.VerticalTransitionOrientation)
            {
                case VerticalTransitionOrientation.FromTop:
                    target.TranslationY = wrapper.Height * -1;

                    break;
                case VerticalTransitionOrientation.FromBottom:
                    target.TranslationY = wrapper.Height;

                    break;
                default:
                    target.TranslationY = 0;

                    break;
            }
        }

        private AbstractAnimation CreateFlatHandFanItemAnimation(Xamarin.Forms.VisualElement target, int delayMilisecounds = 0)
        {
            if (context.HorizontalTransitionOrientation != HorizontalTransitionOrientation.None && context.VerticalTransitionOrientation != VerticalTransitionOrientation.None)
            {
                if (context.HorizontalTransitionEasingType == context.VerticalTransitionEasingType)
                {
                    return new TranslateToAnimation
                    {
                        Target = target,
                        EasingType = context.HorizontalTransitionEasingType,
                        TranslateY = 0,
                        TranslateX = 0,
                        Duration = "800",
                        Delay = delayMilisecounds.ToString()
                    };
                }

                var storyboard = new Storyboard
                {
                    Target = target,
                    Delay = delayMilisecounds.ToString(),
                    IsParallel = true
                };

                storyboard.Animations.Add(new TranslateToXAnimation { TranslateX = 0, EasingType = context.HorizontalTransitionEasingType });
                storyboard.Animations.Add(new TranslateToYAnimation { TranslateY = 0, EasingType = context.VerticalTransitionEasingType });

                return storyboard;
            }

            if (context.HorizontalTransitionOrientation != HorizontalTransitionOrientation.None)
            {
                return new TranslateToAnimation
                {
                    Target = target,
                    EasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicOut,
                    TranslateX = 0,
                    Duration = "800",
                    Delay = delayMilisecounds.ToString()
                };
            }

            return new TranslateToAnimation
            {
                Target = target,
                EasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicOut,
                TranslateY = 0,
                Duration = "800",
                Delay = delayMilisecounds.ToString()
            };
        }
    }
}
