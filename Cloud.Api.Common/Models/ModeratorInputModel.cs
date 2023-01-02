namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class ModeratorInputModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityInputModel<Persistence.Entities.Moderator, System.Guid>
    {
        public System.Guid Id { get; set; }

        public UserInputModel User { get; set; }

        public PersonIdentityInputModel Identity { get; set; }

        public bool IsPrivileged { get; set; }

        public Persistence.Entities.Moderator Export()
        {
            var entity = new Persistence.Entities.Moderator
            {
                User = new MOROPO.SAPRESSC.Middleware.Security.Essential.Persistence.Entities.User
                {
                    Id = User.Id,
                    FailAuthenticationCount = 0,
                    HasFullAccess = false
                },
                Identity = new MOROPO.SAPRESSC.Middleware.Identity.Persistence.Entities.PersonIdentity()
            };

            entity.UserId = entity.User.Id;
            entity.IdentityId = entity.Identity.Id;

            Mergue(entity);

            return entity;
        }

        public void Import(Persistence.Entities.Moderator entity)
        {
            throw new System.NotImplementedException();
        }

        public void Mergue(Persistence.Entities.Moderator entity)
        {
            entity.ModerationKind = IsPrivileged ? Persistence.Enumerations.ModerationKind.Privileged : Persistence.Enumerations.ModerationKind.Standard;
            entity.User.Enabled = User.Enabled;
            entity.User.PasswordChangeRequired = User.PasswordChangeRequired;
            entity.Identity.Name = Identity.Name;
            entity.Identity.LastName = Identity.LastName;
            //entity.Identity.BornDate = Identity.BornDate;
            //entity.Identity.Alias = Identity.Alias;
            //entity.Identity.CivilStatus = Identity.CivilStatus;
            entity.Identity.Sex = Identity.Sex;
            entity.Identity.About = Identity.About;
        }
    }
}
