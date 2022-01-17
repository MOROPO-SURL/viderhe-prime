using SmartSolucionesCuba.Viderhe.Extend.Common.Transitioning.Presentation.Providers.Facade;
using SmartSolucionesCuba.Viderhe.Extend.Common.Transitioning.Presentation.Transitions.Contract;
using SmartSolucionesCuba.Viderhe.Extend.Common.Presentation.Providers.Facade;
using SmartSolucionesCuba.Viderhe.Extend.Common.Presentation.Views;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Support.Extend.Transitioning.Presentation.Providers
{
    public class TransitionProvider : ITransitionProviderFacade
    {
        private static TransitionProvider instance;

        public static TransitionProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TransitionProvider();
                }

                return instance;
            }
        }

        private TransitionProvider()
        {
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.FullName.Contains(".Viderhe.Support.Extend") && System.Text.RegularExpressions.Regex.IsMatch(a.FullName, ".+\\.Viderhe\\.Extend\\.[a-zA-Z0-9|\\.]+\\.Transitioning.Presentation,\\sVersion=")).ToArray();

            XFToolKit.IoC.TinyIoCContainer.Current.AutoRegister(assemblies, XFToolKit.IoC.DuplicateImplementationActions.RegisterMultiple, t => typeof(ITransitionProviderFacade).IsAssignableFrom(t));
        }

        public AbstractTransition GetFor(System.Guid transitionDescriptorId, IAccountInformationProviderFacade accountInformationProvider, BasePlacardPresentationView incomingPlacardPresentationView, BasePlacardPresentationView outgoingPlacardPresentationView, System.Action<BasePlacardPresentationView> motionCompletedActionCallback)
        {
            var providers = XFToolKit.IoC.TinyIoCContainer.Current.ResolveAll<ITransitionProviderFacade>();

            foreach (var provider in providers)
            {
                var result = provider.GetFor(transitionDescriptorId, accountInformationProvider, incomingPlacardPresentationView, outgoingPlacardPresentationView, motionCompletedActionCallback);

                if (result == null) continue;

                return result;
            }

            throw new System.InvalidOperationException();
        }

        public AbstractTransition GetRandom(IAccountInformationProviderFacade accountInformationProvider, BasePlacardPresentationView incomingPlacardPresentationView, BasePlacardPresentationView outgoingPlacardPresentationView, System.Action<BasePlacardPresentationView> motionCompletedActionCallback)
        {
            var providers = XFToolKit.IoC.TinyIoCContainer.Current.ResolveAll<ITransitionProviderFacade>();

            var randomizer = new System.Random();

            var randomProviderIndex = randomizer.Next(0, providers.Count());

            var provider = providers.ElementAt(randomProviderIndex);

            return provider.GetRandom(accountInformationProvider, incomingPlacardPresentationView, outgoingPlacardPresentationView, motionCompletedActionCallback);
        }
    }
}
