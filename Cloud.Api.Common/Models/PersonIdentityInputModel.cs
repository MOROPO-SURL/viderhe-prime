namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class PersonIdentityInputModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityInputModel<MOROPO.SAPRESSC.Middleware.Identity.Persistence.Entities.PersonIdentity, System.Guid>
    {
        public System.Guid Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }

        public string LastName { get; set; }

        public System.DateTime? BornDate { get; set; }

        public MOROPO.SAPRESSC.Middleware.Identity.Persistence.Enumerations.CivilStatus CivilStatus { get; set; }

        public MOROPO.SAPRESSC.Middleware.Identity.Persistence.Enumerations.Sex Sex { get; set; }

        public string Alias { get; set; }

        public ImageInputModel Image { get; set; }

        public string About { get; set; }

        public MOROPO.SAPRESSC.Middleware.Identity.Persistence.Entities.PersonIdentity Export()
        {
            var entity = new MOROPO.SAPRESSC.Middleware.Identity.Persistence.Entities.PersonIdentity();

            Mergue(entity);

            return entity;
        }

        public void Import(MOROPO.SAPRESSC.Middleware.Identity.Persistence.Entities.PersonIdentity entity)
        {
            throw new System.InvalidOperationException();
        }

        public void Mergue(MOROPO.SAPRESSC.Middleware.Identity.Persistence.Entities.PersonIdentity entity)
        {
            entity.Name = Name;
            entity.LastName = LastName;
            entity.BornDate = BornDate;
            entity.CivilStatus = CivilStatus;
            entity.Sex = Sex;
            entity.Alias = Alias;
            entity.About = About;
        }
    }
}
