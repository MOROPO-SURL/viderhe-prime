namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class FrameDisplayModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.Frame, long>
    {
        public long Id { get; set; }

        public PlacardDisplayModel Placard { get; set; }

        public int Priority { get; set; }

        public void Import(Persistence.Entities.Frame entity)
        {
            Id = entity.Id;
            Priority = entity.Priority;

            Placard = new PlacardDisplayModel();

            Placard.Import(entity.Placard);
        }
    }
}
