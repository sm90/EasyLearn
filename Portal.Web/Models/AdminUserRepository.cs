using System;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Web.Models
{
    public class AdminUserRepository
    {

        private Entities _db = new Entities();


        public void SetLastLoginDate(string username)
        {
            var user = _db.UserProfiles.FirstOrDefault(u => u.AspNetUser.UserName == username);

            if (user != null)
            {
                user.LastLoginDate = DateTime.Now;
                _db.SaveChanges();
            }
        }

        public void DeleteUser(string username)
        {
            var aspNetUser = _db.AspNetUsers.FirstOrDefault(a => a.UserName == username);

            _db.AspNetUsers.Remove(aspNetUser);


            _db.SaveChanges();
        }

        public void DeleteUserWithOutSave(string username)
        {
            var aspNetUser = _db.AspNetUsers.FirstOrDefault(a => a.UserName == username);
            _db.Entry(aspNetUser).State = System.Data.Entity.EntityState.Deleted;
        }

        public List<Company> GetCompanies()
        {
            return _db.Companies.OrderBy(c => c.Name).ToList();
        }

        public List<Department> GetDepartments(int companyId)
        {
            return _db.Departments.Where(d => d.CompanyId == companyId).OrderBy(d => d.DepartmentName).ToList();

        }

        public Company GetCompanyById(int companyId)
        {
            return _db.Companies.Where(d => d.CompanyId == companyId).FirstOrDefault();

        }

        public Department GetDepartment(int departmentId)
        {
            return _db.Departments.FirstOrDefault(d => d.DepartmentId == departmentId);
        }

        public void AddDepartment(Department department)
        {
            _db.Departments.Add(department);
            _db.SaveChanges();
        }

        public void DeleteDepartment(int departmentId)
        {
            var department = _db.Departments.FirstOrDefault(d => d.DepartmentId == departmentId);

            if (department != null)
            {
                _db.Departments.Remove(department);
                _db.SaveChanges();
            }
        }


        /// <summary>
        /// Returnerer en brukers UserProfile ut fra brukernavn
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserProfile GetUserProfileByUsername(string username)
        {
            var result = from aspusers in _db.AspNetUsers
                         from profile in _db.UserProfiles
                         where aspusers.Id == profile.AspNetUserId
                               && aspusers.UserName == username
                         select profile;


            return result.FirstOrDefault();
        }

        public UserProfile GetUserProfileByAspNetUserId(string id)
        {
            var result = from aspusers in _db.AspNetUsers
                         from profile in _db.UserProfiles
                         where id == profile.AspNetUserId
                         select profile;


            return result.FirstOrDefault();
        }

        public bool IsUsernameAvailable(string username)
        {
            return _db.AspNetUsers.Any(u => u.UserName.ToLower() == username);
        }
    }
}