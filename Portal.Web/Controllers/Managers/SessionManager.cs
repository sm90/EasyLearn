using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Portal.Web.Models;

namespace Portal.Web.Controllers.Managers
{

    public class ThreadLocalStorage
    {
        public AspNetUser user;
        public UserProfile UserProfile;
        public Company Company;
    }
    public class SessionManager
    {

        public readonly static SessionManager inst =  new SessionManager();
        private CourseRepository courseRepository = new CourseRepository();

        private ThreadLocal<ThreadLocalStorage> storage;
        
        protected SessionManager()
        {

        }
        public AspNetUser User()
        {
          return courseRepository.GetAspNetUserByUsername(System.Security.Principal.WindowsPrincipal.Current.Identity.Name);
        }

        public void SetViseted(UserProfile profile)
        {
            profile.isCreated = 0;
            courseRepository.Save();
        }
    }
}