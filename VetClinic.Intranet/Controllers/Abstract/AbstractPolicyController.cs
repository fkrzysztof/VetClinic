using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetClinic.Data;
using VetClinic.Intranet.Policy;
using Microsoft.AspNetCore.Http;

namespace VetClinic.Intranet.Controllers.Abstract
{
    public abstract class AbstractPolicyController : Controller
    {
        protected readonly VetClinicContext _context;

        public AbstractPolicyController(VetClinicContext context)
        {
            _context = context;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            UserPolicy policy = new UserPolicy(_context, HttpContext, this.ControllerContext.RouteData);
            if (await policy.hasNoAccess())
            {
                if (HttpContext.Session.GetString("UserID") != null)
                {
                    filterContext.Result = new RedirectResult(Url.Action("Index", "Home"));
                }
                else
                {
                    filterContext.Result = new RedirectResult(Url.Action("Index", "Login"));
                }

                this.OnActionExecuting(filterContext);
            }
            else
            {
                ViewData = policy.PopulateViewData(ViewData);
                this.OnActionExecuting(filterContext);
                var resultContext = await next();
                this.OnActionExecuted(resultContext);
            }
        }
    }
}
