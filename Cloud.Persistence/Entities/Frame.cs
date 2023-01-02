using System.ComponentModel.DataAnnotations;

namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.Entities
{
    public class Frame : MOROPO.SAPRESSC.Core.Persistence.Entities.IEntity<long>
    {
        [Key]
        public long Id { get; set; }

        public System.Guid ScreenplayId { get; set; }

        public Screenplay Screenplay { get; set; }

        public System.Guid PlacardId { get; set; }

        public Placard Placard { get; set; }

        public int Priority { get; set; }
    }
}
