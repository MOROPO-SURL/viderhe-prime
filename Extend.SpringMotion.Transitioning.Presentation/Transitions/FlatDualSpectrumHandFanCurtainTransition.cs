using SmartSolucionesCuba.XFToolKit.Presentation.Transform.Animations;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Extend.SpringMotion.Transitioning.Presentation.Transitions
{
    public class FlatDualSpectrumHandFanCurtainTransition : Common.Transitioning.Presentation.Transitions.AbstractTransition
    {
        private Xamarin.Forms.BoxView startFlatSpectrumHandFanPrimaryBoxView;
        private Xamarin.Forms.BoxView startFlatSpectrumHandFanSecoundaryBoxView;
        private Xamarin.Forms.BoxView startFlatSpectrumHandFanNeutralBoxView;
        private Xamarin.Forms.BoxView startFlatSpectrumHandFanPrimaryOpaqueBoxView;
        private Xamarin.Forms.BoxView startFlatSpectrumHandFanSecoundaryOpaqueBoxView;
        private Xamarin.Forms.BoxView startFlatSpectrumHandFanNeutralOpaqueBoxView;

        private Xamarin.Forms.BoxView endFlatSpectrumHandFanPrimaryBoxView;
        private Xamarin.Forms.BoxView endFlatSpectrumHandFanSecoundaryBoxView;
        private Xamarin.Forms.BoxView endFlatSpectrumHandFanNeutralBoxView;
        private Xamarin.Forms.BoxView endFlatSpectrumHandFanPrimaryOpaqueBoxView;
        private Xamarin.Forms.BoxView endFlatSpectrumHandFanSecoundaryOpaqueBoxView;
        private Xamarin.Forms.BoxView endFlatSpectrumHandFanNeutralOpaqueBoxView;

        private Models.Context.FlatDualSpectrumHandFanCurtainTransitionContextModel context;

        private AbstractAnimation factShowInAnimation;

        public FlatDualSpectrumHandFanCurtainTransition(Common.Presentation.Views.BasePlacardPresentationView incomingPlacardPresentationView, Common.Presentation.Views.BasePlacardPresentationView outgoingPlacardPresentationView, Models.Context.FlatDualSpectrumHandFanCurtainTransitionContextModel context, System.Action<Common.Presentation.Views.BasePlacardPresentationView> motionCompletedActionCallback) : base(incomingPlacardPresentationView, outgoingPlacardPresentationView, motionCompletedActionCallback)
        {
            this.context = context;
        }

        protected override AbstractAnimation FactShowInAnimation => factShowInAnimation;

        public override void GetReady(Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider, Xamarin.Forms.Layout<Xamarin.Forms.View> wrapper)
        {
            var flatSpectrumHandFanBoxViewRequiredWidth = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Orientation == Xamarin.Essentials.DisplayOrientation.Portrait ? wrapper.Width : wrapper.Width / 2;
            var flatSpectrumHandFanBoxViewRequiredHeight = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Orientation == Xamarin.Essentials.DisplayOrientation.Portrait ? wrapper.Height / 2 : wrapper.Height;
            var startFlatSpectrumHandFanBoxViewHorizontalLayoutOption = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Orientation == Xamarin.Essentials.DisplayOrientation.Portrait ? Xamarin.Forms.LayoutOptions.Fill : Xamarin.Forms.LayoutOptions.Start;
            var startFlatSpectrumHandFanBoxViewVerticalLayoutOtion = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Orientation == Xamarin.Essentials.DisplayOrientation.Portrait ? Xamarin.Forms.LayoutOptions.Start : Xamarin.Forms.LayoutOptions.Fill;
            var endFlatSpectrumHandFanBoxViewHorizontalLayoutOption = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Orientation == Xamarin.Essentials.DisplayOrientation.Portrait ? Xamarin.Forms.LayoutOptions.Fill : Xamarin.Forms.LayoutOptions.End;
            var endFlatSpectrumHandFanBoxViewVerticalLayoutOtion = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Orientation == Xamarin.Essentials.DisplayOrientation.Portrait ? Xamarin.Forms.LayoutOptions.End : Xamarin.Forms.LayoutOptions.Fill;
            var startFlatSpectrumHandFanBoxViewVerticalTranslationY = flatSpectrumHandFanBoxViewRequiredHeight * -1;
            var startFlatSpectrumHandFanBoxViewVerticalTranslationX = flatSpectrumHandFanBoxViewRequiredWidth * -1;
            var endFlatSpectrumHandFanBoxViewVerticalTranslationY = flatSpectrumHandFanBoxViewRequiredHeight;
            var endFlatSpectrumHandFanBoxViewVerticalTranslationX = flatSpectrumHandFanBoxViewRequiredWidth;
            var startFlatSpectrumHandFanBoxViewVerticalRotation = 45 * -1;
            var endFlatSpectrumHandFanBoxViewVerticalRotation = 45;

            if (Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Orientation == Xamarin.Essentials.DisplayOrientation.Portrait)
            {
                if (context.Mode == Enumerations.DualSpectrumHandFanCurtainMode.OutRotation)
                {
                    startFlatSpectrumHandFanBoxViewVerticalTranslationY = 0;
                    endFlatSpectrumHandFanBoxViewVerticalTranslationY = 0;
                }
            }
            else
            {
                if (context.Mode == Enumerations.DualSpectrumHandFanCurtainMode.OutRotation)
                {
                    startFlatSpectrumHandFanBoxViewVerticalTranslationX = 0;
                    endFlatSpectrumHandFanBoxViewVerticalTranslationX = 0;
                }
            }

            if (context.Mode == Enumerations.DualSpectrumHandFanCurtainMode.OutRotation)
            {
                startFlatSpectrumHandFanBoxViewVerticalRotation = 0;
                endFlatSpectrumHandFanBoxViewVerticalRotation = 0;
            }

            startFlatSpectrumHandFanPrimaryBoxView = new Xamarin.Forms.BoxView
            {
                HorizontalOptions = startFlatSpectrumHandFanBoxViewHorizontalLayoutOption,
                VerticalOptions = startFlatSpectrumHandFanBoxViewVerticalLayoutOtion,
                Color = accountInformationProvider.PrimaryColor,
                WidthRequest = flatSpectrumHandFanBoxViewRequiredWidth,
                HeightRequest = flatSpectrumHandFanBoxViewRequiredHeight,
                TranslationX = startFlatSpectrumHandFanBoxViewVerticalTranslationX,
                TranslationY = startFlatSpectrumHandFanBoxViewVerticalTranslationY,
                Rotation = startFlatSpectrumHandFanBoxViewVerticalRotation
            };

            incomingPlacardPresentationView.ConfigureLayout(startFlatSpectrumHandFanPrimaryBoxView);

            endFlatSpectrumHandFanPrimaryBoxView = new Xamarin.Forms.BoxView
            {
                HorizontalOptions = endFlatSpectrumHandFanBoxViewHorizontalLayoutOption,
                VerticalOptions = endFlatSpectrumHandFanBoxViewVerticalLayoutOtion,
                Color = accountInformationProvider.PrimaryColor,
                WidthRequest = flatSpectrumHandFanBoxViewRequiredWidth,
                HeightRequest = flatSpectrumHandFanBoxViewRequiredHeight,
                TranslationX = endFlatSpectrumHandFanBoxViewVerticalTranslationX,
                TranslationY = endFlatSpectrumHandFanBoxViewVerticalTranslationY,
                Rotation = endFlatSpectrumHandFanBoxViewVerticalRotation
            };

            incomingPlacardPresentationView.ConfigureLayout(endFlatSpectrumHandFanPrimaryBoxView);

            startFlatSpectrumHandFanSecoundaryBoxView = new Xamarin.Forms.BoxView
            {
                HorizontalOptions = startFlatSpectrumHandFanBoxViewHorizontalLayoutOption,
                VerticalOptions = startFlatSpectrumHandFanBoxViewVerticalLayoutOtion,
                Color = accountInformationProvider.SecoundaryColor,
                WidthRequest = flatSpectrumHandFanBoxViewRequiredWidth,
                HeightRequest = flatSpectrumHandFanBoxViewRequiredHeight,
                TranslationX = startFlatSpectrumHandFanBoxViewVerticalTranslationX,
                TranslationY = startFlatSpectrumHandFanBoxViewVerticalTranslationY,
                Rotation = startFlatSpectrumHandFanBoxViewVerticalRotation
            };

            incomingPlacardPresentationView.ConfigureLayout(startFlatSpectrumHandFanSecoundaryBoxView);

            endFlatSpectrumHandFanSecoundaryBoxView = new Xamarin.Forms.BoxView
            {
                HorizontalOptions = endFlatSpectrumHandFanBoxViewHorizontalLayoutOption,
                VerticalOptions = endFlatSpectrumHandFanBoxViewVerticalLayoutOtion,
                Color = accountInformationProvider.SecoundaryColor,
                WidthRequest = flatSpectrumHandFanBoxViewRequiredWidth,
                HeightRequest = flatSpectrumHandFanBoxViewRequiredHeight,
                TranslationX = endFlatSpectrumHandFanBoxViewVerticalTranslationX,
                TranslationY = endFlatSpectrumHandFanBoxViewVerticalTranslationY,
                Rotation = endFlatSpectrumHandFanBoxViewVerticalRotation
            };

            incomingPlacardPresentationView.ConfigureLayout(endFlatSpectrumHandFanSecoundaryBoxView);

            if (context.UseOpaqueFlatSpectrumItems)
            {
                startFlatSpectrumHandFanPrimaryOpaqueBoxView = new Xamarin.Forms.BoxView
                {
                    HorizontalOptions = startFlatSpectrumHandFanBoxViewHorizontalLayoutOption,
                    VerticalOptions = startFlatSpectrumHandFanBoxViewVerticalLayoutOtion,
                    Color = accountInformationProvider.PrimaryColor.MultiplyAlpha(0.5),
                    WidthRequest = flatSpectrumHandFanBoxViewRequiredWidth,
                    HeightRequest = flatSpectrumHandFanBoxViewRequiredHeight,
                    TranslationX = startFlatSpectrumHandFanBoxViewVerticalTranslationX,
                    TranslationY = startFlatSpectrumHandFanBoxViewVerticalTranslationY,
                    Rotation = startFlatSpectrumHandFanBoxViewVerticalRotation
                };

                incomingPlacardPresentationView.ConfigureLayout(startFlatSpectrumHandFanPrimaryOpaqueBoxView);

                endFlatSpectrumHandFanPrimaryOpaqueBoxView = new Xamarin.Forms.BoxView
                {
                    HorizontalOptions = endFlatSpectrumHandFanBoxViewHorizontalLayoutOption,
                    VerticalOptions = endFlatSpectrumHandFanBoxViewVerticalLayoutOtion,
                    Color = accountInformationProvider.PrimaryColor.MultiplyAlpha(0.5),
                    WidthRequest = flatSpectrumHandFanBoxViewRequiredWidth,
                    HeightRequest = flatSpectrumHandFanBoxViewRequiredHeight,
                    TranslationX = endFlatSpectrumHandFanBoxViewVerticalTranslationX,
                    TranslationY = endFlatSpectrumHandFanBoxViewVerticalTranslationY,
                    Rotation = endFlatSpectrumHandFanBoxViewVerticalRotation
                };

                incomingPlacardPresentationView.ConfigureLayout(endFlatSpectrumHandFanPrimaryOpaqueBoxView);

                startFlatSpectrumHandFanSecoundaryOpaqueBoxView = new Xamarin.Forms.BoxView
                {
                    HorizontalOptions = startFlatSpectrumHandFanBoxViewHorizontalLayoutOption,
                    VerticalOptions = startFlatSpectrumHandFanBoxViewVerticalLayoutOtion,
                    Color = accountInformationProvider.SecoundaryColor.MultiplyAlpha(0.5),
                    WidthRequest = flatSpectrumHandFanBoxViewRequiredWidth,
                    HeightRequest = flatSpectrumHandFanBoxViewRequiredHeight,
                    TranslationX = startFlatSpectrumHandFanBoxViewVerticalTranslationX,
                    TranslationY = startFlatSpectrumHandFanBoxViewVerticalTranslationY,
                    Rotation = startFlatSpectrumHandFanBoxViewVerticalRotation
                };

                incomingPlacardPresentationView.ConfigureLayout(startFlatSpectrumHandFanSecoundaryOpaqueBoxView);

                endFlatSpectrumHandFanSecoundaryOpaqueBoxView = new Xamarin.Forms.BoxView
                {
                    HorizontalOptions = endFlatSpectrumHandFanBoxViewHorizontalLayoutOption,
                    VerticalOptions = endFlatSpectrumHandFanBoxViewVerticalLayoutOtion,
                    Color = accountInformationProvider.SecoundaryColor.MultiplyAlpha(0.5),
                    WidthRequest = flatSpectrumHandFanBoxViewRequiredWidth,
                    HeightRequest = flatSpectrumHandFanBoxViewRequiredHeight,
                    TranslationX = endFlatSpectrumHandFanBoxViewVerticalTranslationX,
                    TranslationY = endFlatSpectrumHandFanBoxViewVerticalTranslationY,
                    Rotation = endFlatSpectrumHandFanBoxViewVerticalRotation
                };

                incomingPlacardPresentationView.ConfigureLayout(endFlatSpectrumHandFanSecoundaryOpaqueBoxView);
            }

            if (context.UseNeutralFlatSpectrumItems)
            {
                startFlatSpectrumHandFanNeutralBoxView = new Xamarin.Forms.BoxView
                {
                    HorizontalOptions = startFlatSpectrumHandFanBoxViewHorizontalLayoutOption,
                    VerticalOptions = startFlatSpectrumHandFanBoxViewVerticalLayoutOtion,
                    Color = Xamarin.Forms.Color.White,
                    WidthRequest = flatSpectrumHandFanBoxViewRequiredWidth,
                    HeightRequest = flatSpectrumHandFanBoxViewRequiredHeight,
                    TranslationX = startFlatSpectrumHandFanBoxViewVerticalTranslationX,
                    TranslationY = startFlatSpectrumHandFanBoxViewVerticalTranslationY,
                    Rotation = startFlatSpectrumHandFanBoxViewVerticalRotation
                };

                incomingPlacardPresentationView.ConfigureLayout(startFlatSpectrumHandFanNeutralBoxView);

                endFlatSpectrumHandFanNeutralBoxView = new Xamarin.Forms.BoxView
                {
                    HorizontalOptions = endFlatSpectrumHandFanBoxViewHorizontalLayoutOption,
                    VerticalOptions = endFlatSpectrumHandFanBoxViewVerticalLayoutOtion,
                    Color = Xamarin.Forms.Color.White,
                    WidthRequest = flatSpectrumHandFanBoxViewRequiredWidth,
                    HeightRequest = flatSpectrumHandFanBoxViewRequiredHeight,
                    TranslationX = endFlatSpectrumHandFanBoxViewVerticalTranslationX,
                    TranslationY = endFlatSpectrumHandFanBoxViewVerticalTranslationY,
                    Rotation = endFlatSpectrumHandFanBoxViewVerticalRotation
                };

                incomingPlacardPresentationView.ConfigureLayout(endFlatSpectrumHandFanNeutralBoxView);

                if (context.UseOpaqueFlatSpectrumItems)
                {
                    startFlatSpectrumHandFanNeutralOpaqueBoxView = new Xamarin.Forms.BoxView
                    {
                        HorizontalOptions = startFlatSpectrumHandFanBoxViewHorizontalLayoutOption,
                        VerticalOptions = startFlatSpectrumHandFanBoxViewVerticalLayoutOtion,
                        Color = Xamarin.Forms.Color.White.MultiplyAlpha(0.5),
                        WidthRequest = flatSpectrumHandFanBoxViewRequiredWidth,
                        HeightRequest = flatSpectrumHandFanBoxViewRequiredHeight,
                        TranslationX = startFlatSpectrumHandFanBoxViewVerticalTranslationX,
                        TranslationY = startFlatSpectrumHandFanBoxViewVerticalTranslationY,
                        Rotation = startFlatSpectrumHandFanBoxViewVerticalRotation
                    };

                    incomingPlacardPresentationView.ConfigureLayout(startFlatSpectrumHandFanNeutralOpaqueBoxView);
                }

                if (context.UseOpaqueFlatSpectrumItems)
                {
                    endFlatSpectrumHandFanNeutralOpaqueBoxView = new Xamarin.Forms.BoxView
                    {
                        HorizontalOptions = endFlatSpectrumHandFanBoxViewHorizontalLayoutOption,
                        VerticalOptions = endFlatSpectrumHandFanBoxViewVerticalLayoutOtion,
                        Color = Xamarin.Forms.Color.White.MultiplyAlpha(0.5),
                        WidthRequest = flatSpectrumHandFanBoxViewRequiredWidth,
                        HeightRequest = flatSpectrumHandFanBoxViewRequiredHeight,
                        TranslationX = endFlatSpectrumHandFanBoxViewVerticalTranslationX,
                        TranslationY = endFlatSpectrumHandFanBoxViewVerticalTranslationY,
                        Rotation = endFlatSpectrumHandFanBoxViewVerticalRotation
                    };

                    incomingPlacardPresentationView.ConfigureLayout(endFlatSpectrumHandFanNeutralOpaqueBoxView);
                }
            }

            incomingPlacardPresentationView.PlacardView.IsVisible = false;

            if (context.UseNeutralFlatSpectrumItems)
            {
                incomingPlacardPresentationView.WrapLayout.Children.Add(startFlatSpectrumHandFanNeutralBoxView);
                incomingPlacardPresentationView.WrapLayout.Children.Add(endFlatSpectrumHandFanNeutralBoxView);
            }

            if (context.UseOpaqueFlatSpectrumItems)
            {
                if (context.UseNeutralFlatSpectrumItems)
                {
                    incomingPlacardPresentationView.WrapLayout.Children.Add(startFlatSpectrumHandFanNeutralOpaqueBoxView);
                    incomingPlacardPresentationView.WrapLayout.Children.Add(endFlatSpectrumHandFanNeutralOpaqueBoxView);
                }

                incomingPlacardPresentationView.WrapLayout.Children.Add(startFlatSpectrumHandFanSecoundaryOpaqueBoxView);
                incomingPlacardPresentationView.WrapLayout.Children.Add(endFlatSpectrumHandFanSecoundaryOpaqueBoxView);
                incomingPlacardPresentationView.WrapLayout.Children.Add(startFlatSpectrumHandFanPrimaryOpaqueBoxView);
                incomingPlacardPresentationView.WrapLayout.Children.Add(endFlatSpectrumHandFanPrimaryOpaqueBoxView);
            }

            incomingPlacardPresentationView.WrapLayout.Children.Add(startFlatSpectrumHandFanSecoundaryBoxView);
            incomingPlacardPresentationView.WrapLayout.Children.Add(endFlatSpectrumHandFanSecoundaryBoxView);
            incomingPlacardPresentationView.WrapLayout.Children.Add(startFlatSpectrumHandFanPrimaryBoxView);
            incomingPlacardPresentationView.WrapLayout.Children.Add(endFlatSpectrumHandFanPrimaryBoxView);
        }

        protected override void GarbageCollect()
        {
            if (context.UseOpaqueFlatSpectrumItems)
            {
                if (context.UseNeutralFlatSpectrumItems)
                {
                    incomingPlacardPresentationView.WrapLayout.Children.Remove(startFlatSpectrumHandFanNeutralOpaqueBoxView);
                    incomingPlacardPresentationView.WrapLayout.Children.Remove(endFlatSpectrumHandFanNeutralOpaqueBoxView);
                }

                incomingPlacardPresentationView.WrapLayout.Children.Remove(startFlatSpectrumHandFanSecoundaryOpaqueBoxView);
                incomingPlacardPresentationView.WrapLayout.Children.Remove(endFlatSpectrumHandFanSecoundaryOpaqueBoxView);
                incomingPlacardPresentationView.WrapLayout.Children.Remove(startFlatSpectrumHandFanPrimaryOpaqueBoxView);
                incomingPlacardPresentationView.WrapLayout.Children.Remove(endFlatSpectrumHandFanPrimaryOpaqueBoxView);
            }

            if (context.UseNeutralFlatSpectrumItems)
            {
                incomingPlacardPresentationView.WrapLayout.Children.Remove(startFlatSpectrumHandFanNeutralBoxView);
                incomingPlacardPresentationView.WrapLayout.Children.Remove(endFlatSpectrumHandFanNeutralBoxView);
            }

            incomingPlacardPresentationView.WrapLayout.Children.Remove(startFlatSpectrumHandFanSecoundaryBoxView);
            incomingPlacardPresentationView.WrapLayout.Children.Remove(endFlatSpectrumHandFanSecoundaryBoxView);
            incomingPlacardPresentationView.WrapLayout.Children.Remove(startFlatSpectrumHandFanPrimaryBoxView);
            incomingPlacardPresentationView.WrapLayout.Children.Remove(endFlatSpectrumHandFanPrimaryBoxView);

            startFlatSpectrumHandFanNeutralOpaqueBoxView = null;
            startFlatSpectrumHandFanNeutralBoxView = null;
            startFlatSpectrumHandFanSecoundaryOpaqueBoxView = null;
            startFlatSpectrumHandFanPrimaryOpaqueBoxView = null;
            startFlatSpectrumHandFanSecoundaryBoxView = null;
            startFlatSpectrumHandFanPrimaryBoxView = null;

            endFlatSpectrumHandFanNeutralOpaqueBoxView = null;
            endFlatSpectrumHandFanNeutralBoxView = null;
            endFlatSpectrumHandFanSecoundaryOpaqueBoxView = null;
            endFlatSpectrumHandFanPrimaryOpaqueBoxView = null;
            endFlatSpectrumHandFanSecoundaryBoxView = null;
            endFlatSpectrumHandFanPrimaryBoxView = null;

            factShowInAnimation = null;

            context = null;
        }

        protected override Storyboard CreateShowInMotionStoryboard()
        {
            var storyboard = new Storyboard
            {
                IsParallel = true
            };

            var duration = 500;

            var inFlatSpectrumItemCount = 0;

            if (startFlatSpectrumHandFanNeutralOpaqueBoxView != null) storyboard.Animations.Add(CreateInItemAnimationStoryboard(startFlatSpectrumHandFanNeutralOpaqueBoxView, duration, 110 * inFlatSpectrumItemCount));
            if (endFlatSpectrumHandFanNeutralOpaqueBoxView != null) storyboard.Animations.Add(CreateInItemAnimationStoryboard(endFlatSpectrumHandFanNeutralOpaqueBoxView, duration, 110 * inFlatSpectrumItemCount++));

            if (startFlatSpectrumHandFanSecoundaryOpaqueBoxView != null) storyboard.Animations.Add(CreateInItemAnimationStoryboard(startFlatSpectrumHandFanSecoundaryOpaqueBoxView, duration, 110 * inFlatSpectrumItemCount));
            if (endFlatSpectrumHandFanSecoundaryOpaqueBoxView != null) storyboard.Animations.Add(CreateInItemAnimationStoryboard(endFlatSpectrumHandFanSecoundaryOpaqueBoxView, duration, 110 * inFlatSpectrumItemCount++));

            if (startFlatSpectrumHandFanPrimaryOpaqueBoxView != null) storyboard.Animations.Add(CreateInItemAnimationStoryboard(startFlatSpectrumHandFanPrimaryOpaqueBoxView, duration, 110 * inFlatSpectrumItemCount));
            if (endFlatSpectrumHandFanPrimaryOpaqueBoxView != null) storyboard.Animations.Add(CreateInItemAnimationStoryboard(startFlatSpectrumHandFanPrimaryOpaqueBoxView, duration, 110 * inFlatSpectrumItemCount++));

            if (startFlatSpectrumHandFanNeutralBoxView != null) storyboard.Animations.Add(CreateInItemAnimationStoryboard(startFlatSpectrumHandFanNeutralBoxView, duration, 110 * inFlatSpectrumItemCount));
            if (endFlatSpectrumHandFanNeutralBoxView != null) storyboard.Animations.Add(CreateInItemAnimationStoryboard(endFlatSpectrumHandFanNeutralBoxView, duration, 110 * inFlatSpectrumItemCount++));

            storyboard.Animations.Add(CreateInItemAnimationStoryboard(startFlatSpectrumHandFanSecoundaryBoxView, duration, 110 * inFlatSpectrumItemCount));
            storyboard.Animations.Add(CreateInItemAnimationStoryboard(endFlatSpectrumHandFanSecoundaryBoxView, duration, 110 * inFlatSpectrumItemCount++));

            var startFlatSpectrumHandFanPrimaryBoxViewShowInAnimation = CreateInItemAnimationStoryboard(startFlatSpectrumHandFanPrimaryBoxView, duration, 110 * inFlatSpectrumItemCount);

            var curtainDownStepAnimation = (startFlatSpectrumHandFanPrimaryBoxViewShowInAnimation as Storyboard).Animations[0];

            curtainDownStepAnimation.Completed += CurtainDownStepAnimation_Completed;

            storyboard.Animations.Add(startFlatSpectrumHandFanPrimaryBoxViewShowInAnimation);
            storyboard.Animations.Add(CreateInItemAnimationStoryboard(endFlatSpectrumHandFanPrimaryBoxView, duration, 110 * inFlatSpectrumItemCount));

            var curtainDownStepDuration = int.Parse(startFlatSpectrumHandFanPrimaryBoxViewShowInAnimation.Delay) + duration;

            var outFlatSpectrumItemCount = 0;

            storyboard.Animations.Add(CreateOutItemAnimationStoryboard(startFlatSpectrumHandFanPrimaryBoxView, duration, 110 * outFlatSpectrumItemCount + curtainDownStepDuration));
            storyboard.Animations.Add(CreateOutItemAnimationStoryboard(endFlatSpectrumHandFanPrimaryBoxView, duration, 110 * outFlatSpectrumItemCount++ + curtainDownStepDuration));

            var lastFlatSpectrumHandFanBoxViewShowInAnimation = CreateOutItemAnimationStoryboard(startFlatSpectrumHandFanSecoundaryBoxView, duration, 110 * outFlatSpectrumItemCount + curtainDownStepDuration);

            storyboard.Animations.Add(lastFlatSpectrumHandFanBoxViewShowInAnimation);
            storyboard.Animations.Add(CreateOutItemAnimationStoryboard(endFlatSpectrumHandFanSecoundaryBoxView, duration, 110 * outFlatSpectrumItemCount++ + curtainDownStepDuration));

            if (startFlatSpectrumHandFanNeutralBoxView != null) storyboard.Animations.Add(CreateOutItemAnimationStoryboard(startFlatSpectrumHandFanNeutralBoxView, duration, 110 * outFlatSpectrumItemCount + curtainDownStepDuration));
            if (endFlatSpectrumHandFanNeutralBoxView != null) storyboard.Animations.Add(CreateOutItemAnimationStoryboard(endFlatSpectrumHandFanNeutralBoxView, duration, 110 * outFlatSpectrumItemCount++ + curtainDownStepDuration));

            if (startFlatSpectrumHandFanPrimaryOpaqueBoxView != null) storyboard.Animations.Add(CreateOutItemAnimationStoryboard(startFlatSpectrumHandFanPrimaryOpaqueBoxView, duration, 110 * outFlatSpectrumItemCount + curtainDownStepDuration));
            if (endFlatSpectrumHandFanPrimaryOpaqueBoxView != null) storyboard.Animations.Add(CreateOutItemAnimationStoryboard(startFlatSpectrumHandFanPrimaryOpaqueBoxView, duration, 110 * outFlatSpectrumItemCount++ + curtainDownStepDuration));

            if (startFlatSpectrumHandFanSecoundaryOpaqueBoxView != null) storyboard.Animations.Add(CreateOutItemAnimationStoryboard(startFlatSpectrumHandFanSecoundaryOpaqueBoxView, duration, 110 * outFlatSpectrumItemCount + curtainDownStepDuration));
            if (endFlatSpectrumHandFanSecoundaryOpaqueBoxView != null) storyboard.Animations.Add(CreateOutItemAnimationStoryboard(endFlatSpectrumHandFanSecoundaryOpaqueBoxView, duration, 110 * outFlatSpectrumItemCount++ + curtainDownStepDuration));

            if (startFlatSpectrumHandFanNeutralOpaqueBoxView != null) storyboard.Animations.Add(CreateOutItemAnimationStoryboard(startFlatSpectrumHandFanNeutralOpaqueBoxView, duration, 110 * outFlatSpectrumItemCount + curtainDownStepDuration));
            if (endFlatSpectrumHandFanNeutralOpaqueBoxView != null) storyboard.Animations.Add(CreateOutItemAnimationStoryboard(endFlatSpectrumHandFanNeutralOpaqueBoxView, duration, 110 * outFlatSpectrumItemCount + curtainDownStepDuration));

            factShowInAnimation = (storyboard.Animations.Last() as Storyboard).Animations[0];

            return storyboard;
        }

        private Storyboard CreateInItemAnimationStoryboard(Xamarin.Forms.VisualElement target, int duration, int delayMilisecounds = 0)
        {
            var storyboard = new Storyboard
            {
                IsParallel = true,
                Target = target,
                Delay = delayMilisecounds.ToString()
            };

            if (context.Mode != Enumerations.DualSpectrumHandFanCurtainMode.OutRotation)
            {
                storyboard.Animations.Add(new TranslateToAnimation { TranslateX = 0, TranslateY = 0, EasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicOut, Duration = duration.ToString() });
                storyboard.Animations.Add(new RotateToAnimation { Rotation = 0, EasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicOut, Duration = duration.ToString() });
            }
            else
            {
                if (Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Orientation == Xamarin.Essentials.DisplayOrientation.Portrait)
                {
                    storyboard.Animations.Add(new TranslateToXAnimation { TranslateX = 0, EasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicOut, Duration = duration.ToString() });
                }
                else
                {
                    storyboard.Animations.Add(new TranslateToYAnimation { TranslateY = 0, EasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicOut, Duration = duration.ToString() });
                }
            }

            return storyboard;
        }

        private Storyboard CreateOutItemAnimationStoryboard(Xamarin.Forms.View target, int duration, int delayMilisecounds = 0)
        {
            var storyboard = new Storyboard
            {
                IsParallel = true,
                Target = target,
                Delay = delayMilisecounds.ToString()
            };

            double rotation;
            double translateX;
            double translateY;

            if (Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Orientation == Xamarin.Essentials.DisplayOrientation.Portrait)
            {
                rotation = target.VerticalOptions.Alignment == Xamarin.Forms.LayoutOptions.Start.Alignment ? 45 : 45 * -1;
                translateX = target.VerticalOptions.Alignment == Xamarin.Forms.LayoutOptions.Start.Alignment ? target.WidthRequest * -1 : target.WidthRequest;
                translateY = target.VerticalOptions.Alignment == Xamarin.Forms.LayoutOptions.Start.Alignment ? target.HeightRequest * -1 : target.HeightRequest;
            }
            else
            {
                rotation = target.HorizontalOptions.Alignment == Xamarin.Forms.LayoutOptions.Start.Alignment ? 45 : 45 * -1;
                translateX = target.HorizontalOptions.Alignment == Xamarin.Forms.LayoutOptions.Start.Alignment ? target.WidthRequest * -1 : target.WidthRequest;
                translateY = target.HorizontalOptions.Alignment == Xamarin.Forms.LayoutOptions.Start.Alignment ? target.HeightRequest * -1 : target.HeightRequest;
            }

            if (context.Mode != Enumerations.DualSpectrumHandFanCurtainMode.InRotation)
            {
                storyboard.Animations.Add(new TranslateToAnimation { TranslateX = translateX, TranslateY = translateY, EasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicOut, Duration = duration.ToString() });
                storyboard.Animations.Add(new RotateToAnimation { Rotation = rotation, EasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicOut, Duration = duration.ToString() });
            }
            else
            {
                if (Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Orientation == Xamarin.Essentials.DisplayOrientation.Portrait)
                {
                    storyboard.Animations.Add(new TranslateToXAnimation { TranslateX = translateX, EasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicOut, Duration = duration.ToString() });
                }
                else
                {
                    storyboard.Animations.Add(new TranslateToYAnimation { TranslateY = translateY, EasingType = XFToolKit.Presentation.Transform.Enumerations.EasingType.CubicOut, Duration = duration.ToString() });
                }
            }

            return storyboard;
        }

        private void CurtainDownStepAnimation_Completed(object sender, System.EventArgs eventArgs)
        {
            (sender as AbstractAnimation).Completed -= CurtainDownStepAnimation_Completed;

            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                incomingPlacardPresentationView.PlacardView.IsVisible = true;
            });
        }
    }
}
