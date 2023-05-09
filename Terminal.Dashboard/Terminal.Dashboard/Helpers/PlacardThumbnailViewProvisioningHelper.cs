using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Terminal.Dashboard.Helpers
{
    public class PlacardThumbnailViewProvisioningHelper : Support.Presentation.Dashboard.Helpers.Facade.IPlacardThumbnailViewProvisioningHelperFacade
    {
        public PlacardThumbnailViewProvisioningHelper()
        {
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.FullName.Contains(".Viderhe.Support.Extend") && System.Text.RegularExpressions.Regex.IsMatch(a.FullName, ".+\\.Viderhe\\.Extend\\.[a-zA-Z0-9|\\.]+\\.Templating.Dashboard.Presentation,\\sVersion=")).ToArray();

            XFToolKit.IoC.TinyIoCContainer.Current.AutoRegister(assemblies, XFToolKit.IoC.DuplicateImplementationActions.RegisterMultiple, t => typeof(Extend.Common.Templating.Dashboard.Presentation.Providers.Facade.IPlacardThumbnailViewProviderFacade).IsAssignableFrom(t));
        }

        public Xamarin.Forms.ContentView GetPlacardThumnailView(Support.Presentation.Models.View.PlacardViewModel model)
        {
            var providers = XFToolKit.IoC.TinyIoCContainer.Current.ResolveAll<Extend.Common.Templating.Dashboard.Presentation.Providers.Facade.IPlacardThumbnailViewProviderFacade>();

            foreach (var provider in providers)
            {
                var result = provider.GetPlacardThumnailView(model);

                if (result == null) continue;

                return result;
            }

            throw new System.InvalidOperationException();
        }
    }
}
