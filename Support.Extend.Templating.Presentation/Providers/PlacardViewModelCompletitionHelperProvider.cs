using SmartSolucionesCuba.Viderhe.Extend.Common.Templating.Presentation.Helpers.Facade;
using SmartSolucionesCuba.Viderhe.Extend.Common.Templating.Presentation.Providers.Facade;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Support.Extend.Templating.Presentation.Providers
{
    public sealed class PlacardViewModelCompletitionHelperProvider
    {
        private static PlacardViewModelCompletitionHelperProvider instance;

        public static PlacardViewModelCompletitionHelperProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlacardViewModelCompletitionHelperProvider();
                }

                return instance;
            }
        }

        private PlacardViewModelCompletitionHelperProvider()
        {
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies().Where(a => System.Text.RegularExpressions.Regex.IsMatch(a.FullName, ".+\\.Viderhe\\.Extend\\.[a-zA-Z0-9|\\.]+\\.Templating.Presentation,\\sVersion=")).ToArray();

            XFToolKit.IoC.TinyIoCContainer.Current.AutoRegister(assemblies, XFToolKit.IoC.DuplicateImplementationActions.RegisterMultiple, t => typeof(IPlacardViewModelCompletitionHelperProviderFacade).IsAssignableFrom(t));
        }

        public IPlacardViewModelCompletitionHelperFacade GetFor(Cloud.Api.Common.Models.PlacardDisplayModel model)
        {
            var providers = XFToolKit.IoC.TinyIoCContainer.Current.ResolveAll<IPlacardViewModelCompletitionHelperProviderFacade>();

            foreach (var provider in providers)
            {
                var result = provider.GetFor(model);

                if (result == null) continue;

                return result;
            }

            return null;
        }
    }
}
