using System;
using Portal.Web.Models;
using System.Web.Mvc;
using System.Web.Security;
using Portal.Web.Models.Helpers;

namespace Portal.Web.Controllers
{
    public class ApplicationController : Controller
    {
        GranatRepository granatRepository = new GranatRepository();
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (CustomizeHelper.checkSession(Session))
            {
                if (Request.Url != null)
                {
                     CustomizeHelper.SetSession(Session, granatRepository.GetDomeinCompany(Request.Url.Host.ToLower()));
                     Session["isCreated"] = true;
                }                
            }
        }
    }
}