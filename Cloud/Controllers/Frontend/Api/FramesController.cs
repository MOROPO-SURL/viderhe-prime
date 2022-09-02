using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models;
using SmartSolucionesCuba.Viderhe.Cloud.Api.Common.Models.Filter;
using System.Linq;

namespace SmartSolucionesCuba.Viderhe.Cloud.Controllers.Frontend.Api
{
    [Microsoft.AspNetCore.Authorization.Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Policy = Security.Authorization.Policies.ACCOUNT_MODERATION)]
    [Route("api/[area]/[controller]")]
    [Area("account")]
    [SAPRESSC.Core.Web.Security.Filters.ChangeUserPasswordRequiredAction]
    [SAPRESSC.Core.Web.Security.Filters.CatchChangeUserPasswordRequiredException]
    public class FramesController : SAPRESSC.Core.Web.Api.Controllers.AbstractEntityApiManagementFilteredController<Persistence.Entities.Frame, long, FrameInputModel, FrameDisplayModel, FrameFilterModel>
    {
        private readonly IEntityRepository<Persistence.Entities.Screenplay, System.Guid> screenplayRepository;
        private readonly IEntityRepository<Persistence.Entities.Placard, System.Guid> placardRepository;

        public FramesController(IEntityRepository<Persistence.Entities.Placard, System.Guid> placardRepository, IEntityRepository<Persistence.Entities.Screenplay, System.Guid> screenplayRepository, IEntityRepository<Persistence.Entities.Frame, long> repository, Microsoft.Extensions.Localization.IStringLocalizer<FramesController> localizer, ILogger<FramesController> logger) : base(repository, localizer, logger)
        {
            this.screenplayRepository = screenplayRepository;
            this.placardRepository = placardRepository;
        }

        public override ActionResult<FrameDisplayModel[]> Get()
        {
            return NotFound();
        }

        public override ActionResult<FrameDisplayModel> Get(long key)
        {
            return NotFound();
        }

        public override ActionResult<SAPRESSC.Core.Common.Models.PaginationModel<FrameDisplayModel>> GetPaginate(int sheet = 1, int limit = 25)
        {
            return NotFound();
        }

        public override ActionResult Edit([FromBody] FrameInputModel inputModel)
        {
            return NotFound();
        }

        [HttpPut("{key}/order/{priority}")]
        public ActionResult Order(long key, int priority)
        {
            try
            {
                var target = Repository.Get(key);

                System.Collections.Generic.Queue<Persistence.Entities.Frame> implicates;

                if (priority < target.Priority)
                {
                    implicates = new System.Collections.Generic.Queue<Persistence.Entities.Frame>(target.Screenplay.Frames.OrderByDescending(e => e.Priority).Where(e => e.Priority >= priority && e.Priority < target.Priority));                    
                }
                else
                {
                    implicates = new System.Collections.Generic.Queue<Persistence.Entities.Frame>(target.Screenplay.Frames.OrderByDescending(e => e.Priority).Where(e => e.Priority <= priority && e.Priority > target.Priority));
                }

                while (implicates.Count > 0)
                {
                    var current = implicates.Dequeue();

                    if (priority < target.Priority)
                    {
                        current.Priority++;                        
                    }
                    else
                    {
                        current.Priority--;
                    }

                    Repository.Update(current, User.Identity.Name, false);
                }

                target.Priority = priority;
                
                Repository.Update(target, User.Identity.Name);
            }
            catch (System.InvalidOperationException e)
            {
                logger.LogError(e.InnerException, e.Message);

                return NoContent();
            }
            catch (System.Exception e)
            {
                logger.LogError(e.InnerException, e.Message);

                return Conflict();
            }

            return Ok();
        }

        protected override System.Linq.Expressions.Expression<System.Func<Persistence.Entities.Frame, bool>> GetGlobalSelector()
        {
            var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            return entity => entity.Screenplay.Account.Id == accountId && entity.Placard.Template.Enabled;
        }

        protected override void PreCreate(Persistence.Entities.Frame entity, FrameInputModel inputModel)
        {
            base.PreCreate(entity, inputModel);

            var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            entity.Screenplay = screenplayRepository.FindOneBy(e => e.Id == inputModel.ScreenplayId && e.Account.Id == accountId) ?? throw new System.InvalidOperationException();

            entity.Placard = placardRepository.FindOneBy(e => e.Id == inputModel.PlacardId && e.Account.Id == accountId) ?? throw new System.InvalidOperationException();

            var screenplayLastFrame = entity.Screenplay.Frames.OrderBy(e => e.Priority).LastOrDefault();

            entity.Priority = screenplayLastFrame?.Priority + 1 ?? 1;
        }

        protected override void PreDelete(Persistence.Entities.Frame entity)
        {
            base.PreDelete(entity);

            var accountId = System.Guid.Parse(User.Claims.First(claim => claim.Type == Security.Authorization.Claims.ACCOUNT_CLAIM).Value);

            if (entity.Placard.Account.Id != accountId) throw new System.InvalidOperationException();
        }
    }
}
