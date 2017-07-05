using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Org.BouncyCastle.Ocsp;
using Portal.Web.Models;

namespace Portal.Web.Controllers
{
	[Authorize]
    public class CoursePlayerController : ApplicationController
	{
		private AjaxRepository _ajax = new AjaxRepository();

		//
		// GET: /CoursePlayer/
		[Authorize(Roles = "User")]
		public ActionResult Index(int? id)
		{
			ViewBag.requestedItem = id;

			// TODO: Må endre til at vi får inn courseid som en paramter
			int courseId = 1;

			List<MovieModel> result = new List<MovieModel>();

			var modules = _ajax.GetModulesForCourse(courseId);

			foreach (var module in modules)
			{
				var model = new MovieModel { VideoUrl = module.VideoUrl, ThumbUrl = module.ThumbUrl };

				foreach (var question in module.ModuleQuestions)
				{
					var q = new QuestionModel();

					var alternative = new Alternative();
					alternative.ImgUrl = question.ImageUrl1;
					alternative.IsCorrect = question.IsImage1Correct;

					q.Alternatives.Add(alternative);

					alternative = new Alternative();
					alternative.ImgUrl = question.ImageUrl2;
					alternative.IsCorrect = question.IsImage2Correct;

					q.Alternatives.Add(alternative);

					model.Questions.Add(q);

				}

				result.Add(model);
			}

			ViewBag.courses = result;


			string username = System.Security.Principal.WindowsPrincipal.Current.Identity.Name;

			int lastSeen = 0;

			var lastSeenModuleId = _ajax.GetLastSeenModuleId(username);

			if (lastSeenModuleId.HasValue)
				lastSeen = lastSeenModuleId.Value;
		

			ViewBag.lastSeen = lastSeen;

			int requestedModule = -1;

			if (id.HasValue)
				requestedModule = id.Value-1; // id er ikke 0-basert

			ViewBag.requestedModule = requestedModule;

			return View();
		}
	}
}