using SmartSolucionesCuba.Viderhe.Extend.Common.Presentation.Views;

namespace SmartSolucionesCuba.Viderhe.Extend.SpringMotion.Transitioning.Presentation.Providers
{
    public class TransitionProvider : Common.Transitioning.Presentation.Providers.Facade.ITransitionProviderFacade
    {
        public Common.Transitioning.Presentation.Transitions.AbstractTransition GetFor(System.Guid transitionDescriptorId, Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider, BasePlacardPresentationView incomingPlacardPresentationView, BasePlacardPresentationView outgoingPlacardPresentationView, System.Action<BasePlacardPresentationView> motionCompletedActionCallback)
        {
            // TODO Complete method implementation!!
            return null;
        }

        public Common.Transitioning.Presentation.Transitions.AbstractTransition GetRandom(Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider, BasePlacardPresentationView incomingPlacardPresentationView, BasePlacardPresentationView outgoingPlacardPresentationView, System.Action<BasePlacardPresentationView> motionCompletedActionCallback)
        {
            var randomizer = new System.Random();

            var randomTransitionIndex = randomizer.Next(0, 7);

            switch (randomTransitionIndex)
            {
                case 0:
                case 1:
                    return Factories.RandomTransitionFactory.CreateFlatSpectrumHandFanTransition(incomingPlacardPresentationView, outgoingPlacardPresentationView, motionCompletedActionCallback);

                case 2:
                    var colors = new Xamarin.Forms.Color[]
                    {
                        Xamarin.Forms.Color.Accent,
                        Xamarin.Forms.Color.Black,
                        Xamarin.Forms.Color.White,
                        accountInformationProvider.PrimaryColor,
                        accountInformationProvider.SecoundaryColor
                    };

                    return Factories.RandomTransitionFactory.CreateFlatBoxMatrixSweptCurtainTransition(colors[randomizer.Next(0, colors.Length)], incomingPlacardPresentationView, outgoingPlacardPresentationView, motionCompletedActionCallback);

                case 3:
                case 4:
                    return Factories.RandomTransitionFactory.CreateFlatSpectrumInverseGothTransition(incomingPlacardPresentationView, outgoingPlacardPresentationView, motionCompletedActionCallback);

                case 5:
                case 6:
                default:
                    return Factories.RandomTransitionFactory.CreateFlatDualSpectrumHandFanCurtainTransition(incomingPlacardPresentationView, outgoingPlacardPresentationView, motionCompletedActionCallback);
            }
        }
    }
}
