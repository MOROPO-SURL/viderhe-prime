namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.EF.Configurators
{
    public class ModeratorEntityTypeConfigurator : MOROPO.SAPRESSC.Core.Persistence.EF.Configurators.AbstractEntityTypeConfigurator<Entities.Moderator>
    {
        protected override void ApplayRules(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Entities.Moderator> entityTypeBuilder)
        {
            base.ApplayRules(entityTypeBuilder);

            entityTypeBuilder.HasOne(e => e.Identity).WithMany().HasForeignKey(e => e.IdentityId);
            entityTypeBuilder.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
        }
    }
}
