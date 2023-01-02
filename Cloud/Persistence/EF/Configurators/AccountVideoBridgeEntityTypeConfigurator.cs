namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.EF.Configurators
{
    public class AccountVideoBridgeEntityTypeConfigurator : MOROPO.SAPRESSC.Core.Persistence.EF.Configurators.AbstractEntityTypeConfigurator<Entities.AccountVideoBridge>
    {
        protected override void ApplayRules(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.AccountVideoBridge> entityTypeBuilder)
        {
            base.ApplayRules(entityTypeBuilder);

            entityTypeBuilder.HasKey(b => new { b.AccountId, b.VideoId });

            entityTypeBuilder.HasOne(b => b.Account).WithMany(e => e.VideoBridges).HasForeignKey(b => b.AccountId);
            entityTypeBuilder.HasOne(b => b.Video).WithMany().HasForeignKey(b => b.VideoId);
        }
    }
}
