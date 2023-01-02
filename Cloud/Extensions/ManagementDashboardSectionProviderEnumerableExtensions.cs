using MOROPO.SAPRESSC.Core.Web.Backend.Models.View;
using System.Collections.Generic;

namespace SmartSolucionesCuba.Viderhe.Cloud.Extensions
{
    public static class ManagementDashboardSectionProviderEnumerableExtensions
    {
        public static Dictionary<string, ICollection<ManagementDashboardSectionViewModel>> ToManagementDashboardSectionGroups(this IEnumerable<MOROPO.SAPRESSC.Core.Web.Backend.Providers.Facade.IManagementDashboardSectionProviderFacade> self)
        {
            var securitySections = new List<ManagementDashboardSectionViewModel>();
            var serviceSections = new List<ManagementDashboardSectionViewModel>();
            var cloudSections = new List<ManagementDashboardSectionViewModel>();

            foreach (var provider in self)
            {
                if (provider is MOROPO.SAPRESSC.Middleware.Security.Essential.Web.Backend.Providers.ManagementDashboardSectionProvider)
                {
                    securitySections.AddRange(provider.GetManagementDashboardSections());
                }
            }

            securitySections.Add(new ManagementDashboardSectionViewModel
            {
                Title = "Registros",
                Controller = "traces",
                Action = "index",
                Area = "business",
                Role = Security.Authorization.Permission.TraceViewer
            });

            serviceSections.Add(new ManagementDashboardSectionViewModel
            {
                Title = "Cuentas",
                Controller = "accounts",
                Action = "index",
                Area = "business",
                Role = Security.Authorization.Permission.AccountViewer
            });

            serviceSections.Add(new ManagementDashboardSectionViewModel
            {
                Title = "Platillas",
                Controller = "templates",
                Action = "index",
                Area = "business",
                Role = Security.Authorization.Permission.TraceViewer
            });

            serviceSections.Add(new ManagementDashboardSectionViewModel
            {
                Title = "Transiciones",
                Controller = "transitions",
                Action = "index",
                Area = "business",
                Role = Security.Authorization.Permission.TraceViewer
            });

            cloudSections.Add(new ManagementDashboardSectionViewModel
            {
                Title = "Reproductores",
                Controller = "players",
                Action = "index",
                Area = "business",
                Role = Security.Authorization.Permission.TraceViewer
            });

            cloudSections.Add(new ManagementDashboardSectionViewModel
            {
                Title = "Imágenes",
                Controller = "images",
                Action = "index",
                Area = "media",
                Role = Security.Authorization.Permission.TraceViewer
            });

            cloudSections.Add(new ManagementDashboardSectionViewModel
            {
                Title = "Videos",
                Controller = "videos",
                Action = "index",
                Area = "media",
                Role = Security.Authorization.Permission.TraceViewer
            });

            return new Dictionary<string, ICollection<ManagementDashboardSectionViewModel>>
            {
                { "Seguridad", securitySections },
                { "Servicio", serviceSections },
                { "Nube", cloudSections }
            };
        }
    }
}
