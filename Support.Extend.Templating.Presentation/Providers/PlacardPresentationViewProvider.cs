using SmartSolucionesCuba.Viderhe.Extend.Common.Presentation.Views;
using SmartSolucionesCuba.Viderhe.Extend.Common.Templating.Presentation.Providers.Facade;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Support.Extend.Templating.Presentation.Providers
{
    public sealed class PlacardPresentationViewProvider
    {
        private static PlacardPresentationViewProvider instance;

        public static PlacardPresentationViewProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlacardPresentationViewProvider();
                }

                return instance;
            }
        }

        private PlacardPresentationViewProvider()
        {
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies().Where(a => System.Text.RegularExpressions.Regex.IsMatch(a.FullName, ".+\\.Viderhe\\.Extend\\.[a-zA-Z0-9|\\.]+\\.Templating.Presentation,\\sVersion=")).ToArray();

            XFToolKit.IoC.TinyIoCContainer.Current.AutoRegister(assemblies, XFToolKit.IoC.DuplicateImplementationActions.RegisterMultiple, t => typeof(IPlacardPresentationViewProviderFacade).IsAssignableFrom(t));
        }

        public BasePlacardPresentationView GetFor(Support.Presentation.Models.View.PlacardViewModel model, Viderhe.Extend.Common.Presentation.Providers.Facade.IAccountInformationProviderFacade accountInformationProvider, Support.Presentation.Providers.Facade.ICloudServerProviderFacade cloudServerProvider)
        {
            var providers = XFToolKit.IoC.TinyIoCContainer.Current.ResolveAll<IPlacardPresentationViewProviderFacade>();

            foreach (var provider in providers)
            {
                var result = provider.GetFor(model, accountInformationProvider, cloudServerProvider);

                if (result == null) continue;

                return result;
            }

            throw new System.InvalidOperationException();
        }
    }
}
