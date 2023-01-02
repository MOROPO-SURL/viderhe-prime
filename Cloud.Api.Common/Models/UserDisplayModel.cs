namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class UserDisplayModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<MOROPO.SAPRESSC.Middleware.Security.Essential.Persistence.Entities.User, string>
    {
        public string Id { get; set; }

        public bool Enabled { get; set; }
        
        public bool PasswordChangeRequired { get; set; }
        
        public System.DateTime Created { get; set; }
        
        public System.DateTime LastSessionDate { get; set; }
        
        public int FailAuthenticationCount { get; set; }

        public void Import(MOROPO.SAPRESSC.Middleware.Security.Essential.Persistence.Entities.User entity)
        {
            Id = entity.Id;
            Enabled = entity.Enabled;
            PasswordChangeRequired = entity.PasswordChangeRequired;
            Created = entity.Created;
            LastSessionDate = entity.LastSessionDate;
            FailAuthenticationCount = entity.FailAuthenticationCount;
        }
    }
}
