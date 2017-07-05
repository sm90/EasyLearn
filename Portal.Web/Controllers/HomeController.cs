using System.IO;
using System.Web.Mvc;

namespace Portal.Web.Controllers
{
    [Authorize]
    public class HomeController : ApplicationController
    {
        [Authorize(Roles = "User")]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Course");
        }

        [Authorize(Roles = "User")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize(Roles = "User")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Css()
        {
            var dir = HttpContext.Server.MapPath("~/content/site.css"); //location of the template file

            FileInfo fileInfo = new FileInfo(dir);

            var stream = fileInfo.OpenRead();


            return File(fileInfo.OpenRead(), "text/plain");

            // "~/Content/site.css"
            // "~/Content/NY_Site.css"
            // return Content("", "text/css");
        }
    }
}