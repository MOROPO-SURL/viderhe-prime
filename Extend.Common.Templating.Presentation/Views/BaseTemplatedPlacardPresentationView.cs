namespace SmartSolucionesCuba.Viderhe.Extend.Common.Templating.Presentation.Views
{
    public class BaseTemplatedPlacardPresentationView : Common.Presentation.Views.BasePlacardPresentationView
    {
        public virtual void StartShowInWhenReady(Transitioning.Presentation.Transitions.Contract.AbstractTransition transition)
        {
            transition.PerformMotion();
        }

        public sealed override void OnShowInTransitionComplete()
        {
            PerformPlacardStarup();

            if (!(BindingContext is Models.View.AbstractScreenTimedPlacardViewModel) || (BindingContext as Models.View.AbstractScreenTimedPlacardViewModel).ScreenPresentationTime <= System.TimeSpan.Zero) return;

            Xamarin.Forms.Device.StartTimer((BindingContext as Models.View.AbstractScreenTimedPlacardViewModel).ScreenPresentationTime, () =>
            {
                if (BindingContext != null)
                {
                    EndPresentation();
                }

                return false;
            });
        }

        public virtual void PerformPlacardStarup() { }
    }
}
