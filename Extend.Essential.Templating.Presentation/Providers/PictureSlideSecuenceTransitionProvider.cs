namespace SmartSolucionesCuba.Viderhe.Extend.Essential.Templating.Presentation.Providers
{
    public class PictureSlideSecuenceTransitionProvider : Common.Transitioning.Presentation.Providers.Facade.ITransitionProviderFacade
    {
        public Common.Transitioning.Presentation.Transitions.Contract.AbstractTransition GetFor(System.Guid transitionDescriptorId, Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider, Common.Presentation.Views.BasePlacardPresentationView incomingPlacardPresentationView, Common.Presentation.Views.BasePlacardPresentationView outgoingPlacardPresentationView, System.Action<Common.Presentation.Views.BasePlacardPresentationView> motionCompletedActionCallback)
        {
            if (transitionDescriptorId == Transitioning.Transitions.ScalingGrowFadeOutInTransitionDescriptor.ID)
            {
                return new Transitioning.Presentation.Transitions.ScalingGrowFadeOutInTransition(incomingPlacardPresentationView, outgoingPlacardPresentationView, motionCompletedActionCallback);
            }

            // TODO Complete method implementation!!
            return null;
        }

        public Common.Transitioning.Presentation.Transitions.Contract.AbstractTransition GetRandom(Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider, Common.Presentation.Views.BasePlacardPresentationView incomingPlacardPresentationView, Common.Presentation.Views.BasePlacardPresentationView outgoingPlacardPresentationView, System.Action<Common.Presentation.Views.BasePlacardPresentationView> motionCompletedActionCallback)
        {
            return new Transitioning.Presentation.Transitions.ScalingGrowFadeOutInTransition(incomingPlacardPresentationView, outgoingPlacardPresentationView, motionCompletedActionCallback);
        }
    }
}
