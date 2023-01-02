using MOROPO.SAPRESSC.Core.Persistence.Entities;
using System.ComponentModel.DataAnnotations;

namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.Entities
{
    public class Player : INamedEntity<string>, IPersistenceSignable
    {
        public Player()
        {
            ScheduledRelaunches = new System.Collections.Generic.List<ScheduledRelaunch>();
        }

        [Key, RegularExpression(@"^[0-9A-Z]{7}$")]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public System.Guid AccountId { get; set; }

        public Account Account { get; set; }

        public System.Guid? ScreenplayId { get; set; }

        public Screenplay Screenplay { get; set; }

        public System.Guid? OverlayId { get; set; }

        public Overlay Overlay { get; set; }

        public string SerializedOverlayContext { get; set; }

        public string About { get; set; }

        [Required]
        public string Signature { get; set; }

        public System.Collections.Generic.List<ScheduledRelaunch> ScheduledRelaunches { get; set; }

        public object GetSignableEntityModel()
        {
            return new { Id, Name, AccountId = Account.Id };
        }
    }
}
