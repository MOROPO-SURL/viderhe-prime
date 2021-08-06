using System;
using CoreGraphics;
using UIKit;

namespace SmartSolucionesCuba.Viderhe.Terminal.Player.iOS.Views
{
    public class LinearProgressBarView : UIView
    {
        private nfloat heightforLienarBar = 5;
        private nfloat widthForLinearBar = 0;
        private static CGRect screenSize = UIScreen.MainScreen.Bounds;
        private bool isAnimationRunning = false;
        private UIView progressBarIndicator;

        public UIColor BackgroundProgressBarColor { get; set; } = UIColor.Green;

        public UIColor ProgressColor { get; set; } = UIColor.Green;

        public nfloat HeightForLinearBar
        {
            get => heightforLienarBar;
            set => heightforLienarBar = value;
        }

        public nfloat WidthForLinearBar
        {
            get => widthForLinearBar;
            set => widthForLinearBar = value;
        }

        public LinearProgressBarView() : base(new CGRect(0, 20, screenSize.Width, 0))
        {
            progressBarIndicator = new UIView(new CGRect(0, 0, 0, HeightForLinearBar));

        }

        public LinearProgressBarView(CGRect frame) : base(frame)
        {
            progressBarIndicator = new UIView(new CGRect(0, 0, 0, HeightForLinearBar));
        }

        public LinearProgressBarView(Foundation.NSCoder nScoder)
        {
            throw new NotImplementedException();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            screenSize = UIScreen.MainScreen.Bounds;

            if (WidthForLinearBar == 0 || WidthForLinearBar == screenSize.Height)
            {
                WidthForLinearBar = screenSize.Width;
            }

            if (UIDeviceOrientationExtensions.IsLandscape(UIDevice.CurrentDevice.Orientation))
            {
                Frame = new CGRect(Frame.X, Frame.Y, WidthForLinearBar, Frame.Height);
            }

            if (UIDeviceOrientationExtensions.IsPortrait(UIDevice.CurrentDevice.Orientation))
            {
                Frame = new CGRect(Frame.X, Frame.Y, WidthForLinearBar, Frame.Height);
            }
        }

        public void StartAnimation()
        {
            ConfigureColors();

            if (!isAnimationRunning)
            {
                isAnimationRunning = true;

                Animate(0.5, 0, UIViewAnimationOptions.TransitionFlipFromLeft, () =>
                {
                    Frame = new CGRect(0, Frame.Y, WidthForLinearBar, HeightForLinearBar);
                }, () =>
                {
                    AddSubview(progressBarIndicator);

                    ConfigureAnimation();
                });
            }
        }

        public void StopAnimation()
        {
            isAnimationRunning = false;

            Animate(0.5, () => {
                progressBarIndicator.Frame = new CGRect(0, 0, WidthForLinearBar, 0);
                Frame = new CGRect(0, Frame.Y, WidthForLinearBar, 0);
            });
        }

        private void ConfigureColors()
        {
            progressBarIndicator.BackgroundColor = ProgressColor;
        }

        private void ConfigureAnimation()
        {
            progressBarIndicator.Frame = new CGRect(0, 0, 0, HeightForLinearBar);


            Animate(0.5, 0, UIViewAnimationOptions.TransitionFlipFromLeft, () =>
            {
                progressBarIndicator.Frame = new CGRect(0, 0, WidthForLinearBar * 0.7, HeightForLinearBar);
            }, null);

            Animate(0.4, 0.4, UIViewAnimationOptions.TransitionFlipFromLeft, () =>
            {
                progressBarIndicator.Frame = new CGRect(Frame.Width, 0, 0, HeightForLinearBar);
            }, () =>
            {
                if (isAnimationRunning) ConfigureAnimation();
            });
        }
    }
}
