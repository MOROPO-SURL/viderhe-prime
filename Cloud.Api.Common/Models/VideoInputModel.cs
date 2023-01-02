namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class VideoInputModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityInputModel<MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Video, System.Guid>
    {
        public System.Guid Id { get; set; }

        public string About { get; set; }

        public MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Video Export()
        {
            throw new System.NotImplementedException();
        }

        public void Import(MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Video entity)
        {
            About = entity.About;
        }

        public void Mergue(MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Video entity)
        {
            entity.About = About;
        }
    }
}
