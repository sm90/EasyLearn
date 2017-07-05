using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OfficeOpenXml;
using Portal.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Portal.Web.Models.Helpers;

namespace Portal.Web.Controllers
{
    public class AdminController : ApplicationController
    {
        private CourseRepository _courseRepository = new CourseRepository();
        private AdminUserRepository _adminUserRepository = new AdminUserRepository();
        private StatisticsRepository _statisticsRepository = new StatisticsRepository();
        private ExamRepository _examRepository = new ExamRepository();
        private SendEmail email = new SendEmail();

        public AdminController()
        {
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };

        }

        //
        // GET: /Admin/
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult ResendWelcomeMessage()
        {

            return View();
        }

  
        [Authorize(Roles = "Administrator")]
        public ActionResult SendWelcomeMessage(string Email, string Username, string Password)
        {
            SendEmail email = new SendEmail();
            email.SendJobsEmail(Email, Username, Password);

            ViewBag.sendt = Email;

            return View("ResendWelcomeMessage");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Overview()
        {
            string loggedInUser = System.Security.Principal.WindowsPrincipal.Current.Identity.Name;
            var profile = _courseRepository.GetUserProfileByUsername(loggedInUser);

            int companyId = profile.CompanyId;

            int usersAssignedToCoursesCount = _statisticsRepository.UsersAssignedToCoursesCount(companyId);
            int usersStartedCoursesCount = _statisticsRepository.UsersStartedCoursesCount(companyId);
            int usersCompletedCoursesCount = _statisticsRepository.UsersCompletedCoursesCount(companyId);

            int usersSartedNotCompletedCount = usersStartedCoursesCount - usersCompletedCoursesCount;
            int usersNotStartedCount = usersAssignedToCoursesCount - usersStartedCoursesCount;

            decimal onePercent = 0;

            if (usersAssignedToCoursesCount > 0)
            {
                onePercent = ((decimal)100 / usersAssignedToCoursesCount);
            }

            ViewBag.usersCompletedCoursesPercent = onePercent * usersCompletedCoursesCount;
            ViewBag.usersSartedNotCompletedPercent = onePercent * usersSartedNotCompletedCount;
            ViewBag.usersNotStartedCount = onePercent * usersNotStartedCount;


            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult EditUser(string username)
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult CreateUser()
        {
            var profileModel = new ProfileModel();

            var departments = _adminUserRepository.GetDepartments(GetLoggedInCompanyId());
            ViewBag.departments = new SelectList(departments, "DepartmentId", "DepartmentName");


            return View(profileModel);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult ConfirmUserDeletion(string id)
        {
            // TODO: Kontroller at pålogget bruker tilhører samme firma som brukeren som ønskes slettet

            var profile = _courseRepository.GetUserProfileById(id);

            return View(profile);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult InviteUser(string name)
        {
            var password = CommonHelper.GeneretedPassword();
            var user = _courseRepository.GetUserByUsername(name);
            user.PasswordHash = UserManager.PasswordHasher.HashPassword(password);
            var profile = user.UserProfiles.FirstOrDefault();
            profile.UserKey = CommonHelper.UserKey();
            _courseRepository.Save();
            var param = new Dictionary<string, string>();
            param["USERNAME"] = user.UserName;
            param["FNAME"] = profile.Firstname;
            param["KEY"] = profile.UserKey;
            param["URL"] = profile.Company.Domain;
            EmailModel.inst.Send(profile.Email, "Easylearn invitation", "userInvite", param);
            return Json("ok", JsonRequestBehavior.AllowGet);
  
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteUser(string UserName)
        {
            // TODO: Sjekk at brukeren som slettes tilhører samme firma som brukeren for forsøker å slette
            _adminUserRepository.DeleteUser(UserName);
            return RedirectToAction("Overview");
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult  Profile(ProfileModel profileModel)
        {
            string username = System.Security.Principal.WindowsPrincipal.Current.Identity.Name;
            var profile = _courseRepository.GetUserProfileByUsername(username);

            profile.Firstname = profileModel.Firstname;
            profile.Lastname = profileModel.Lastname;
            profile.Phone = profileModel.Phone;
            profile.Email = profileModel.Email;

            // Det skal ikke være mulig å endre avdeling via dette skjermbildet
            //profile.DepartmentId = profileModel.DepartmentId;
            if (ViewData.ModelState["Firstname"].Errors.Count == 0 && ViewData.ModelState["Lastname"].Errors.Count == 0 && ViewData.ModelState["email"].Errors.Count == 0)
            {
                _courseRepository.Save();

                ViewBag.profileUpdate = true;
            }
            var result = _adminUserRepository.GetUserProfileByUsername(username);




            return View(result);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public ActionResult User(ProfileModel profileModel)
        {
            var profile = _courseRepository.GetUserProfileByUsername(profileModel.Username);

            profile.Firstname = profileModel.Firstname;
            profile.Lastname = profileModel.Lastname;
            profile.Phone = profileModel.Phone;
            profile.DepartmentId = profileModel.DepartmentId;
            profile.Email = profileModel.Email;
            if (ViewData.ModelState["Firstname"].Errors.Count == 0 && ViewData.ModelState["Lastname"].Errors.Count == 0 && ViewData.ModelState["email"].Errors.Count == 0) {
                _courseRepository.Save();
                ViewBag.profileUpdate = true;
            }

           // UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            if (profileModel.IsAdministrator)
            {
                if (!UserManager.IsInRole(profile.AspNetUserId, "Administrator"))
                    UserManager.AddToRole(profile.AspNetUserId, "Administrator");
            }
            else
            {
                if (UserManager.IsInRole(profile.AspNetUserId, "Administrator"))
                    UserManager.RemoveFromRole(profile.AspNetUserId, "Administrator");
            }

            ViewBag.IsAdministrator = UserManager.IsInRole(profile.AspNetUserId, "Administrator");

            var departments = _adminUserRepository.GetDepartments(1);
            ViewBag.departments = new SelectList(departments, "DepartmentId", "DepartmentName");

            var result = _adminUserRepository.GetUserProfileByUsername(profileModel.Username);


            return View(result);
        }

        public ActionResult Profile()
        {
            // TODO: Sjekk at brukeren har rettigheter til å se brukeren.

            var username = System.Security.Principal.WindowsPrincipal.Current.Identity.Name;
            var profile = _adminUserRepository.GetUserProfileByUsername(username);

            // TODO: CourseId er i dag hardcoded
            var graph = _statisticsRepository.GetUserScoreForCourse(profile.AspNetUserId, 1);

            ViewBag.graphElements = graph;

            return View(profile);
        }

        public ActionResult Departments()
        {
            var username = System.Security.Principal.WindowsPrincipal.Current.Identity.Name;
            var profile = _adminUserRepository.GetUserProfileByUsername(username);

            var departments = _adminUserRepository.GetDepartments(profile.CompanyId);

            return View(departments);

        }

        public ActionResult CreateDepartment()
        {
            var department = new Department();
            return View(department);
        }

        [HttpPost]
        public ActionResult CreateDepartment(Department department)
        {
            var username = System.Security.Principal.WindowsPrincipal.Current.Identity.Name;
            var profile = _adminUserRepository.GetUserProfileByUsername(username);

            department.CompanyId = profile.CompanyId;
            department.DepartmentName = department.DepartmentName.Trim();
            if (!String.IsNullOrEmpty(department.DepartmentName))
            {
                var departments =  _adminUserRepository.GetDepartments(profile.CompanyId);
                bool isFree = true;
                foreach (var dep in departments)
                {
                    if (dep.DepartmentName == department.DepartmentName)
                    {
                        isFree = false;
                        break;

                    }
                }
                if (isFree)
                {
                    _adminUserRepository.AddDepartment(department);
                    return RedirectToAction("Departments");
                }
                else
                {
                    ModelState.AddModelError("DepartmentName", "Deprtment name is dublicte");
                }
            }
            else
            {
                ModelState.AddModelError("DepartmentName", "Deprtment name is empty");
            }
          
            return  View(department);
            
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

            string username = System.Security.Principal.WindowsPrincipal.Current.Identity.Name;
            var user = _courseRepository.GetAspNetUserByUsername(username);

         //   var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            //   UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };

            var result = UserManager.ChangePassword(user.Id, passwordModel.Password0, passwordModel.Password1);

            if (result.Succeeded)
            {
                ViewBag.passwordUpdate = true;
            }

            var profile = _courseRepository.GetUserProfileByUsername(username);

            //ProfileModel profileModel = new ProfileModel();
            //profileModel.Firstname = profile.Firstname;
            //profileModel.Lastname = profile.Lastname;
            //// profileModel.Department = "Salg";
            //profileModel.Email = profile.Email;
            //profileModel.Phone = profile.Phone;

            return View("Profile", profile);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult SetPassord(PasswordModel passwordModel, string username)
        {
            if (passwordModel.Password1 != passwordModel.Password2)
            {
                ModelState.AddModelError("Password2", "Passordene er ikke identiske");
            }
            if (passwordModel.Password1 == null || passwordModel.Password1.Length < 6)
            {
                ModelState.AddModelError("Password1", "Passord må minimum bestå av 6 tegn");
            }

            var user = _courseRepository.GetAspNetUserByUsername(username);

          //  var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            UserManager.RemovePassword(user.Id);
            var result = UserManager.AddPassword(user.Id, passwordModel.Password1);


            if (result.Succeeded)
            {
                ViewBag.passwordUpdate = true;
            }

            var profile = _adminUserRepository.GetUserProfileByUsername(username);

            var graph = _statisticsRepository.GetUserScoreForCourse(profile.AspNetUserId, 1);

            ViewBag.graphElements = graph;

            var departments = _adminUserRepository.GetDepartments(profile.CompanyId);
            ViewBag.departments = new SelectList(departments, "DepartmentId", "DepartmentName");

            //UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ViewBag.IsAdministrator = UserManager.IsInRole(profile.AspNetUserId, "Administrator");


            return View("User", profile);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult User(string id)
        {
            // TODO: Sjekk at brukeren har rettigheter til å se brukeren.

            var profile = _adminUserRepository.GetUserProfileByAspNetUserId(id);

            var graph = _statisticsRepository.GetUserScoreForCourse(profile.AspNetUserId, 1);

            ViewBag.graphElements = graph;

            var departments = _adminUserRepository.GetDepartments(profile.CompanyId);
            ViewBag.departments = new SelectList(departments, "DepartmentId", "DepartmentName");

           // UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            ViewBag.IsAdministrator = UserManager.IsInRole(profile.AspNetUserId, "Administrator");

            return View(profile);

        }

        public UserManager<ApplicationUser> UserManager { get; private set; }


        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> CreateUser(ProfileModel profileModel)
        {

            if ( String.IsNullOrEmpty(profileModel.Username) )
            {
                if (!String.IsNullOrEmpty(profileModel.Firstname) && !String.IsNullOrEmpty(profileModel.Lastname))
                {
                    profileModel.Username = profileModel.Firstname + profileModel.Lastname;
                }
                else
                {
                    ModelState.AddModelError("Username", "Drukernavnet er obligatorisk");
                }
                
            }

            if (_adminUserRepository.IsUsernameAvailable(profileModel.Username))
            {
                ModelState.AddModelError("Username", "Det brukernavnet er ikke ledig");
            }

            var departments = _adminUserRepository.GetDepartments(1);
            ViewBag.departments = new SelectList(departments, "DepartmentId", "DepartmentName");

            if (!ModelState.IsValid)
            {

                ViewBag.departments = new SelectList(departments, "DepartmentId", "DepartmentName");
                return View(profileModel);
            }

           // UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
             

            var user = new ApplicationUser() { UserName = profileModel.Username };
            var result = await UserManager.CreateAsync(user, CommonHelper.GeneretedPassword());

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
                userProfile.CompanyId = GetLoggedInCompanyId();
                userProfile.Email = profileModel.Email;
                userProfile.Firstname = profileModel.Firstname;
                userProfile.Lastname = profileModel.Lastname;
                userProfile.Phone = profileModel.Phone;
                userProfile.AspNetUserId = user.Id;
                userProfile.UserKey = CommonHelper.UserKey();
                userProfile.isCreated = 1;
                userProfile.DepartmentId = profileModel.DepartmentId;

                _courseRepository.AddUserProfile(userProfile);
                _courseRepository.Save();

                _courseRepository.AddUserCourse(userProfile, 1);

                // Sender ut epost til ny bruker
                var company = _adminUserRepository.GetCompanyById(userProfile.CompanyId);
                var param = new Dictionary<string, string>();
                param["USERNAME"] = user.UserName;
                param["FNAME"] = userProfile.Firstname;
                param["KEY"] = userProfile.UserKey;
                param["URL"] = company.Domain;
                EmailModel.inst.Send(userProfile.Email, "Easylearn invitation", "userInvite", param);


                return RedirectToAction("Index");

            }

            ViewBag.departments = new SelectList(departments, "DepartmentId", "DepartmentName");


            return View(profileModel);

        }


        public ActionResult _ajax_ReturnUsers([DataSourceRequest]DataSourceRequest request)
        {
            List<AdminUsersModel> users = new List<AdminUsersModel>();


            users.AddRange(_courseRepository.GetAdminUsersModels(GetLoggedInCompanyId()));


            DataSourceResult result = users.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult _ajax_GetUserCourseList([DataSourceRequest]DataSourceRequest request, int id)
        {

            var user = _courseRepository.GetUserProfileById(id);

            var courses = _examRepository.GetCourseResultsForUser(user.AspNetUserId);

            DataSourceResult result = courses.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public int GetLoggedInCompanyId()
        {
            string loggedInUser = System.Security.Principal.WindowsPrincipal.Current.Identity.Name;
            var profile = _courseRepository.GetUserProfileByUsername(loggedInUser);

            return profile.CompanyId;
        }

        public ActionResult _ajax_StatUsers([DataSourceRequest]DataSourceRequest request)
        {


            var users = _statisticsRepository.GetStatUsers(GetLoggedInCompanyId(), 1);


            DataSourceResult result = users.ToDataSourceResult(request);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult UpLoad()
        {
            return View();
        }

        public ActionResult FilterMenuCustomization_Cities()
        {
            var departments = _adminUserRepository.GetDepartments(GetLoggedInCompanyId());

            List<string> result = new List<string>();

            foreach (var department in departments)
            {
                result.Add(department.DepartmentName);
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult UploadUsers(HttpPostedFileBase file)
        {
            string username = System.Security.Principal.WindowsPrincipal.Current.Identity.Name;
            var profile = _courseRepository.GetUserProfileByUsername(username);
            int companyId = profile.CompanyId;
            List<ImportMessages> messages = new List<ImportMessages>();

            if (file == null)
            {
                ViewBag.uploadSuccess = false;

                messages.Add(new ImportMessages("Serveren mottok ingen fil", true));
                ViewBag.messages = messages;

                return View("UploadResult");
            }

            ExcelPackage package = null;
            try
            {
                package = new ExcelPackage(file.InputStream);
            }
            catch (Exception)
            {
                messages.Add(new ImportMessages("Klarte ikke å lese Excel-filen. Bruker du riktig mal?", true));
                ViewBag.messages = messages;
                return View("UploadResult");
            }



            ImportUsers importUsers = new ImportUsers(companyId, package, "Mal");


            //bool isImportOk = import.Import();

            //ViewData["success"] = isImportOk;
            //ViewData["messages"] = import.ResultMessages;

            return View("UploadResult");

        }


    }
}