namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.EF.Configurators
{
    public class PlayerEntityTypeConfigurator : MOROPO.SAPRESSC.Core.Persistence.EF.Configurators.AbstractEntityTypeConfigurator<Entities.Player>
    {
        protected override void ApplayRules(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.Player> entityTypeBuilder)
        {
            base.ApplayRules(entityTypeBuilder);

            entityTypeBuilder.HasOne(entity => entity.Screenplay).WithMany().HasForeignKey(entity => entity.ScreenplayId).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);
            entityTypeBuilder.HasOne(entity => entity.Account).WithMany(entity => entity.Players).HasForeignKey(entity => entity.AccountId);
            entityTypeBuilder.HasOne(entity => entity.Overlay).WithMany().HasForeignKey(entity => entity.OverlayId);
        }
    }
}
