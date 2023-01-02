namespace SmartSolucionesCuba.Viderhe.Cloud.Models.View.Filter
{
    public class TracesFilterViewModel : MOROPO.SAPRESSC.Core.Persistence.Models.Filter.IEntityFilterModel<MOROPO.SAPRESSC.Core.Persistence.EF.Entities.Trace, long>
    {
        public string Author { get; set; }

        public System.Linq.Expressions.Expression<System.Func<MOROPO.SAPRESSC.Core.Persistence.EF.Entities.Trace, bool>> GetSelector()
        {
            if (Author == null || Author == string.Empty) return null;

            return user => user.Author.Contains(Author);
        }
    }
}
