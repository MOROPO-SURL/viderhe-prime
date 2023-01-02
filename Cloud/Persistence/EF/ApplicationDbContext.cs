using Microsoft.EntityFrameworkCore;
using MOROPO.SAPRESSC.Core.Security.Common.Persistence.Entities;
using MOROPO.SAPRESSC.Middleware.Identity.Persistence.Entities;
using MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities;
using MOROPO.SAPRESSC.Middleware.Region.Persistence.Entities;
using MOROPO.SAPRESSC.Middleware.Security.Essential.Persistence.Entities;
using SmartSolucionesCuba.Viderhe.Cloud.Persistence.Entities;

namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.EF
{
    public class ApplicationDbContext :
        MOROPO.SAPRESSC.Core.Persistence.EF.Context.AbstractTraceableDbContext,
        MOROPO.SAPRESSC.Core.Security.Common.Persistence.EF.Contract.IModulePersistenceDbContext,
        MOROPO.SAPRESSC.Middleware.Security.Essential.Persistence.EF.Contract.IModulePersistenceDbContext,
        MOROPO.SAPRESSC.Middleware.Identity.Persistence.EF.Context.IIdentityMiddlewarePersistenceDbContext
    {
        public ApplicationDbContext(System.Collections.Generic.IEnumerable<MOROPO.SAPRESSC.Core.Persistence.EF.Providers.Facade.IDbSchemaConfigurationProviderFacade> dbSchemaConfigurationProviders, DbContextOptions options) : base(dbSchemaConfigurationProviders, options)
        {
        }

#if DEBUG
        public override bool TraceChanges => false;
#endif

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermissionBridge> RolePermissionBridges { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRoleBridge> UserRoleBridges { get; set; }

        public DbSet<Thumbnail> Thumbnails { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Video> Videos { get; set; }

        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<Locality> Localities { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<CountryRegion> CountryRegions { get; set; }
        public DbSet<StateRegion> StateRegions { get; set; }
        public DbSet<MunicipalityRegion> MunicipalityRegions { get; set; }
        public DbSet<LocalityRegion> LocalityRegions { get; set; }

        public DbSet<PersonIdentity> PersonIdentities { get; set; }
        public DbSet<OrganizationPostalContactItemBridge> OrganizationPostalContactItemBridges { get; set; }
        public DbSet<OrganizationPhoneContactItemBridge> OrganizationPhoneContactItemBridges { get; set; }
        public DbSet<OrganizationEmailContactItemBridge> OrganizationEmailContactItemBridges { get; set; }
        public DbSet<PersonWebSiteContactItemBridge> PersonWebSiteContactItemBridges { get; set; }
        public DbSet<PersonSocialContactItemBridge> PersonSocialContactItemBridges { get; set; }
        public DbSet<PersonPostalContactItemBridge> PersonPostalContactItemBridges { get; set; }
        public DbSet<PersonPhoneContactItemBridge> PersonPhoneContactItemBridges { get; set; }
        public DbSet<PersonEmailContactItemBridge> PersonEmailContactItemBridges { get; set; }
        public DbSet<PostalContactItem> PostalContactItems { get; set; }
        public DbSet<WebSiteContactItem> WebSiteContactItems { get; set; }
        public DbSet<SocialContactItem> SocialContactItems { get; set; }
        public DbSet<PhoneContactItem> PhoneContactItems { get; set; }
        public DbSet<EmailContactItem> EmailContactItems { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<OrganizationIdentity> OrganizationIdentities { get; set; }
        public DbSet<OrganizationSocialContactItemBridge> OrganizationSocialContactItemBridges { get; set; }
        public DbSet<OrganizationWebSiteContactItemBridge> OrganizationWebSiteContactItemBridges { get; set; }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Screenplay> ScreenPlays { get; set; }
        public DbSet<Frame> Frames { get; set; }
        public DbSet<Placard> Placards { get; set; }
        public DbSet<PlacardImageBridge> PlacardImageBridges { get; set; }
        public DbSet<PlacardVideoBridge> PlacardVideoBridges { get; set; }
        public DbSet<Moderator> Moderators { get; set; }
        public DbSet<Transition> Transitions { get; set; }
        public DbSet<Overlay> Overlays { get; set; }
        public DbSet<Extension> Extensions { get; set; }
        public DbSet<AccountModeratorBridge> AccountModeratorBridges { get; set; }
        public DbSet<AccountImageBridge> AccountImageBridges { get; set; }
        public DbSet<AccountVideoBridge> AccountVideoBridges { get; set; }
        public DbSet<AccountExtensionBridge> AccountExtensionBridges { get; set; }
        public DbSet<ScheduledRelaunch> ScheduledRelaunches { get; set; }
    }
}
