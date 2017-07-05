using Kendo.Mvc.UI;
using Portal.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Portal.Web.Controllers
{
    public class TestController : ApplicationController
    {
        public ActionResult Test()
        {
            return View();
        }


        //
        // GET: /Test/
        public ActionResult Index()
        {
            List<Testmodel> test = new List<Testmodel>();

            var testModel = new Testmodel { Column1 = "aaaa", Column2 = "bbbb", Column3 = DateTime.Now, Column4 = 3 };
            test.Add(testModel);
            testModel = new Testmodel { Column1 = "cccc", Column2 = "dddd", Column3 = DateTime.Now.AddDays(-1), Column4 = null };
            test.Add(testModel);
            testModel = new Testmodel { Column1 = "eeee", Column2 = "ffff", Column3 = DateTime.Now.AddDays(-2), Column4 = 8 };
            test.Add(testModel);
            testModel = new Testmodel { Column1 = "gggg", Column2 = "hhhh", Column3 = DateTime.Now.AddDays(-3), Column4 = -3 };
            test.Add(testModel);


            return View(test);
        }

        public ActionResult _ajax_GetTestdata([DataSourceRequest]DataSourceRequest request)
        {
            List<Testmodel> test = new List<Testmodel>();

            var testModel = new Testmodel { Column1 = "aaaa", Column2 = "bbbb", Column3 = DateTime.Now, Column4 = 3 };
            test.Add(testModel);
            testModel = new Testmodel { Column1 = "cccc", Column2 = "dddd", Column3 = DateTime.Now.AddDays(-1), Column4 = null };
            test.Add(testModel);
            testModel = new Testmodel { Column1 = "eeee", Column2 = "ffff", Column3 = DateTime.Now.AddDays(-2), Column4 = 8 };
            test.Add(testModel);
            testModel = new Testmodel { Column1 = "gggg", Column2 = "hhhh", Column3 = DateTime.Now.AddDays(-3), Column4 = -3 };
            test.Add(testModel);

            return Json(test.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}