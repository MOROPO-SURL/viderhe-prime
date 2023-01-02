namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class ImageInputModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityInputModel<MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Image, System.Guid>
    {
        public System.Guid Id { get; set; }

        public byte[] ContentData { get; set; }

        public string ContentType { get; set; }

        public string About { get; set; }

        public MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Image Export()
        {
            throw new System.NotImplementedException();
        }

        public void Import(MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Image entity)
        {
            About = entity.About;
        }

        public void Mergue(MOROPO.SAPRESSC.Middleware.Media.Persistence.Entities.Image entity)
        {
            entity.About = About;
        }
    }
}
