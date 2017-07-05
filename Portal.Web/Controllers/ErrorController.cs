using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Web.Controllers
{
    public class ErrorController : ApplicationController
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CourseNotCompleted()
        {
            return View();
        }
    }
}