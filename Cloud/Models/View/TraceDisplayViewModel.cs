using MOROPO.SAPRESSC.Core.Persistence.EF.Entities;

namespace SmartSolucionesCuba.Viderhe.Cloud.Models.View
{
    public class TraceDisplayViewModel : MOROPO.SAPRESSC.Core.Persistence.Models.IEntityDisplayModel<Trace, long>
    {
        public long Id { get; set; }

        public Microsoft.EntityFrameworkCore.EntityState Type { get; set; }

        public string Author { get; set; }

        public System.DateTime Registered { get; private set; }

        public string EntityName { get; set; }

        public string BeforeData { get; set; }

        public string AfterData { get; set; }

        public void Import(Trace entity)
        {
            Id = entity.Id;
            Type = entity.Type;
            Author = entity.Author;
            Registered = entity.Registered;
            EntityName = entity.EntityName;
            BeforeData = entity.BeforeData;
            AfterData = entity.AfterData;
        }
    }
}
