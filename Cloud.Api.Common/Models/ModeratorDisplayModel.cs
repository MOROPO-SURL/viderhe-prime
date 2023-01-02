namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class ModeratorDisplayModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Persistence.Entities.Moderator, System.Guid>
    {
        public System.Guid Id { get; set; }

        public UserDisplayModel User { get; set; }

        public PersonIdentityDisplayModel Identity { get; set; }

        public bool IsPrivileged { get; set; }

        public void Import(Persistence.Entities.Moderator entity)
        {
            Id = entity.Id;
            User = new UserDisplayModel();
            Identity = new PersonIdentityDisplayModel();
            IsPrivileged = entity.ModerationKind == Persistence.Enumerations.ModerationKind.Privileged;

            User.Import(entity.User);

            Identity.Import(entity.Identity);
        }
    }
}
