namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.EF.Providers
{
    public sealed class BusinessDbSchemaConfigurationProvider : MOROPO.SAPRESSC.Core.Persistence.EF.Providers.Facade.IDbSchemaConfigurationProviderFacade
    {
        public string DefaultPersistenceSchema => null;

        public System.Collections.Generic.ICollection<MOROPO.SAPRESSC.Core.Persistence.EF.Configurators.IEntityTypeConfigurator> EntityTypeConfigurators => new System.Collections.Generic.List<MOROPO.SAPRESSC.Core.Persistence.EF.Configurators.IEntityTypeConfigurator>
        {
            new Configurators.AccountImageBridgeEntityTypeConfigurator(),
            new Configurators.AccountExtensionBridgeEntityTypeConfigurator(),
            new Configurators.AccountModeratorBridgeEntityTypeConfigurator(),
            new Configurators.AccountVideoBridgeEntityTypeConfigurator(),
            new Configurators.FrameEntityTypeConfigurator(),
            new Configurators.PlayerEntityTypeConfigurator(),
            new Configurators.ModeratorEntityTypeConfigurator(),
            new Configurators.ScheduledRelaunchEntityTypeConfigurator(),
            new Configurators.PlacardImageBridgeEntityTypeConfigurator(),
            new Configurators.PlacardVideoBridgeEntityTypeConfigurator()
        };
    }
}
