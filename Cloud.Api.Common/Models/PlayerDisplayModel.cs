using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class PlayerDisplayModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.Player, string>
    {
        public PlayerDisplayModel()
        {
            IsConnected = false;
            AudioEnabled = false;
            AudioLevel = -1;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string About { get; set; }

        public ScreenplayDisplayModel Screenplay { get; set; }

        public System.Guid? OverlayId { get; set; }

        public string SerializedOverlayContext { get; set; }

        public bool IsConnected { get; set; }

        public int AudioLevel { get; set; }

        public bool AudioEnabled { get; set; }

        public void Import(Persistence.Entities.Player entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            About = entity.About;
            OverlayId = entity.OverlayId;
            SerializedOverlayContext = entity.SerializedOverlayContext;

            if (entity.Screenplay == null) return;

            Screenplay = new ScreenplayDisplayModel();

            Screenplay.Import(entity.Screenplay);
        }
    }
}
