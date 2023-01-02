using System.ComponentModel.DataAnnotations;

namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class PlayerInputModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Persistence.Entities.Player, string>
    {
        [Required, RegularExpression(@"^[0-9A-Z]{7}$")]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public System.Guid ScreenplayId { get; set; }

        public System.Guid? OverlayId { get; set; }

        public string SerializedOverlayContext { get; set; }

        public string About { get; set; }

        public Persistence.Entities.Player Export()
        {
            var entity = new Persistence.Entities.Player
            {
                Id = Id
            };

            Mergue(entity);

            return entity;
        }

        public void Import(Persistence.Entities.Player entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            About = entity.About;
            ScreenplayId = entity.ScreenplayId ?? System.Guid.Empty;
            OverlayId = entity.OverlayId;
            SerializedOverlayContext = entity.SerializedOverlayContext;
        }

        public void Mergue(Persistence.Entities.Player entity)
        {
            entity.Name = Name;
            entity.About = About;
            entity.OverlayId = OverlayId;
            entity.SerializedOverlayContext = SerializedOverlayContext;
        }
    }
}
