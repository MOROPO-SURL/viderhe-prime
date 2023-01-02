namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models.Filter
{
    public class FrameFilterModel : MOROPO.SAPRESSC.Core.Persistence.Models.Filter.IEntityFilterModel<Persistence.Entities.Frame, long>
    {
        public System.Guid ScreenplayId { get; set; }

        public System.Linq.Expressions.Expression<System.Func<Persistence.Entities.Frame, bool>> GetSelector()
        {
            return entity => entity.ScreenplayId == ScreenplayId;
        }
    }
}
