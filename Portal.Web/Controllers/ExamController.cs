using Portal.Web.Models;
using System.Web.Mvc;

namespace Portal.Web.Controllers
{
    [Authorize]
    public class ExamController : ApplicationController
    {
        private CourseRepository _courseRepository = new CourseRepository();
        private ExamRepository _examRepository = new ExamRepository();

        private int _courseId = 1;

        public ActionResult Start()
        {
            string username = System.Security.Principal.WindowsPrincipal.Current.Identity.Name;

            ViewBag.course = _courseRepository.GetCourse(_courseId);


            return View();
        }

        //
        // GET: /Exam/
        [Authorize(Roles = "User")]
        public ActionResult Index()
        {
            string username = System.Security.Principal.WindowsPrincipal.Current.Identity.Name;

            // sjekker om brukeren har fullført siste modul for kurset.
            var modulesCompleted = _examRepository.HasCompletedAllModules(_courseId, username);

            if (!modulesCompleted)
                return RedirectToAction("Index", "CoursePlayer");


            var firstQuestion = _examRepository.StartExam(_courseId, username);

            ViewBag.course = _courseRepository.GetCourse(_courseId);

            return View(firstQuestion);
        }

        [Authorize(Roles = "User")]
        public ActionResult Result()
        {
            var username = System.Security.Principal.WindowsPrincipal.Current.Identity.Name;

            var aspNetUser = _examRepository.GetUser(username);
            var exam = _examRepository.GetUsersLastExam(aspNetUser.Id);

            return View(exam);
        }

        [Authorize(Roles = "User")]
        public ActionResult GetCourse()
        {
            var result = _courseRepository.GetCourse(1);

            var resultSerializable = new { courseId = result.CourseId, name = result.Name };

            return Json(resultSerializable, JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "User")]
        public ActionResult GetQuestions()
        {

            string userName = User.Identity.Name;

            _examRepository.StartExam(1, userName);
            return Json(_courseRepository.GetQuestionsFlat(1), JsonRequestBehavior.AllowGet);
        }


        public JsonResult _ajax_AnswerQuestion(int id)
        {
            string userName = User.Identity.Name;


            // var exam = _examRepository.GetCurrentExamQuestion()
            var nextQuestion = _examRepository.AnswerExamQuestion(id, userName);

            if (nextQuestion != null)
            {
                // Kaster det om til en anonym type for å unngå sirkulær referanse i entity framework
                var result = new
                {
                    ImageUrl1 = nextQuestion.ModuleQuestion.ImageUrl1,
                    ImageUrl2 = nextQuestion.ModuleQuestion.ImageUrl2,

                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Brukeren er ferdig.

                return Json(-1, JsonRequestBehavior.AllowGet);
            }


        }
    }
}