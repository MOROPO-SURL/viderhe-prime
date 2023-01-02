using SmartSolucionesCuba.Viderhe.Extend.Common.Transitioning.Presentation.Providers.Facade;
using SmartSolucionesCuba.Viderhe.Extend.Common.Presentation.Views;
using SmartSolucionesCuba.Viderhe.Support.Presentation.Extensions;
using SmartSolucionesCuba.Viderhe.Support.Presentation.Models.View;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Extend.Common.Templating.Presentation.Presenters
{
    public class ScreenplayPresenter : Facade.IOvarlablePresenter, System.IDisposable
    {
        private System.Action presentationEndedCallbackAction;
        private System.Action screenplayPresentIterationCompleteCallbackAction;
        private System.Action<System.Guid> placardPlayingCallbackAction;
        private System.Action<BasePlacardPresentationView, BasePlacardPresentationView> placardViewTransitionCompleteCallbackAction;

        private System.Func<PlacardViewModel, BasePlacardPresentationView> placardViewModelToPresentationViewFunction;

        private System.Collections.Generic.Stack<PlacardViewModel> framesStack;

        private PlacardViewModel[] placards;

        private ITransitionProviderFacade transitionProvider;

        private BasePlacardPresentationView outgoingFrameView;

        private Xamarin.Forms.Layout<Xamarin.Forms.View> container;

        private ScreenplayPresenter interventionPresenter;

        private bool suspended;
        private bool loop;

        public ScreenplayPresenter(Xamarin.Forms.Layout<Xamarin.Forms.View> container, System.Func<PlacardViewModel, BasePlacardPresentationView> placardViewModelToPresentationViewFunction, PlacardViewModel[] placards, ITransitionProviderFacade transitionProvider, System.Action screenplayPresentationEndedCallbackAction, bool loop, System.Action<System.Guid> placardPlayingCallbackAction, System.Action screenplayPresentIterationCompleteCallbackAction = null, System.Action<BasePlacardPresentationView, BasePlacardPresentationView> placardViewTransitionCompleteCallbackAction = null)
        {
            suspended = false;

            this.container = container;
            this.placardViewModelToPresentationViewFunction = placardViewModelToPresentationViewFunction;
            this.placards = placards;
            this.transitionProvider = transitionProvider;
            this.presentationEndedCallbackAction = screenplayPresentationEndedCallbackAction;
            this.placardPlayingCallbackAction = placardPlayingCallbackAction;
            this.screenplayPresentIterationCompleteCallbackAction = screenplayPresentIterationCompleteCallbackAction;
            this.placardViewTransitionCompleteCallbackAction = placardViewTransitionCompleteCallbackAction;
            this.loop = loop;

            framesStack = new System.Collections.Generic.Stack<PlacardViewModel>(placards.Reverse());
        }

        public PlacardViewModel CurrentPlacardViewModel { get; set; }

        public void BeginPresentation()
        {
            RequestPresentNextFrame();
        }

        public void RequestPresentNextFrame()
        {
            PresentNextPlacardPresentationView();
        }

        public void RequestPresentPreviousFrame()
        {
            if (placards.Length - framesStack.Count <= 1) return;

            var targetIndex = placards.Length - framesStack.Count - 2;

            framesStack.Push(placards[targetIndex + 1]);
            framesStack.Push(placards[targetIndex]);

            PresentNextPlacardPresentationView();
        }

        public void PresentInterventionPlacards(PlacardViewModel[] placards)
        {
            SuspendPresentation();

            interventionPresenter = new ScreenplayPresenter(container, placardViewModelToPresentationViewFunction, placards, transitionProvider, InterventionPlacardsPresentationCompleted_Callback, false, null);

            interventionPresenter.BeginPresentation();
        }

        public void RequestRestart(Cloud.SignalR.Connect.Common.Enumerations.ScreenplayHandlingImmediacyMode immediacyMode)
        {
            switch (immediacyMode)
            {
                case Cloud.SignalR.Connect.Common.Enumerations.ScreenplayHandlingImmediacyMode.Immediately:
                    framesStack.Clear();

                    PresentNextPlacardPresentationView();

                    break;

                case Cloud.SignalR.Connect.Common.Enumerations.ScreenplayHandlingImmediacyMode.CurrentFramePresentationComplete:
                    framesStack.Clear();

                    break;

                default:
                    throw new System.InvalidOperationException("Modo de inmediatez no soportado para esta acción.");
            }
        }

        public void RequestReload(Cloud.SignalR.Connect.Common.Enumerations.ScreenplayHandlingImmediacyMode immediacyMode)
        {
            if (placards.Length <= 0)
            {
                immediacyMode = Cloud.SignalR.Connect.Common.Enumerations.ScreenplayHandlingImmediacyMode.Immediately;
            }

            switch (immediacyMode)
            {
                case Cloud.SignalR.Connect.Common.Enumerations.ScreenplayHandlingImmediacyMode.Immediately:
                    framesStack.Clear();
                    
                    loop = false;

                    presentationEndedCallbackAction?.Invoke();
                    presentationEndedCallbackAction = null;

                    break;

                case Cloud.SignalR.Connect.Common.Enumerations.ScreenplayHandlingImmediacyMode.CurrentFramePresentationComplete:
                    framesStack.Clear();
                    
                    loop = false;

                    break;

                case Cloud.SignalR.Connect.Common.Enumerations.ScreenplayHandlingImmediacyMode.FramesPresentationComplete:
                default:
                    loop = false;

                    break;
            }
        }

        private void PlacardPresentationView_PresentationComplete(object sender, System.EventArgs eventArgs)
        {
            PresentNextPlacardPresentationView();
        }

        private void MotionTransitionCompleted_Callback(BasePlacardPresentationView incomingFrameView)
        {
            if (outgoingFrameView != null)
            {
                container?.Children.Remove(outgoingFrameView);
            }

            incomingFrameView.OnShowInTransitionComplete();

            placardViewTransitionCompleteCallbackAction?.Invoke(incomingFrameView, outgoingFrameView);

            if (outgoingFrameView is System.IDisposable)
            {
                (outgoingFrameView as System.IDisposable).Dispose();
            }

            outgoingFrameView = null;

            if (suspended)
            {
                incomingFrameView.PresentationComplete -= PlacardPresentationView_PresentationComplete;

                container.Children.Remove(incomingFrameView);

                if (incomingFrameView is System.IDisposable)
                {
                    (incomingFrameView as System.IDisposable).Dispose();
                }
            }
        }

        private void InterventionPlacardsPresentationCompleted_Callback()
        {
            interventionPresenter.Dispose();

            interventionPresenter = null;

            ResumePresentation();
        }

        private void PresentNextPlacardPresentationView()
        {
            if (suspended) return;

            CurrentPlacardViewModel = null;

            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                if (framesStack != null && framesStack.Count > 0)
                {
                    var model = framesStack.Pop();

                    while (!model.SupportedByMainDisplayOrientation())
                    {
                        model = null;

                        if (framesStack.Count <= 0) break;

                        model = framesStack.Pop();
                    }

                    if (model != null)
                    {
                        var incomingFrameView = placardViewModelToPresentationViewFunction.Invoke(model);

                        incomingFrameView.PresentationComplete += PlacardPresentationView_PresentationComplete;

                        outgoingFrameView = container.Children.FirstOrDefault() as BasePlacardPresentationView;

                        if (outgoingFrameView != null)
                        {
                            outgoingFrameView.PresentationComplete -= PlacardPresentationView_PresentationComplete;
                        }

                        var transition = transitionProvider.GetRandom(incomingFrameView, outgoingFrameView, MotionTransitionCompleted_Callback);

                        transition?.GetReady(container);

                        container.Children.Add(incomingFrameView);

                        CurrentPlacardViewModel = model;

                        if (transition != null)
                        {
                            if (incomingFrameView is Views.BaseTemplatedPlacardPresentationView)
                            {
                                (incomingFrameView as Views.BaseTemplatedPlacardPresentationView).StartShowInWhenReady(transition);
                            }
                            else
                            {
                                transition.PerformMotion();
                            }
                        }
                        else
                        {
                            if (outgoingFrameView != null)
                            {
                                container.Children.Remove(outgoingFrameView);
                            }

                            incomingFrameView.OnShowInTransitionComplete();

                            placardViewTransitionCompleteCallbackAction?.Invoke(incomingFrameView, outgoingFrameView);

                            if (outgoingFrameView is System.IDisposable)
                            {
                                (outgoingFrameView as System.IDisposable).Dispose();
                            }

                            outgoingFrameView = null;

                            if (suspended)
                            {
                                incomingFrameView.PresentationComplete -= PlacardPresentationView_PresentationComplete;

                                container.Children.Remove(incomingFrameView);

                                if (incomingFrameView is System.IDisposable)
                                {
                                    (incomingFrameView as System.IDisposable).Dispose();
                                }
                            }
                        }

                        placardPlayingCallbackAction?.Invoke(model.Id);

                        return;
                    }
                }

                if (!loop)
                {
                    presentationEndedCallbackAction?.Invoke();
                    presentationEndedCallbackAction = null;
                }
                else
                {
                    screenplayPresentIterationCompleteCallbackAction?.Invoke();

                    framesStack = new System.Collections.Generic.Stack<PlacardViewModel>(placards.Reverse());

                    PresentNextPlacardPresentationView();
                }
            });
        }

        #region IOverlablePresenter implementation

        public void SuspendPresentation()
        {
            suspended = true;

            if (!(container.Children.FirstOrDefault() is BasePlacardPresentationView currentPlacardFrameView)) return;

            currentPlacardFrameView.PresentationComplete -= PlacardPresentationView_PresentationComplete;

            container.Children.Remove(currentPlacardFrameView);

            if (currentPlacardFrameView is System.IDisposable)
            {
                (currentPlacardFrameView as System.IDisposable).Dispose();
            }
        }

        public void ResumePresentation()
        {
            suspended = false;

            RequestPresentNextFrame();
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar llamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: elimine el estado administrado (objetos administrados).
                }

                // TODO: libere los recursos no administrados (objetos no administrados) y reemplace el siguiente finalizador.
                // TODO: configure los campos grandes en nulos.

                framesStack.Clear();

                placards = null;
                framesStack = null;
                presentationEndedCallbackAction = null;
                placardPlayingCallbackAction = null;
                screenplayPresentIterationCompleteCallbackAction = null;
                placardViewTransitionCompleteCallbackAction = null;
                placardViewModelToPresentationViewFunction = null;
                transitionProvider = null;
                container = null;

                disposedValue = true;
            }
        }

        // TODO: reemplace un finalizador solo si el anterior Dispose(bool disposing) tiene código para liberar los recursos no administrados.
        // ~ScreenplayPresenter() {
        //   // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
        //   Dispose(false);
        // }

        // Este código se agrega para implementar correctamente el patrón descartable.
        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
            Dispose(true);
            // TODO: quite la marca de comentario de la siguiente línea si el finalizador se ha reemplazado antes.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
