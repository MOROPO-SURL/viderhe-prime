namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.EF.Configurators
{
    public class AccountExtensionBridgeEntityTypeConfigurator : MOROPO.SAPRESSC.Core.Persistence.EF.Configurators.AbstractEntityTypeConfigurator<Entities.AccountExtensionBridge>
    {
        protected override void ApplayRules(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.AccountExtensionBridge> entityTypeBuilder)
        {
            base.ApplayRules(entityTypeBuilder);

            entityTypeBuilder.HasKey(e => new { e.AccountId, e.ExtensionId });

            entityTypeBuilder.HasOne(e => e.Account).WithMany(b => b.ExtensionBridges).HasForeignKey(e => e.AccountId);
            entityTypeBuilder.HasOne(e => e.Extension).WithMany().HasForeignKey(e => e.ExtensionId);
        }
    }
}
