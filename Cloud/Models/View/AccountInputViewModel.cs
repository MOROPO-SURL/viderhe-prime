using MOROPO.SAPRESSC.Core.Web.Common.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace SmartSolucionesCuba.Viderhe.Cloud.Models.View
{
    public class AccountInputViewModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Persistence.Entities.Account, System.Guid>
    {
        public AccountInputViewModel()
        {
            Id = System.Guid.NewGuid();
            PrimaryColor = "#5580b0";
            SecoundaryColor = "#eabf19";
        }

        public System.Guid Id { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public bool Enabled { get; set; }

        [Display(Name = "MaxPlayerCount")]
        public int MaxPlayerCount { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "PrimaryColor")]
        public string PrimaryColor { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "SecoundaryColor")]
        public string SecoundaryColor { get; set; }

        public System.Guid LogoImageId { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage"), FormFileContentTypes(ContentTypes = new string[] { "image/png", "image/jpeg" }, ErrorMessage = "PNG_JPEG_FileAllowedOnlyMessage.")]
        [Display(Name = "Logo")]
        public Microsoft.AspNetCore.Http.IFormFile LogoImageFile { get; set; }

        public System.Guid HorizontalSplashImageId { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage"), FormFileContentTypes(ContentTypes = new string[] { "image/png", "image/jpeg" }, ErrorMessage = "PNG_JPEG_FileAllowedOnlyMessage.")]
        [Display(Name = "Splash Horizontal")]
        public Microsoft.AspNetCore.Http.IFormFile HorizontalSplashImageFile { get; set; }

        public System.Guid VerticalSplashImageId { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage"), FormFileContentTypes(ContentTypes = new string[] { "image/png", "image/jpeg" }, ErrorMessage = "PNG_JPEG_FileAllowedOnlyMessage.")]
        [Display(Name = "Splash Vertical")]
        public Microsoft.AspNetCore.Http.IFormFile VerticalSplashImageFile { get; set; }

        public Persistence.Entities.Account Export()
        {
            var entity = new Persistence.Entities.Account();

            Mergue(entity);

            return entity;
        }

        public void Import(Persistence.Entities.Account entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Enabled = entity.Enabled;
            MaxPlayerCount = entity.MaxPlayerCount;
            PrimaryColor = entity.PrimaryColor;
            SecoundaryColor = entity.SecoundaryColor;
            LogoImageId = entity.Logo != null ? entity.Logo.Id : System.Guid.Empty;
            HorizontalSplashImageId = entity.HorizontalSplashId;
            VerticalSplashImageId = entity.VerticalSplashId;
        }

        public void Mergue(Persistence.Entities.Account entity)
        {
            entity.Name = Name;
            entity.Enabled = Enabled;
            entity.MaxPlayerCount = MaxPlayerCount;
            entity.PrimaryColor = PrimaryColor;
            entity.SecoundaryColor = SecoundaryColor;

            entity.Signature = Helpers.Integrity.AccountIntegrityHelper.GetSignature(entity);
        }
    }
}
