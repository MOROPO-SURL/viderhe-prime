using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.Entities
{
    public class Screenplay : MOROPO.SAPRESSC.Core.Persistence.Entities.IEntity<System.Guid>
    {
        [Key]
        public System.Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string About { get; set; }

        public System.Guid AccountId { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("AccountId")]
        public Account Account { get; set; }

        public ICollection<Frame> Frames { get; set; }

        public Screenplay()
        {
            Id = System.Guid.NewGuid();

            Frames = new List<Frame>();
        }
    }
}
