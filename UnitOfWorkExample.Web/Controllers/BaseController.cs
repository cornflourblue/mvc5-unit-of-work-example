using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using UnitOfWorkExample.Domain.Helpers;
using Ninject;

namespace UnitOfWorkExample.Web.Controllers
{
    public class BaseController : Controller
    {
        private bool _isChildAction;

        [Inject]
        public IUnitOfWork UnitOfWork { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _isChildAction = filterContext.ActionDescriptor.GetCustomAttributes(typeof(ChildActionOnlyAttribute), false).Any();
            if (!_isChildAction)
            {
                UnitOfWork.BeginTransaction();
            }
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (!_isChildAction)
            {
                UnitOfWork.Commit();
            }
        }
    }
}
