using System.Collections.Generic;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using Portal.Web.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Portal.Web.Models.DataObject;

namespace Portal.Web.Controllers
{
    public class SuperadminController : ApplicationController
    {
        private AdminUserRepository _adminUserRepository = new AdminUserRepository();
        private CourseRepository _courseRepository = new CourseRepository();
        public CompanyModel companyModel = new CompanyModel();
        public UserManager<ApplicationUser> UserManager { get; private set; }

        [Authorize(Roles = "Superadmin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Superadmin")]
        public ActionResult CreateUser()
        {
            var profileModel = new ProfileModel();

            var companies = _adminUserRepository.GetCompanies();
            ViewBag.companies = new SelectList(companies, "CompanyId", "Name");



            //var departments = _adminUserRepository.GetDepartments(1);

            //ViewBag.departments = new SelectList(departments, "DepartmentId", "DepartmentName");


            return View(profileModel);
        }


        [Authorize(Roles = "Superadmin")]
        public ActionResult Company(int id = 0)
        {

            var editCompanyModel = new EditCompanyModel();
            var temp = companyModel.CompanyById(id);
            if(temp != null)
            {
                editCompanyModel.Company = temp;
                editCompanyModel.CustomizeCompany = temp.CustomizeCompany != null
                    ? JsonConvert.DeserializeObject<CustomizeCompany>(temp.CustomizeCompany)
                    : new CustomizeCompany();

            }
            return View("Company", editCompanyModel);
        }


        [HttpPost]
        [Authorize(Roles = "Superadmin")]
        public ActionResult Company(EditCompanyModel editCompanyModel)
        {
            if(ModelState.IsValid)
            {
                Company tmp = null;
                if(editCompanyModel.CompanyId == 0)
                {
                    tmp = companyModel.AddCompany(Server, Request.Files, editCompanyModel);     
                }
                else
                {
                     tmp =companyModel.UpdateCompany(Server, Request.Files, editCompanyModel);

                }
                if(editCompanyModel.isSendMail.Equals("true"))
                {
                    var user = _courseRepository.GetUserById(tmp.AdminId);
                    var profile = user.UserProfiles.FirstOrDefault();
                    var param = new Dictionary<string, string>();
                    param["USERNAME"] = user.UserName;
                    param["FNAME"] = profile.Firstname;
                    param["KEY"] = profile.UserKey;
                    param["URL"] = tmp.Domain;
                    EmailModel.inst.Send(tmp.Email, "Easylearn invitation", "userInvite", param);
                }
                Response.Redirect("/Superadmin/Companys");
            }
         
            return View("Company", editCompanyModel);
        }

        public ActionResult _ajax_Companys([DataSourceRequest]DataSourceRequest request)
        {
            var companys = companyModel.GetCompanyList();
            DataSourceResult result = companys.ToDataSourceResult(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Superadmin")]
        public ActionResult Companys([DataSourceRequest]DataSourceRequest request)
        {

            return View("Companys");
        }
        [HttpPost]
        [AllowAnonymous]
        [Authorize(Roles = "Superadmin")]
        public ActionResult DeleteCompany(int id)
        {
            companyModel.DeleteCompany(id);
            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "Superadmin")]
        public async Task<ActionResult> CreateUser(ProfileModel profileModel)
        {

            if (!String.IsNullOrEmpty(profileModel.Password1))
            {
                if (profileModel.Password1 != profileModel.Password2)
                    ModelState.AddModelError("Password2", "Passordene er ikke identiske");

                if (profileModel.Password1.Length < 6)
                    ModelState.AddModelError("Password1", "Passord må minimum bestå av 6 tegn");
            }

            if (_adminUserRepository.IsUsernameAvailable(profileModel.Username))
            {
                ModelState.AddModelError("Username", "Det brukernavnet er ikke ledig");
            }



            var companies = _adminUserRepository.GetCompanies();

            if (!ModelState.IsValid)
            {

                ViewBag.companies = new SelectList(companies, "CompanyId", "Name");
                return View(profileModel);
            }

            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            //    UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };

            var user = new ApplicationUser() { UserName = profileModel.Username };
            var result = await UserManager.CreateAsync(user, profileModel.Password1);

            if (result.Succeeded)
            {
                // Gir vanlige brukerrettigheter
                UserManager.AddToRole(user.Id, "User");

                if (profileModel.IsAdministrator)
                {
                    // Gir brukeren administratorrettigheter
                    UserManager.AddToRole(user.Id, "Administrator");
                }

                UserProfile userProfile = new UserProfile();
                userProfile.CompanyId = profileModel.CompanyId;
                userProfile.Email = profileModel.Email;
                userProfile.Firstname = profileModel.Firstname;
                userProfile.Lastname = profileModel.Lastname;
                userProfile.Phone = profileModel.Phone;
                userProfile.AspNetUserId = user.Id;

                // Knytter den nye brukere til den første avdelingen som er registrert på firmaet
                var department = _adminUserRepository.GetDepartments(profileModel.CompanyId).FirstOrDefault();
                userProfile.DepartmentId = department.DepartmentId;

                _courseRepository.AddUserProfile(userProfile);
                _courseRepository.Save();

                _courseRepository.AddUserCourse(userProfile, 1);

                // Sender ut epost til ny bruker
                //       SendEmail email = new SendEmail();
                //       email.SendJobsEmail(profileModel.Email, profileModel.Username, profileModel.Password1);


                return RedirectToAction("Index");

            }

            ViewBag.companies = new SelectList(companies, "CompanyId", "Name");

            return View(profileModel);

        }


    }
}