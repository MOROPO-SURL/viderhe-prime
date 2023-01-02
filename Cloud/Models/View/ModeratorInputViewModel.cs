using MOROPO.SAPRESSC.Core.Web.Common.ComponentModel.DataAnnotations;
using MOROPO.SAPRESSC.Middleware.Media.Web.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SmartSolucionesCuba.Viderhe.Cloud.Models.View
{
    public class ModeratorInputViewModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Persistence.Entities.Moderator, System.Guid>
    {
        public ModeratorInputViewModel()
        {
            Account = new AccountDisplayViewModel();
            SelectableUsers = new System.Collections.Generic.List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
        }

        public System.Guid Id { get; set; }

        public AccountDisplayViewModel Account { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "User")]
        public string UserId { get; set; }

        public System.Collections.Generic.List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> SelectableUsers { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "Alias")]
        public string Alias { get; set; }

        [Required(ErrorMessage = "RequiredValidationErrorMessage")]
        [Display(Name = "BornDate")]
        public System.DateTime? BornDate { get; set; }

        [Display(Name = "CivilStatus")]
        public MOROPO.SAPRESSC.Middleware.Identity.Persistence.Enumerations.CivilStatus CivilStatus { get; set; }

        [Display(Name = "Sex")]
        public MOROPO.SAPRESSC.Middleware.Identity.Persistence.Enumerations.Sex Sex { get; set; }

        [Display(Name = "About")]
        public string About { get; set; }

        [FormFileContentTypes(ContentTypes = new string[] { "image/png", "image/jpeg" }, ErrorMessage = "PNG_JPEG_FileAllowedOnlyMessage")]
        [FormFileMaxLength(Length = 1048576, ErrorMessage = "FileLimitSizeExcededMessage")]
        [Display(Name = "ImageFile")]
        public Microsoft.AspNetCore.Http.IFormFile ImageFile { get; set; }

        public bool ResetImage { get; set; }

        public System.Guid ImageId { get; set; }

        public string ImageExtension { get; set; }

        public Persistence.Entities.Moderator Export()
        {
            var entity = new Persistence.Entities.Moderator
            {
                UserId = UserId,
                ModerationKind = Persistence.Enumerations.ModerationKind.Privileged
            };

            Mergue(entity);

            return entity;
        }

        public void Import(Persistence.Entities.Moderator entity)
        {
            Id = entity.Id;
            UserId = entity.UserId;
            Name = entity.Identity.Name;
            LastName = entity.Identity.LastName;
            Alias = entity.Identity.Alias;
            BornDate = entity.Identity.BornDate;
            CivilStatus = entity.Identity.CivilStatus;
            Sex = entity.Identity.Sex;
            About = entity.Identity.About;
            ImageId = entity.Identity.ImageId ?? System.Guid.Empty;
            ImageExtension = entity.Identity.Image?.ContentType.ContentTypeToExtension() ?? null;
        }

        public void Mergue(Persistence.Entities.Moderator entity)
        {
            entity.Identity.Name = Name;
            entity.Identity.LastName = LastName;
            entity.Identity.Alias = Alias;
            entity.Identity.BornDate = BornDate;
            entity.Identity.Sex = Sex;
            entity.Identity.CivilStatus = CivilStatus;
            entity.Identity.About = About;
        }
    }
}
