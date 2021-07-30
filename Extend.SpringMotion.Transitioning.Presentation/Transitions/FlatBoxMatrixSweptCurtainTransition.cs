using SmartSolucionesCuba.XFToolKit.Presentation.Transform.Animations;

namespace SmartSolucionesCuba.Viderhe.Extend.SpringMotion.Transitioning.Presentation.Transitions
{
    public class FlatBoxMatrixSweptCurtainTransition : Common.Transitioning.Presentation.Transitions.AbstractTransition
    {
        private const int BOX_VIEW_MATRIX_ITEMS_COUNT = 6;
        private const int BOX_VIEW_QUICK_SHOW_ANIMATE_DURATION_MILLISECOUNDS = 240;
        private const int BOX_VIEW_QUICK_SHOW_ANIMATE_DURATION_DELAY_RAZON = 2;
        private const int BOX_VIEW_SOFT_SHOW_ANIMATE_DURATION_MILLISECOUNDS = 675;
        private const int BOX_VIEW_SOFT_SHOW_ANIMATE_DURATION_DELAY_RAZON = 3;

        private Xamarin.Forms.Grid boxViewsGrid;

        private Models.Context.FlatBoxMatrixSweptCurtainTransitionContextModel context;

        private AbstractAnimation squaresShowInAnimation;
        private AbstractAnimation factShowInAnimation;

        public FlatBoxMatrixSweptCurtainTransition(Common.Presentation.Views.BasePlacardPresentationView incomingPlacardPresentationView, Common.Presentation.Views.BasePlacardPresentationView outgoingPlacardPresentationView, Models.Context.FlatBoxMatrixSweptCurtainTransitionContextModel context, System.Action<Common.Presentation.Views.BasePlacardPresentationView> motionCompletedActionCallback) : base(incomingPlacardPresentationView, outgoingPlacardPresentationView, motionCompletedActionCallback)
        {
            this.context = context;
        }

        protected override AbstractAnimation FactShowInAnimation => factShowInAnimation;

        public override void GetReady(Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider, Xamarin.Forms.Layout<Xamarin.Forms.View> wrapper)
        {
            var aspectRation = wrapper.Width / wrapper.Height;

            boxViewsGrid = new Xamarin.Forms.Grid
            {
                ColumnSpacing = 0,
                RowSpacing = 0
            };

            var columCount = BOX_VIEW_MATRIX_ITEMS_COUNT;
            var rowCount = BOX_VIEW_MATRIX_ITEMS_COUNT;

            //var colunmWidth = Xamarin.Forms.GridLength.Star;
            //var rowHeight = Xamarin.Forms.GridLength.Star;

            if (aspectRation == 2)
            {
                columCount *= 2;
            }
            else if (aspectRation == 0.5)
            {
                rowCount *= 2;
            }
            else if (aspectRation > 1)
            {
                columCount = (int)(wrapper.Width / (wrapper.Height / rowCount)) + 1;
            }
            else if (aspectRation < 1)
            {
                rowCount = (int)(wrapper.Height / (wrapper.Width / rowCount)) + 1;
            }

            for (int i = 0; i < rowCount; i++)
            {
                if (rowCount - 1 == i)
                {
                    boxViewsGrid.RowDefinitions.Add(new Xamarin.Forms.RowDefinition
                    {
                        Height = rowCount % BOX_VIEW_MATRIX_ITEMS_COUNT != 0 ? Xamarin.Forms.GridLength.Auto : Xamarin.Forms.GridLength.Star
                    });
                }
                else
                {
                    boxViewsGrid.RowDefinitions.Add(new Xamarin.Forms.RowDefinition());
                }
            }

            for (int i = 0; i < columCount; i++)
            {
                if (columCount - 1 == i)
                {
                    boxViewsGrid.ColumnDefinitions.Add(new Xamarin.Forms.ColumnDefinition
                    {
                        Width = columCount % BOX_VIEW_MATRIX_ITEMS_COUNT != 0 ? Xamarin.Forms.GridLength.Auto : Xamarin.Forms.GridLength.Star
                    });
                }
                else
                {
                    boxViewsGrid.ColumnDefinitions.Add(new Xamarin.Forms.ColumnDefinition());
                }
            }

            for (int x = 0; x < columCount; x++)
            {
                for (int y = 0; y < rowCount; y++)
                {
                    var boxView = new Xamarin.Forms.BoxView
                    {
                        BackgroundColor = context.Color
                    };

                    switch (context.MotionType)
                    {
                        //case Enumerations.SweptTransitionItemMotionType.Fade:
                        //    boxView.Opacity = 0;

                        //    break;

                        case Enumerations.SweptTransitionItemMotionType.Scale:
                            boxView.Scale = 0;

                            break;

                        default:
                            boxView.Opacity = 0;
                            boxView.Scale = 0;

                            break;
                    }

                    Xamarin.Forms.Grid.SetColumn(boxView, x);
                    Xamarin.Forms.Grid.SetRow(boxView, y);

                    boxViewsGrid.Children.Add(boxView);
                }
            }

            incomingPlacardPresentationView.PlacardView.IsVisible = false;

            incomingPlacardPresentationView.WrapLayout.Children.Add(boxViewsGrid);
        }

        protected override Storyboard CreateShowInMotionStoryboard()
        {
            var duration = System.TimeSpan.FromMilliseconds(GetContextualBoxViewShowAnimateDurationMillisecounds());

            var delay = duration.TotalMilliseconds / GetContextualBoxViewShowAnimateDurationDelayRazon();

            var columnCount = boxViewsGrid.ColumnDefinitions.Count;
            var rowCount = boxViewsGrid.RowDefinitions.Count;

            var storyboard = new Storyboard
            {
                IsParallel = true
            };

            var squaresShowInStoryboard = new Storyboard
            {
                IsParallel = true
            };

            var squaresShowOutStoryboard = new Storyboard
            {
                IsParallel = true,
                Delay = ((int)(delay * columnCount) + 100).ToString()
            };


            foreach (var item in boxViewsGrid.Children)
            {
                squaresShowInStoryboard.Animations.Add(CreateSquareShowInAnimation(item));

                squaresShowOutStoryboard.Animations.Add(CreateSquareShowOutAnimation(item));
            }

            storyboard.Animations.Add(squaresShowInStoryboard);
            storyboard.Animations.Add(squaresShowOutStoryboard);

            squaresShowInAnimation.Completed += SquaresShowInAnimation_Completed;

            return storyboard;
        }

        private void SquaresShowInAnimation_Completed(object sender, System.EventArgs e)
        {
            (sender as AbstractAnimation).Completed -= SquaresShowInAnimation_Completed;

            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                incomingPlacardPresentationView.PlacardView.IsVisible = true;
            });
        }

        protected override void GarbageCollect()
        {
            while (boxViewsGrid.Children.Count > 0)
            {
                boxViewsGrid.Children.RemoveAt(0);
            }

            incomingPlacardPresentationView.WrapLayout.Children.Remove(boxViewsGrid);

            boxViewsGrid = null;

            squaresShowInAnimation = null;
            factShowInAnimation = null;

            context = null;
        }

        private int GetContextualBoxViewShowAnimateDurationMillisecounds()
        {
            return context.MotionMode == Enumerations.SweptTransitionItemMotionMode.Quick ? BOX_VIEW_QUICK_SHOW_ANIMATE_DURATION_MILLISECOUNDS : BOX_VIEW_SOFT_SHOW_ANIMATE_DURATION_MILLISECOUNDS;
        }

        private int GetContextualBoxViewShowAnimateDurationDelayRazon()
        {
            return context.MotionMode == Enumerations.SweptTransitionItemMotionMode.Quick ? BOX_VIEW_QUICK_SHOW_ANIMATE_DURATION_DELAY_RAZON : BOX_VIEW_SOFT_SHOW_ANIMATE_DURATION_DELAY_RAZON;
        }

        private AbstractAnimation CreateSquareShowInAnimation(Xamarin.Forms.View target)
        {
            var duration = System.TimeSpan.FromMilliseconds(GetContextualBoxViewShowAnimateDurationMillisecounds());

            var delay = duration.TotalMilliseconds / GetContextualBoxViewShowAnimateDurationDelayRazon();

            var columnCount = boxViewsGrid.ColumnDefinitions.Count;
            var rowCount = boxViewsGrid.RowDefinitions.Count;

            var column = Xamarin.Forms.Grid.GetColumn(target);
            var row = Xamarin.Forms.Grid.GetRow(target);

            AbstractAnimation result;

            if (context.MotionType == Enumerations.SweptTransitionItemMotionType.Scale)
            {
                result = new ScaleToAnimation { Target = target, Scale = 1, Duration = duration.TotalMilliseconds.ToString() };
            }
            //else if (context.MotionType == Enumerations.SweptTransitionItemMotionType.Fade)
            //{
            //    result = new FadeToAnimation { Target = target, Opacity = 1, Duration = duration.TotalMilliseconds.ToString() };
            //}
            else
            {
                var storyboard = new Storyboard
                {
                    Target = target,
                    IsParallel = true
                };

                storyboard.Animations.Add(new ScaleToAnimation { Scale = 1, Duration = duration.TotalMilliseconds.ToString() });
                storyboard.Animations.Add(new FadeToAnimation { Opacity = 1, Duration = duration.TotalMilliseconds.ToString() });

                result = storyboard;
            }

            switch (context.Orientation)
            {
                case Enumerations.SweptTransitionOrientation.LeftToRight:
                    if (column == columnCount - 1 && squaresShowInAnimation == null)
                    {
                        squaresShowInAnimation = result;
                    }

                    result.Delay = System.Convert.ToString((int)(delay * column));

                    break;

                case Enumerations.SweptTransitionOrientation.TopToBottom:
                    if (row == rowCount - 1 && squaresShowInAnimation == null)
                    {
                        squaresShowInAnimation = result;
                    }

                    result.Delay = System.Convert.ToString((int)(delay * row));

                    break;

                case Enumerations.SweptTransitionOrientation.RightToLeft:
                    if (column == 0 && squaresShowInAnimation == null)
                    {
                        squaresShowInAnimation = result;
                    }

                    var maxHorizontalDelay = delay * columnCount;

                    result.Delay = System.Convert.ToString((int)(maxHorizontalDelay - (delay * column)));

                    break;

                case Enumerations.SweptTransitionOrientation.BottomToTop:
                    if (row == 0 && squaresShowInAnimation == null)
                    {
                        squaresShowInAnimation = result;
                    }

                    var maxVerticalDelay = delay * rowCount;

                    result.Delay = System.Convert.ToString((int)(maxVerticalDelay - (delay * row)));

                    break;
            }

            if (squaresShowInAnimation is Storyboard)
            {
                squaresShowInAnimation = (squaresShowInAnimation as Storyboard).Animations[0];
            }

            return result;
        }

        private AbstractAnimation CreateSquareShowOutAnimation(Xamarin.Forms.View target)
        {
            var duration = System.TimeSpan.FromMilliseconds(GetContextualBoxViewShowAnimateDurationMillisecounds());

            var delay = duration.TotalMilliseconds / GetContextualBoxViewShowAnimateDurationDelayRazon();

            var columnCount = boxViewsGrid.ColumnDefinitions.Count;
            var rowCount = boxViewsGrid.RowDefinitions.Count;

            var column = Xamarin.Forms.Grid.GetColumn(target);
            var row = Xamarin.Forms.Grid.GetRow(target);

            AbstractAnimation result;

            if (context.MotionType == Enumerations.SweptTransitionItemMotionType.Scale)
            {
                result = new ScaleToAnimation { Target = target, Scale = 0, Duration = duration.TotalMilliseconds.ToString() };
            }
            //else if (context.MotionType == Enumerations.SweptTransitionItemMotionType.Fade)
            //{
            //    result = new FadeToAnimation { Target = target, Opacity = 0, Duration = duration.TotalMilliseconds.ToString() };
            //}
            else
            {
                var storyboard = new Storyboard
                {
                    Target = target,
                    IsParallel = true
                };

                storyboard.Animations.Add(new ScaleToAnimation { Scale = 0, Duration = duration.TotalMilliseconds.ToString() });
                storyboard.Animations.Add(new FadeToAnimation { Opacity = 0, Duration = duration.TotalMilliseconds.ToString() });

                result = storyboard;
            }

            if (context.InOutKind == Enumerations.SweptTransitionInOutKind.Elastic)
            {
                switch (context.Orientation)
                {
                    case Enumerations.SweptTransitionOrientation.RightToLeft:
                        if (column == columnCount - 1 && factShowInAnimation == null)
                        {
                            factShowInAnimation = result;
                        }

                        result.Delay = System.Convert.ToString((int)(delay * column));

                        break;

                    case Enumerations.SweptTransitionOrientation.BottomToTop:
                        if (row == rowCount - 1 && factShowInAnimation == null)
                        {
                            factShowInAnimation = result;
                        }

                        result.Delay = System.Convert.ToString((int)(delay * row));

                        break;

                    case Enumerations.SweptTransitionOrientation.LeftToRight:
                        if (column == 0 && factShowInAnimation == null)
                        {
                            factShowInAnimation = result;
                        }

                        var maxHorizontalDelay = delay * columnCount;

                        result.Delay = System.Convert.ToString((int)(maxHorizontalDelay - (delay * column)));

                        break;

                    case Enumerations.SweptTransitionOrientation.TopToBottom:
                        if (row == 0 && factShowInAnimation == null)
                        {
                            factShowInAnimation = result;
                        }

                        var maxVerticalDelay = delay * rowCount;

                        result.Delay = System.Convert.ToString((int)(maxVerticalDelay - (delay * row)));

                        break;
                }
            }
            else
            {
                switch (context.Orientation)
                {
                    case Enumerations.SweptTransitionOrientation.LeftToRight:
                        if (column == columnCount - 1 && factShowInAnimation == null)
                        {
                            factShowInAnimation = result;
                        }

                        result.Delay = System.Convert.ToString((int)(delay * column));

                        break;

                    case Enumerations.SweptTransitionOrientation.TopToBottom:
                        if (row == rowCount - 1 && factShowInAnimation == null)
                        {
                            factShowInAnimation = result;
                        }

                        result.Delay = System.Convert.ToString((int)(delay * row));

                        break;

                    case Enumerations.SweptTransitionOrientation.RightToLeft:
                        if (column == 0 && factShowInAnimation == null)
                        {
                            factShowInAnimation = result;
                        }

                        var maxHorizontalDelay = delay * columnCount;

                        result.Delay = System.Convert.ToString((int)(maxHorizontalDelay - (delay * column)));

                        break;

                    case Enumerations.SweptTransitionOrientation.BottomToTop:
                        if (row == 0 && factShowInAnimation == null)
                        {
                            factShowInAnimation = result;
                        }

                        var maxVerticalDelay = delay * rowCount;

                        result.Delay = System.Convert.ToString((int)(maxVerticalDelay - (delay * row)));

                        break;
                }
            }

            if (factShowInAnimation is Storyboard)
            {
                factShowInAnimation = (factShowInAnimation as Storyboard).Animations[0];
            }

            return result;
        }
    }
}
