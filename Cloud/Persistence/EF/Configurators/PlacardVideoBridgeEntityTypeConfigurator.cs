namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.EF.Configurators
{
    public class PlacardVideoBridgeEntityTypeConfigurator : MOROPO.SAPRESSC.Core.Persistence.EF.Configurators.AbstractEntityTypeConfigurator<Entities.PlacardVideoBridge>
    {
        protected override void ApplayRules(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.PlacardVideoBridge> entityTypeBuilder)
        {
            base.ApplayRules(entityTypeBuilder);

            entityTypeBuilder.HasKey(b => new { b.PlacardId, b.VideoId });

            entityTypeBuilder.HasOne(b => b.Placard).WithMany(e => e.VideoBridges).HasForeignKey(b => b.PlacardId).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);
            entityTypeBuilder.HasOne(b => b.Video).WithMany().HasForeignKey(b => b.VideoId).OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);
        }
    }
}
