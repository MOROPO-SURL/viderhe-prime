using SmartSolucionesCuba.XFToolKit.Presentation.Transform.Animations;

namespace SmartSolucionesCuba.Viderhe.Extend.SpringMotion.Transitioning.Presentation.Transitions
{
    public class FlatSpectrumHandFanTransition : Common.Transitioning.Presentation.Transitions.AbstractTransition
    {
        private Xamarin.Forms.BoxView flatSpectrumHandFanPrimaryBoxView;
        private Xamarin.Forms.BoxView flatSpectrumHandFanSecoundaryBoxView;
        private Xamarin.Forms.BoxView flatSpectrumHandFanNeutralBoxView;
        private Xamarin.Forms.BoxView flatSpectrumHandFanPrimaryOpaqueBoxView;
        private Xamarin.Forms.BoxView flatSpectrumHandFanSecoundaryOpaqueBoxView;
        private Xamarin.Forms.BoxView flatSpectrumHandFanNeutralOpaqueBoxView;

        private Models.Context.BaseFlatSpectrumTransitionContextModel context;

        private AbstractAnimation factShowInAnimation;

        public FlatSpectrumHandFanTransition(Common.Presentation.Views.BasePlacardPresentationView incomingPlacardPresentationView, Common.Presentation.Views.BasePlacardPresentationView outgoingPlacardPresentationView, Models.Context.BaseFlatSpectrumTransitionContextModel context, System.Action<Common.Presentation.Views.BasePlacardPresentationView> motionCompletedActionCallback) : base(incomingPlacardPresentationView, outgoingPlacardPresentationView, motionCompletedActionCallback)
        {
            this.context = context;
        }

        protected override AbstractAnimation FactShowInAnimation => factShowInAnimation;

        public override void GetReady(Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider, Xamarin.Forms.Layout<Xamarin.Forms.View> wrapper)
        {
            flatSpectrumHandFanPrimaryBoxView = new Xamarin.Forms.BoxView
            {
                Color = accountInformationProvider.PrimaryColor,
                WidthRequest = wrapper.Width,
                HeightRequest = wrapper.Height,
                TranslationX = wrapper.Width * -1,
                TranslationY = wrapper.Height * -1,
                Rotation = 45 * -1
            };

            incomingPlacardPresentationView.ConfigureLayout(flatSpectrumHandFanPrimaryBoxView);

            flatSpectrumHandFanSecoundaryBoxView = new Xamarin.Forms.BoxView
            {
                Color = accountInformationProvider.SecoundaryColor,
                WidthRequest = wrapper.Width,
                HeightRequest = wrapper.Height,
                TranslationX = wrapper.Width * -1,
                TranslationY = wrapper.Height * -1,
                Rotation = 45 * -1
            };

            incomingPlacardPresentationView.ConfigureLayout(flatSpectrumHandFanSecoundaryBoxView);

            if (context.UseOpaqueFlatSpectrumItems)
            {
                flatSpectrumHandFanPrimaryOpaqueBoxView = new Xamarin.Forms.BoxView
                {
                    Color = accountInformationProvider.PrimaryColor.MultiplyAlpha(0.5),
                    WidthRequest = wrapper.Width,
                    HeightRequest = wrapper.Height,
                    TranslationX = wrapper.Width * -1,
                    TranslationY = wrapper.Height * -1,
                    Rotation = 45 * -1
                };

                incomingPlacardPresentationView.ConfigureLayout(flatSpectrumHandFanPrimaryOpaqueBoxView);

                flatSpectrumHandFanSecoundaryOpaqueBoxView = new Xamarin.Forms.BoxView
                {
                    Color = accountInformationProvider.SecoundaryColor.MultiplyAlpha(0.5),
                    WidthRequest = wrapper.Width,
                    HeightRequest = wrapper.Height,
                    TranslationX = wrapper.Width * -1,
                    TranslationY = wrapper.Height * -1,
                    Rotation = 45 * -1
                };

                incomingPlacardPresentationView.ConfigureLayout(flatSpectrumHandFanSecoundaryOpaqueBoxView);
            }

            if (context.UseNeutralFlatSpectrumItems)
            {
                flatSpectrumHandFanNeutralBoxView = new Xamarin.Forms.BoxView
                {
                    Color = Xamarin.Forms.Color.White,
                    WidthRequest = wrapper.Width,
                    HeightRequest = wrapper.Height,
                    TranslationX = wrapper.Width * -1,
                    TranslationY = wrapper.Height * -1,
                    Rotation = 45 * -1
                };

                incomingPlacardPresentationView.ConfigureLayout(flatSpectrumHandFanNeutralBoxView);

                if (context.UseOpaqueFlatSpectrumItems)
                {
                    flatSpectrumHandFanNeutralOpaqueBoxView = new Xamarin.Forms.BoxView
                    {
                        Color = Xamarin.Forms.Color.White.MultiplyAlpha(0.5),
                        WidthRequest = wrapper.Width,
                        HeightRequest = wrapper.Height,
                        TranslationX = wrapper.Width * -1,
                        TranslationY = wrapper.Height * -1,
                        Rotation = 45 * -1
                    };

                    incomingPlacardPresentationView.ConfigureLayout(flatSpectrumHandFanNeutralOpaqueBoxView);
                }
            }

            incomingPlacardPresentationView.PlacardView.TranslationX = wrapper.Width * -1;
            incomingPlacardPresentationView.PlacardView.TranslationY = wrapper.Height * -1;
            incomingPlacardPresentationView.PlacardView.Rotation = 45 * -1;

            if (context.UseNeutralFlatSpectrumItems)
            {
                incomingPlacardPresentationView.WrapLayout.Children.Add(flatSpectrumHandFanNeutralBoxView);
            }

            if (context.UseOpaqueFlatSpectrumItems)
            {
                if (context.UseNeutralFlatSpectrumItems)
                {
                    incomingPlacardPresentationView.WrapLayout.Children.Add(flatSpectrumHandFanNeutralOpaqueBoxView);
                }

                incomingPlacardPresentationView.WrapLayout.Children.Add(flatSpectrumHandFanSecoundaryOpaqueBoxView);
                incomingPlacardPresentationView.WrapLayout.Children.Add(flatSpectrumHandFanPrimaryOpaqueBoxView);
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

            if (flatSpectrumHandFanNeutralOpaqueBoxView != null) storyboard.Animations.Add(CreateAnimationItemStoryboard(flatSpectrumHandFanNeutralOpaqueBoxView, 110 * flatSpectrumItemCount++));

            if (flatSpectrumHandFanSecoundaryOpaqueBoxView != null) storyboard.Animations.Add(CreateAnimationItemStoryboard(flatSpectrumHandFanSecoundaryOpaqueBoxView, 110 * flatSpectrumItemCount++));

            if (flatSpectrumHandFanPrimaryOpaqueBoxView != null) storyboard.Animations.Add(CreateAnimationItemStoryboard(flatSpectrumHandFanPrimaryOpaqueBoxView, 110 * flatSpectrumItemCount++));

            if (flatSpectrumHandFanNeutralBoxView != null) storyboard.Animations.Add(CreateAnimationItemStoryboard(flatSpectrumHandFanNeutralBoxView, 110 * flatSpectrumItemCount++));

            storyboard.Animations.Add(CreateAnimationItemStoryboard(flatSpectrumHandFanSecoundaryBoxView, 110 * flatSpectrumItemCount++));

            storyboard.Animations.Add(CreateAnimationItemStoryboard(flatSpectrumHandFanPrimaryBoxView, 110 * flatSpectrumItemCount++));

            var incomingPlacardPresentationViewPlacardViewShowInAnimation = CreateAnimationItemStoryboard(incomingPlacardPresentationView.PlacardView, 110 * flatSpectrumItemCount);

            factShowInAnimation = (incomingPlacardPresentationViewPlacardViewShowInAnimation as Storyboard).Animations[1];

            storyboard.Animations.Add(incomingPlacardPresentationViewPlacardViewShowInAnimation);

            return storyboard;
        }

        private Storyboard CreateAnimationItemStoryboard(Xamarin.Forms.VisualElement target, int delayMilisecounds = 0)
        {
            var storyboard = new Storyboard
            {
                Target = target,
                Delay = delayMilisecounds.ToString()
            };

            storyboard.Animations.Add(new TranslateToAnimation { TranslateX = 0, TranslateY = 0, EasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicOut });
            storyboard.Animations.Add(new RotateToAnimation { Rotation = 0, EasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicOut });

            return storyboard;
        }
    }
}