using SmartSolucionesCuba.Viderhe.Extend.Common.Presentation.Views;

namespace SmartSolucionesCuba.Viderhe.Extend.Essential.Transitioning.Presentation.Providers
{
    public class TransitionProvider : Common.Transitioning.Presentation.Providers.Facade.ITransitionProviderFacade
    {
        public Common.Transitioning.Presentation.Transitions.Contract.AbstractTransition GetFor(System.Guid transitionDescriptorId, Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider, BasePlacardPresentationView incomingPlacardPresentationView, BasePlacardPresentationView outgoingPlacardPresentationView, System.Action<BasePlacardPresentationView> motionCompletedActionCallback)
        {
            // TODO Complete method implementation!!
            return null;
        }

        public Common.Transitioning.Presentation.Transitions.Contract.AbstractTransition GetRandom(Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider, BasePlacardPresentationView incomingPlacardPresentationView, BasePlacardPresentationView outgoingPlacardPresentationView, System.Action<BasePlacardPresentationView> motionCompletedActionCallback)
        {
            var randomizer = new System.Random();

            var randomTransitionIndex = randomizer.Next(0, 6);

            switch (randomTransitionIndex)
            {
                case 0:
                    return Factories.RandomTransitionFactory.CreateFadeInTransition(incomingPlacardPresentationView, outgoingPlacardPresentationView, motionCompletedActionCallback);

                case 1:
                case 2:
                    return Factories.RandomTransitionFactory.CreateArriveTransition(incomingPlacardPresentationView, outgoingPlacardPresentationView, motionCompletedActionCallback);

                case 3:
                case 4:
                    return Factories.RandomTransitionFactory.CreateCarouselTransition(incomingPlacardPresentationView, outgoingPlacardPresentationView, motionCompletedActionCallback);

                case 5:
                default:
                    return Factories.RandomTransitionFactory.CreateZoomInTransition(incomingPlacardPresentationView, outgoingPlacardPresentationView, motionCompletedActionCallback);
            }

            throw new System.InvalidOperationException();
        }
    }
}
