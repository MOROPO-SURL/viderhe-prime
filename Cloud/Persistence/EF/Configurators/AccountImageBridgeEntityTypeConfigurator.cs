namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.EF.Configurators
{
    public class AccountImageBridgeEntityTypeConfigurator : MOROPO.SAPRESSC.Core.Persistence.EF.Configurators.AbstractEntityTypeConfigurator<Entities.AccountImageBridge>
    {
        protected override void ApplayRules(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.AccountImageBridge> entityTypeBuilder)
        {
            base.ApplayRules(entityTypeBuilder);

            entityTypeBuilder.HasKey(e => new { e.AccountId, e.ImageId });

            entityTypeBuilder.HasOne(e => e.Account).WithMany(b => b.ImageBridges).HasForeignKey(e => e.AccountId);
            entityTypeBuilder.HasOne(e => e.Image).WithMany().HasForeignKey(e => e.ImageId);
        }
    }
}
