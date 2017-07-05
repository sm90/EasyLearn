using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Portal.Web.Models;
using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;

namespace Portal.Web.Controllers
{
    public class PdfController : ApplicationController
    {
        CourseRepository _courseRepository = new CourseRepository();
        //



        [Authorize]
        public ActionResult Index(string username)
        {
            // Henter pålogget brukers brukernavn
            string loggedInUser = System.Security.Principal.WindowsPrincipal.Current.Identity.Name;

            // Dersom vi ikke har blitt bedt om å hente diplom til en spesifik bruker henter vi for pålogget bruker
            if (String.IsNullOrEmpty(username))
                username = System.Security.Principal.WindowsPrincipal.Current.Identity.Name;

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));


            var loggedInApplicationUser = userManager.FindById(User.Identity.GetUserId());
            var examApplicationUser = userManager.FindByName(username);

            // Dersom man prøver å hente diplom tilhørende en annen bruker må man være administrator
            if (loggedInUser != username && !userManager.IsInRole(loggedInApplicationUser.Id, "Administrator"))
            {
                return RedirectToAction("Index", "Error");
            }

            // Sjekke om brukeren har bestått eksamen
            ExamRepository examRepository = new ExamRepository();
            var exam = examRepository.GetUsersFirstPassedExam(examApplicationUser.Id);

            // exam er null dersom vi ikke finner en bestått eksamen for brukeren
            if (exam == null)
                return RedirectToAction("Index", "Error");

            var profile = _courseRepository.GetUserProfileByUsername(username);

            string templatePath = @AppDomain.CurrentDomain.BaseDirectory + @"Content\diplom.pdf";

            PdfMergeStreamer streamer = new PdfMergeStreamer();
            var pdfMemoryStream = new System.IO.MemoryStream();

            Thread.CurrentThread.CurrentCulture = new CultureInfo("nb-NO");

            streamer.fillPDF(templatePath, pdfMemoryStream, profile.Firstname + " " + profile.Lastname, exam.Completed.Value.ToShortDateString());
            string contentType = "application/pdf";
            var cd = new System.Net.Mime.ContentDisposition();
            cd.Inline = false;
            cd.FileName = "diplom.pdf";


            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(pdfMemoryStream.ToArray(), contentType);
        }


    }
}