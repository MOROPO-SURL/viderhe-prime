using Microsoft.EntityFrameworkCore;
using MOROPO.SAPRESSC.Core.Security.Common.Persistence.EF.Extensions;
using MOROPO.SAPRESSC.Core.Security.Common.Persistence.Entities;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Cloud.Helpers
{
    public sealed class BusinessDataInitializationHelper : MOROPO.SAPRESSC.Core.Persistence.EF.Helpers.Facade.IDataInitializationHelper
    {
        public void InitialiseData(DbContext dbContext)
        {
            #region Account Permission definitions

            var roleDbSet = dbContext.Set<Permission>();

            if (!roleDbSet.Any(role => role.Code.Equals(Cloud.Security.Authorization.Permission.AccountViewer)))
            {
                roleDbSet.Add(Cloud.Security.Authorization.Permission.AccountViewer.ToPermissionEntity(Cloud.Security.Authorization.PermissionFamily.AccountManagement));
            }

            if (!roleDbSet.Any(role => role.Code.Equals(Cloud.Security.Authorization.Permission.AccountCreator)))
            {
                roleDbSet.Add(Cloud.Security.Authorization.Permission.AccountCreator.ToPermissionEntity(Cloud.Security.Authorization.PermissionFamily.AccountManagement));
            }

            if (!roleDbSet.Any(role => role.Code.Equals(Cloud.Security.Authorization.Permission.AccountEditor)))
            {
                roleDbSet.Add(Cloud.Security.Authorization.Permission.AccountEditor.ToPermissionEntity(Cloud.Security.Authorization.PermissionFamily.AccountManagement));
            }

            if (!roleDbSet.Any(role => role.Code.Equals(Cloud.Security.Authorization.Permission.AccountRemover)))
            {
                roleDbSet.Add(Cloud.Security.Authorization.Permission.AccountRemover.ToPermissionEntity(Cloud.Security.Authorization.PermissionFamily.AccountManagement));
            }

            #endregion

            #region Extension definitions

            var extensionDbSet = dbContext.Set<Persistence.Entities.Extension>();

            var extensionDescriptors = Support.Extend.Providers.ExtensionDescriptorProvider.Instance.GetDescriptors();

            var templateDescriptors = Support.Extend.Templating.Providers.TemplateDescriptorProvider.Instance.GetDescriptors();
            var transitionDescriptors = Support.Extend.Transitioning.Providers.TransitionDescriptorProvider.Instance.GetDescriptors();
            var overlayDescriptors = Support.Extend.Overlaying.Providers.OverlayDescriptorProvider.Instance.GetDescriptors();

            foreach (var extensionDescriptor in extensionDescriptors)
            {
                var extensionVersion = extensionDescriptor.GetType().Assembly.GetName().Version.ToString();

                var extensionNamespace = extensionDescriptor.GetType().Assembly.FullName.Split(",")[0];

                var spectedExtensionTemplateDescriptors = templateDescriptors.Where(d => d.GetType().Assembly.FullName.StartsWith(extensionNamespace)).ToArray();
                var spectedExtensionTransitionDescriptors = transitionDescriptors.Where(d => d.GetType().Assembly.FullName.StartsWith(extensionNamespace)).ToArray();
                var spectedExtensionOverlayDescriptors = overlayDescriptors.Where(d => d.GetType().Assembly.FullName.StartsWith(extensionNamespace)).ToArray();

                var extension = extensionDbSet
                    .Include(e => e.Templates)
                    .Include(e => e.Transitions)
                    .Include(e => e.Overlays)
                    .FirstOrDefault(e => e.Id == extensionDescriptor.Id);

                if (extension == null)
                {
                    extension = new Persistence.Entities.Extension
                    {
                        Id = extensionDescriptor.Id,
                        Enabled = true,
                        Version = extensionVersion,
                        Templates = spectedExtensionTemplateDescriptors.Select(d => new Persistence.Entities.Template { Id = d.Id, Enabled = true }).ToList(),
                        Transitions = spectedExtensionTransitionDescriptors.Select(d => new Persistence.Entities.Transition { Id = d.Id, Enabled = true }).ToList(),
                        Overlays = spectedExtensionOverlayDescriptors.Select(d => new Persistence.Entities.Overlay { Id = d.Id, Enabled = true }).ToList()
                    };

                    extensionDbSet.Add(extension);
                }
                else
                {
                    var spectedExtensionTemplates = spectedExtensionTemplateDescriptors.Select(d => new Persistence.Entities.Template { Id = d.Id, Enabled = true }).ToArray();

                    var divergentExtensionTemplateDescriptors = extension.Templates.Except(spectedExtensionTemplates, Comparison.TemplateEntityCollectionEqualityComparisonHelper.Instance).ToList();

                    divergentExtensionTemplateDescriptors.AddRange(spectedExtensionTemplates.Except(extension.Templates, Comparison.TemplateEntityCollectionEqualityComparisonHelper.Instance));

                    var templateDbSet = dbContext.Set<Persistence.Entities.Template>();

                    foreach (var templateDescriptor in divergentExtensionTemplateDescriptors)
                    {
                        var persistedTemplate = extension.Templates.FirstOrDefault(e => e.Id == templateDescriptor.Id);

                        if (persistedTemplate != null)
                        {
                            extension.Templates.Remove(persistedTemplate);

                            templateDbSet.Remove(persistedTemplate);
                        }
                        else
                        {
                            extension.Templates.Add(new Persistence.Entities.Template { Id = templateDescriptor.Id, Enabled = true });
                        }
                    }

                    var spectedExtensionTransitions = spectedExtensionTransitionDescriptors.Select(d => new Persistence.Entities.Transition { Id = d.Id, Enabled = true }).ToArray();

                    var divergentExtensionTransitionDescriptors = extension.Transitions.Except(spectedExtensionTransitions, Comparison.TransitionEntityCollectionEqualityComparisonHelper.Instance).ToList();

                    divergentExtensionTransitionDescriptors.AddRange(spectedExtensionTransitions.Except(extension.Transitions, Comparison.TransitionEntityCollectionEqualityComparisonHelper.Instance));

                    var transitionDbSet = dbContext.Set<Persistence.Entities.Transition>();

                    foreach (var transitionDescriptor in divergentExtensionTransitionDescriptors)
                    {
                        var persistedTransition = extension.Transitions.FirstOrDefault(e => e.Id == transitionDescriptor.Id);

                        if (persistedTransition != null)
                        {
                            extension.Transitions.Remove(persistedTransition);

                            transitionDbSet.Remove(persistedTransition);
                        }
                        else
                        {
                            extension.Transitions.Add(new Persistence.Entities.Transition { Id = transitionDescriptor.Id, Enabled = true });
                        }
                    }

                    var spectedExtensionOverlays = spectedExtensionOverlayDescriptors.Select(d => new Persistence.Entities.Overlay { Id = d.Id, Enabled = true }).ToArray();

                    var divergentExtensionOverlayDescriptors = extension.Overlays.Except(spectedExtensionOverlays, Comparison.OverlayEntityCollectionEqualityComparisonHelper.Instance).ToList();

                    divergentExtensionOverlayDescriptors.AddRange(spectedExtensionOverlays.Except(extension.Overlays, Comparison.OverlayEntityCollectionEqualityComparisonHelper.Instance));

                    var overlayDbSet = dbContext.Set<Persistence.Entities.Overlay>();

                    foreach (var overlayDescriptor in divergentExtensionOverlayDescriptors)
                    {
                        var persistedOverlay = extension.Overlays.FirstOrDefault(e => e.Id == overlayDescriptor.Id);

                        if (persistedOverlay != null)
                        {
                            extension.Overlays.Remove(persistedOverlay);

                            overlayDbSet.Remove(persistedOverlay);
                        }
                        else
                        {
                            extension.Overlays.Add(new Persistence.Entities.Overlay { Id = overlayDescriptor.Id, Enabled = true });
                        }
                    }

                    if (divergentExtensionTemplateDescriptors.Count() > 0 || divergentExtensionTransitionDescriptors.Count() > 0 || divergentExtensionOverlayDescriptors.Count() > 0)
                    {
                        if (extension.Version != extensionVersion)
                        {
                            extension.Version = extensionVersion;
                        }

                        extensionDbSet.Update(extension);
                    }
                    else if (extension.Version != extensionVersion)
                    {
                        extension.Version = extensionVersion;
                    }
                }
            }

            #endregion

            if (dbContext is MOROPO.SAPRESSC.Core.Persistence.EF.Context.ITraceableDbContext)
            {
                (dbContext as MOROPO.SAPRESSC.Core.Persistence.EF.Context.ITraceableDbContext).SaveChanges("system");
            }
            else
            {
                dbContext.SaveChanges();
            }
        }
    }
}
