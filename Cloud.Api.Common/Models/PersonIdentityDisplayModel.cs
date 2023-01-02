namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models
{
    public class PersonIdentityDisplayModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<MOROPO.SAPRESSC.Middleware.Identity.Persistence.Entities.PersonIdentity, System.Guid>
    {
        public System.Guid Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public System.DateTime? BornDate { get; set; }

        public MOROPO.SAPRESSC.Middleware.Identity.Persistence.Enumerations.CivilStatus CivilStatus { get; set; }

        public MOROPO.SAPRESSC.Middleware.Identity.Persistence.Enumerations.Sex Sex { get; set; }

        public string Alias { get; set; }

        public string About { get; set; }

        public ImageDisplayModel Image { get; set; }

        public void Import(MOROPO.SAPRESSC.Middleware.Identity.Persistence.Entities.PersonIdentity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            LastName = entity.LastName;
            BornDate = entity.BornDate;
            CivilStatus = entity.CivilStatus;
            Sex = entity.Sex;
            Alias = entity.Alias;
            About = entity.About;

            if (entity.Image != null)
            {
                Image = new ImageDisplayModel();

                Image.Import(entity.Image);
            }
        }
    }
}
