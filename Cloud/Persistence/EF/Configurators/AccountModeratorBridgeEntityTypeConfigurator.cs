namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.EF.Configurators
{
    public class AccountModeratorBridgeEntityTypeConfigurator : MOROPO.SAPRESSC.Core.Persistence.EF.Configurators.AbstractEntityTypeConfigurator<Entities.AccountModeratorBridge>
    {
        protected override void ApplayRules(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.AccountModeratorBridge> entityTypeBuilder)
        {
            base.ApplayRules(entityTypeBuilder);

            entityTypeBuilder.HasKey(bridge => new { bridge.AccountId, bridge.ModeratorId });

            entityTypeBuilder.HasOne(bridge => bridge.Account).WithMany(entity => entity.ModeratorBridges).HasForeignKey(bridge => bridge.AccountId);
            entityTypeBuilder.HasOne(bridge => bridge.Moderator).WithMany(entit => entit.AccountBridges).HasForeignKey(bridge => bridge.ModeratorId);
        }
    }
}
