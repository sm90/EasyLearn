using Portal.Web.Models;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Web.Controllers
{
    public class AjaxController : ApplicationController
    {
        private CourseRepository _courseRepository = new CourseRepository();
        private AjaxRepository _ajax = new AjaxRepository();

        public ActionResult _ajax_GetCourseModuleStatistics(int id)
        {
            int companyId = GetUsersCompanyId();
            var modules = _ajax.GetModuleStatistics(id, companyId);

            return Json(modules, JsonRequestBehavior.AllowGet);
        }

        public int GetUsersCompanyId()
        {
            string loggedInUser = System.Security.Principal.WindowsPrincipal.Current.Identity.Name;
            var profile = _courseRepository.GetUserProfileByUsername(loggedInUser);

            return profile.CompanyId;
        }


        public ActionResult _ajax_GetCourseModuleAverageScore(int id)
        {
            int companyId = GetUsersCompanyId();

            var modules = _ajax.GetModulesAverageScore(id, companyId);


            // Kaster det om til en anonym type for å unngå sirkulær referanse i entity framework
            var result = from m in modules
                         select new
                         {
                             OrderNum = m.OrderNum,
                             CorrectNum = m.CorrectNum,
                             WrongNum = m.WrongNum,
                             PercentCorrect = m.PercentCorrect,
                             ModuleId = m.ModuleId
                         };


            return Json(modules, JsonRequestBehavior.AllowGet);
        }


        public ActionResult _ajax_SetCurrentModuleId(int id)
        {
            string username = System.Security.Principal.WindowsPrincipal.Current.Identity.Name;

            // TODO: CourseId er hardcoded
            var modules = _ajax.GetModulesForCourse(1).ToArray();

            _ajax.SetLastSeenModule(username, modules[id].ModuleId);

            return null;
        }

        public ActionResult _ajax_GetCourseUsersPassed(int id)
        {
            int companyId = GetUsersCompanyId();
            var result = _ajax.GetGraphTimeLineData(id, companyId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
