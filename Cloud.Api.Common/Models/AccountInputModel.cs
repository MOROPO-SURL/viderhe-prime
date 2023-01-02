using System.ComponentModel.DataAnnotations;

namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class AccountInputModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Persistence.Entities.Account, System.Guid>
    {
        public System.Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PrimaryColor { get; set; }

        [Required]
        public string SecoundaryColor { get; set; }

        public byte[] LogoImageData { get; set; }

        public string LogoImageContentType { get; set; }

        public System.Guid LogoImageId { get; set; }

        public byte[] HorizontalSplashImageData { get; set; }

        public string HorizontalSplashImageContentType { get; set; }

        public System.Guid HorizontalSplashImageId { get; set; }

        public byte[] VerticalSplashImageData { get; set; }

        public string VerticalSplashImageContentType { get; set; }

        public System.Guid VerticalSplashImageId { get; set; }

        public System.Guid? DefaultOverlayId { get; set; }

        public string SerializedOverlayContext { get; set; }

        public Persistence.Entities.Account Export()
        {
            throw new System.NotImplementedException();
        }

        public void Import(Persistence.Entities.Account entity)
        {
            throw new System.NotImplementedException();
        }

        public void Mergue(Persistence.Entities.Account entity)
        {
            entity.Name = Name;
            entity.PrimaryColor = PrimaryColor;
            entity.SecoundaryColor = SecoundaryColor;
            entity.DefaultOverlayId = DefaultOverlayId;
            entity.SerializedOverlayContext = SerializedOverlayContext;
        }
    }
}
