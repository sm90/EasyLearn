using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Portal.Web.Models
{
    public class GranatRepository
    {
        private Entities _db = new Entities();
        protected string defaultCompany = "Jobz";

        public List<Course> GetCourses()
        {
            return _db.Courses.ToList();
        }

        public void AddCourse(Course course)
        {
            _db.Courses.Add(course);
            Save();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public Company GetUsersCompany(string username)
        {
            var user = _db.AspNetUsers.FirstOrDefault(u => u.UserName == username);

            if (user == null)
                return null;

            var profile = user.UserProfiles.FirstOrDefault();

            if (profile == null)
                return null;

            return profile.Company;
        }

        public Company GetDomeinCompany(string domian)
        {
            try
            {
                return _db.Companies.FirstOrDefault(a => a.Domain == domian) ??
                       _db.Companies.FirstOrDefault(a => a.Name == defaultCompany);
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}