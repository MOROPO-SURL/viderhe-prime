using MOROPO.SAPRESSC.Middleware.Media.Web.Common.Extensions;

namespace SmartSolucionesCuba.Viderhe.Cloud.Models.View
{
    public class ModeratorDisplayViewModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.Moderator, System.Guid>
    {
        public System.Guid Id { get; set; }

        public AccountDisplayViewModel Account { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Alias { get; set; }

        public System.DateTime? BornDate { get; set; }

        public MOROPO.SAPRESSC.Middleware.Identity.Persistence.Enumerations.CivilStatus CivilStatus { get; set; }

        public MOROPO.SAPRESSC.Middleware.Identity.Persistence.Enumerations.Sex Sex { get; set; }

        public string About { get; set; }

        public System.Guid ImageId { get; set; }

        public string ImageExtension { get; set; }

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
    }
}
