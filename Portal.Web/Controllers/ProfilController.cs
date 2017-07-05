using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Portal.Web.Models;
using System.Text.RegularExpressions;

namespace Portal.Web.Controllers
{
    public class ProfilController : ApplicationController
    {
        CourseRepository repository = new CourseRepository();
 
        //
        // GET: /Profil/
        [Authorize(Roles = "User")]
        public ActionResult Index()
        {
            string username = User.Identity.Name;
            var profile = repository.GetUserProfileByUsername(username);

            ProfileModel profileModel = new ProfileModel();
            profileModel.Firstname = profile.Firstname;
            profileModel.Lastname = profile.Lastname;
           // profileModel.Department = "Salg";
            profileModel.Email = profile.Email;
            profileModel.Phone = profile.Phone;

           return View(profileModel);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult Index(ProfileModel profileModel)
        {
            string username = User.Identity.Name;
            var profile = repository.GetUserProfileByUsername(username);

            profile.Firstname = profileModel.Firstname;
            profile.Lastname = profileModel.Lastname;
            profile.Phone = profileModel.Phone;

            if (ViewData.ModelState["Firstname"].Errors.Count == 0 && ViewData.ModelState["Lastname"].Errors.Count == 0 && ViewData.ModelState["email"].Errors.Count == 0)
            {

                repository.Save();

                ViewBag.profileUpdate = true;
            }
            
            return View(profileModel);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult Passord(PasswordModel passwordModel)
        {
            if (passwordModel.Password1 != passwordModel.Password2)
            {
                ModelState.AddModelError("Password2", "Passordene er ikke identiske");    
            }
            if (passwordModel.Password1 == null || passwordModel.Password1.Length < 6)
            {
                ModelState.AddModelError("Password1", "Passord må minimum bestå av 6 tegn");
            }

            string username = User.Identity.Name;
            var user = repository.GetAspNetUserByUsername(username);

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };

            var result = UserManager.ChangePassword(user.Id, passwordModel.Password0, passwordModel.Password1);

            if (result.Succeeded)
            {
                ViewBag.passwordUpdate = true;
            }

             var profile = repository.GetUserProfileByUsername(username);

            ProfileModel profileModel = new ProfileModel();
            profileModel.Firstname = profile.Firstname;
            profileModel.Lastname = profile.Lastname;
           // profileModel.Department = "Salg";
            profileModel.Email = profile.Email;
            profileModel.Phone = profile.Phone;

            return View("Index", profileModel);
        }
    }
}