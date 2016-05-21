using UnitOfWorkExample.Domain.Helpers;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace UnitOfWorkExample.WebApi.Helpers
{
    public class UnitOfWorkActionFilter : ActionFilterAttribute
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            UnitOfWork = actionContext.Request.GetDependencyScope().GetService(typeof(IUnitOfWork)) as IUnitOfWork;
            UnitOfWork.BeginTransaction();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            UnitOfWork = actionExecutedContext.Request.GetDependencyScope().GetService(typeof(IUnitOfWork)) as IUnitOfWork;
            if (actionExecutedContext.Exception == null)
            {
                // commit if no exceptions
                UnitOfWork.Commit();
            }
            else
            {
                // rollback if exception
                UnitOfWork.Rollback();
            }
        }
    }
}