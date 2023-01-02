using System.ComponentModel.DataAnnotations;

namespace SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models.Filter
{
    public class ScheduledRelaunchFilterModel : MOROPO.SAPRESSC.Core.Persistence.Models.Filter.IEntityFilterModel<Persistence.Entities.ScheduledRelaunch, System.Guid>
    {
        [Required, RegularExpression(@"^[0-9A-Z]{7}$")]
        public string PlayerId { get; set; }

        public System.Linq.Expressions.Expression<System.Func<Persistence.Entities.ScheduledRelaunch, bool>> GetSelector()
        {
            return entity => entity.PlayerId == PlayerId;
        }
    }
}
