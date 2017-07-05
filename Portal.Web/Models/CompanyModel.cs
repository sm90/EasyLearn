using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Org.BouncyCastle.Asn1.Ocsp;
using Portal.Web.Models.DataObject;
using Portal.Web.Models.Helpers;
using System.Data.Entity.Infrastructure;

namespace Portal.Web.Models
{
    public class CompanyModel
    {
        private Entities _db = new Entities();
        private AdminUserRepository adminUserRepository = new AdminUserRepository();
        private CourseRepository courseRepository = new CourseRepository();
        public UserManager<ApplicationUser> UserManager { get; private set; }

        public const string Root = "/Content/companies/";
        public const string LogoUrlName = "logo";
        public const string LogoLoginName = "lognLogo";
        public const string InnitialDepartment = "Administration";

        public CompanyModel()
        {
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };
        }

        public Company CompanyById(int id)
        {
            try
            {
                return (from x in _db.Companies where x.CompanyId == id select x).First();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ComanyJsonDto> GetCompanyList()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nb-NO");

            return (from u in _db.Companies select new ComanyJsonDto()
            {
                Name = u.Name,
                Domain = u.Domain,
                Contact = u.ContactName + " " +u.ContactSurname,
                CompanyId =  u.CompanyId,
                Logo =  u.CustomizeCompany,
                SetCompanyType = u.CompanyType
            }).ToList();
        }
 
        public Company AddCompany(HttpServerUtilityBase server, HttpFileCollectionBase files, EditCompanyModel company)
        {
            Company tmp = company.Company;
            string pswd = System.Web.Security.Membership.GeneratePassword(6, 2);
            var customData = MakeCustomizeCompany(server, files, company);
            tmp.CustomizeCompany = JsonConvert.SerializeObject(customData);
            tmp.TempPassword = pswd;
            tmp.CompanyStatus = CompanyStatus.EXPECTATION;
            _db.Companies.Add(tmp);
            _db.SaveChanges();
            var department = new Department();
            department.CompanyId = tmp.CompanyId;
            department.DepartmentName = InnitialDepartment;
            adminUserRepository.AddDepartment(department);
            //var userName = tmp.Name.Replace(" ", "_");
            var userName = tmp.Email;
            var user = new ApplicationUser() { UserName = userName };
            var result =  UserManager.Create(user, pswd);
            Company newComapny = null;
            if(result.Succeeded )
            {
                UserManager.AddToRole(user.Id, "User");
                UserManager.AddToRole(user.Id, "Administrator");
                var userProfile = new UserProfile
                {
                    CompanyId = tmp.CompanyId,
                    Email = tmp.Email,
                    Firstname = tmp.ContactName,
                    Lastname = tmp.ContactSurname,
                    Phone = tmp.ContactPhone,
                    AspNetUserId = user.Id,
                    DepartmentId = department.DepartmentId,
                    UserKey = CommonHelper.UserKey()
                };

                courseRepository.AddUserProfile(userProfile);
                courseRepository.AddUserCourse(userProfile, 1);
                tmp.AdminId = user.Id;
                _db.SaveChanges();

            }

            return tmp;
        }

        public void ChngeCompanyStatus(int id, CompanyStatus status)
        {
            var company = CompanyById(id);
            company.CompanyStatus = status;
            _db.SaveChanges();
        }

        public void DeleteCompany(int id)
        {
            var objectContext = ((IObjectContextAdapter)_db).ObjectContext;
            var company = CompanyById(id);
            var departments = company.Departments.ToList();
            var users = company.UserProfiles.ToList();
            foreach (var user in users)
            {
                var aspUser = (from x in _db.AspNetUsers where x.Id == user.AspNetUserId select x).First();
                objectContext.DeleteObject(user);
                objectContext.DeleteObject(aspUser);
            }
            foreach(var department in departments)
            {
                objectContext.DeleteObject(department);
            
            }
            objectContext.DeleteObject(company);
    
            try
            {
                _db.SaveChanges();
            }
            catch(Exception ex){
                var a = 0;
                a++;
                a++;
            }
        }

        public Company UpdateCompany(HttpServerUtilityBase server, HttpFileCollectionBase files, EditCompanyModel company)
        {
            var tmp = CompanyById(company.CompanyId);
            var oldCustomizeCompany  = tmp.CustomizeCompany != null
                  ? JsonConvert.DeserializeObject<CustomizeCompany>(tmp.CustomizeCompany)
                  : new CustomizeCompany();
            company.UpdateCompany(tmp);
            var customData = MakeCustomizeCompany(server, files, company);
            if(customData.LoginLogoUrl != null)
            {
                oldCustomizeCompany.LoginLogoUrl = customData.LoginLogoUrl;
            }
            if (customData.LogoUrl != null)
            {
                oldCustomizeCompany.LogoUrl = customData.LogoUrl;
            }
            if (customData.HeaderColor != null)
            {
                oldCustomizeCompany.HeaderColor = customData.HeaderColor;
            }
            else
            {
                oldCustomizeCompany.HeaderColor = "";
            }
            if (customData.MenuActiveColor != null)
            {
                oldCustomizeCompany.MenuActiveColor = customData.MenuActiveColor;
            }
            else
            {
                oldCustomizeCompany.MenuActiveColor = "";
            }
            if (customData.MenuPassiveColor != null)
            {
                oldCustomizeCompany.MenuPassiveColor = customData.MenuPassiveColor;
            }
            else
            {
                oldCustomizeCompany.MenuPassiveColor = "";
            }
            if (customData.ButtonsBackgroundColor != null)
            {
                oldCustomizeCompany.ButtonsBackgroundColor = customData.ButtonsBackgroundColor;
            }
            else
            {
                oldCustomizeCompany.ButtonsBackgroundColor = "";
            }
            if (customData.ButtonsActiveColor != null)
            {
                oldCustomizeCompany.ButtonsActiveColor = customData.ButtonsActiveColor;
            }
            else
            {
                oldCustomizeCompany.ButtonsActiveColor = "";
            }
            if (customData.StartVidio != null)
            {
                oldCustomizeCompany.StartVidio = customData.StartVidio;
            }
            tmp.CustomizeCompany = JsonConvert.SerializeObject(oldCustomizeCompany);
            _db.SaveChanges();
            return tmp;
        }

        protected CustomizeCompany MakeCustomizeCompany(HttpServerUtilityBase server, HttpFileCollectionBase files,
            EditCompanyModel company)
        {
            var result = new CustomizeCompany();
            SaveFiles(server, files, result, company.companyName);
            //result.BackgroundColor = company.backgroundColor;
            result.MenuActiveColor = company.menuActiveColor;
            result.MenuPassiveColor = company.menuPassiveColor;
            result.HeaderColor = company.headerColor;
            result.ButtonsBackgroundColor = company.buttonsBackgroundColor;
            result.ButtonsActiveColor = company.buttonsActiveColor;
            result.StartVidio = company.startVidio;
            return result;
        }
        protected void SaveFiles(HttpServerUtilityBase server, HttpFileCollectionBase files,CustomizeCompany company, string companyName)
        {
            if(files.Count <= 0) return;
            companyName = companyName.Replace(" ", "_");
            var folder = server.MapPath(Root + companyName);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var logoFile = files["logo"];
            var loginLogoFile = files["loginLogo"];

            if (logoFile != null && logoFile.ContentLength > 0)
            {
                var fileName = LogoUrlName + Path.GetExtension(logoFile.FileName);
                var path = Path.Combine(server.MapPath(@"~"+Root + companyName), fileName);
                company.LogoUrl = Root + companyName + @"/" + fileName;
                logoFile.SaveAs(path);
            }
            if (loginLogoFile != null && loginLogoFile.ContentLength > 0)
            {
                var fileName = LogoLoginName + Path.GetExtension(loginLogoFile.FileName);
                var path = Path.Combine(server.MapPath(@"~" + Root + companyName), fileName);
                company.LoginLogoUrl = Root + companyName + @"/" + fileName;
                loginLogoFile.SaveAs(path);
            }
        }

    }


}