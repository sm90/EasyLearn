using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Portal.Web.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Portal.Web.Controllers
{
    public class GranatController : ApplicationController
    {
        private GranatRepository _granatRepository = new GranatRepository();
        private CourseRepository _courseRepository = new CourseRepository();
        //
        // GET: /Granat/
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Courses()
        {
            return View();
        }

        public ActionResult _ajax_ReturnUsers([DataSourceRequest]DataSourceRequest request)
        {

            List<AdminUsersModel> users = new List<AdminUsersModel>();

            string loggedInUser = System.Security.Principal.WindowsPrincipal.Current.Identity.Name;
            var profile = _courseRepository.GetUserProfileByUsername(loggedInUser);


            users.AddRange(_courseRepository.GetAdminUsersModels(profile.CompanyId));


            DataSourceResult result = users.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}