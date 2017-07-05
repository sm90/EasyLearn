using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Portal.Web.Models;
using Portal.Web.Controllers.Managers;
using Portal.Web.Models.Helpers;

namespace Portal.Web.Controllers
{
    [Authorize]
    public class CourseController : ApplicationController
    {
        //
        // GET: /Course/
        [Authorize(Roles = "User")]
        public ActionResult Index()
        {
            //var vimeo = new Vimeo();
            //vimeo.Test();
            var granatRepository = new GranatRepository();
            Session["companyCss"] = "~/Content/site.css";
            if (Request.Url != null && Session[CustomizeHelper.sessionMarcker] == null)
                CustomizeHelper.SetSession(Session, granatRepository.GetDomeinCompany(Request.Url.Host.ToLower()));
            Session["isCreated"] = false;

            
        
            AspNetUser user = SessionManager.inst.User();
            if (user != null)
            {
                UserProfile profile = user.UserProfiles.FirstOrDefault();
                if (profile.isCreated != null && profile.isCreated.Value == 1)
                {
                    Session["isCreated"] = true;
                    SessionManager.inst.SetViseted(profile);
                }
            }
      
            
            return View();
        }
	}
}