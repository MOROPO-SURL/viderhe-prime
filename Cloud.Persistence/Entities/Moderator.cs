namespace SmartSolucionesCuba.Viderhe.Cloud.Persistence.Entities
{
    public class Moderator : MOROPO.SAPRESSC.Core.Persistence.Entities.IEntity<System.Guid>
    {
        public Moderator()
        {
            Id = System.Guid.NewGuid();
            ModerationKind = Enumerations.ModerationKind.Standard;
            Identity = new MOROPO.SAPRESSC.Middleware.Identity.Persistence.Entities.PersonIdentity();
            AccountBridges = new System.Collections.Generic.List<AccountModeratorBridge>();
        }

        [System.ComponentModel.DataAnnotations.Key]
        public System.Guid Id { get; set; }

        public System.Guid IdentityId { get; set; }

        public MOROPO.SAPRESSC.Middleware.Identity.Persistence.Entities.PersonIdentity Identity { get; set; }

        public string UserId { get; set; }

        public MOROPO.SAPRESSC.Middleware.Security.Essential.Persistence.Entities.User User { get; set; }

        public Enumerations.ModerationKind ModerationKind { get; set; }

        public System.Collections.Generic.List<AccountModeratorBridge> AccountBridges { get; set; }
    }
}