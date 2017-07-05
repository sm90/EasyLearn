using System;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Web.Models
{
    public class CourseRepository
    {
        private Entities _db = new Entities();
        public Microsoft.AspNet.Identity.UserManager<ApplicationUser> userManager { get; private set; }


        public CourseRepository()
        {
            userManager = new Microsoft.AspNet.Identity.UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(new ApplicationDbContext()));
        }
        public Course GetCourse(int id)
        {
            return _db.Courses.FirstOrDefault(c => c.CourseId == id);
        }


        public List<Course> GetCourses()
        {
            return _db.Courses.ToList();
        }

        public Module GetModule(int id)
        {
            return _db.Modules.FirstOrDefault(m => m.ModuleId == id);
        }

        public List<Module> GetModules(int id)
        {
            return _db.Modules.Where(m => m.CourseId == id).ToList();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public List<ModuleQuestion> GetQuestions(int moduleId)
        {
            return _db.ModuleQuestions.Where(q => q.ModuleId == moduleId).ToList();
        }

        /// <summary>
        /// Returnerer siste examen for en bruker
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        //public ExamResult GetUsersLatestExam(string username)
        //{
        //    var userProfile = GetUserProfileByUsername(username);

        //    var exam = userProfile.ExamResults.OrderByDescending(e => e.ExamResultId).FirstOrDefault();

        //    return exam;
        //}

        /// <summary>
        /// Returnerer en brukerprofil tilhørende et brukernavn
        /// </summary>
        /// <param name="username">Brukernavnet vi skal finne profilen til.</param>
        /// <returns>UserProfile. Returnerer null dersom profilen ikke er funnet</returns>
        public UserProfile GetUserProfileByUsername(string username)
        {
            var user = _db.AspNetUsers.FirstOrDefault(u => u.UserName == username);

            if (user == null)
                return null;

            return user.UserProfiles.FirstOrDefault();

        }

        public AspNetUser GetUserByUsername(string username)
        {
            var user = _db.AspNetUsers.FirstOrDefault(u => u.UserName == username);

            if (user == null)
                return null;

            return user;

        }

        public UserProfile GetUserProfileById(int userId)
        {
            return _db.UserProfiles.FirstOrDefault(u => u.UserProfileId == userId);

        }

        public UserProfile GetUserProfileById(string userId)
        {
            return GetUserById(userId).UserProfiles.First();

        }

        public AspNetUser GetUserById(string userId)
        {
            return _db.AspNetUsers.FirstOrDefault(u => u.Id == userId);

        }

        /// <summary>
        /// Returnerer et AspNetUser objekt tilhørende et brukernavn
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public AspNetUser GetAspNetUserByUsername(string username)
        {
            return _db.AspNetUsers.FirstOrDefault(u => u.UserName == username);
        }

        public List<AdminUsersModel> GetAdminUsersModels(int companyId)
        {
            var result = from u in _db.AspNetUsers
                         from p in _db.UserProfiles
                         where u.Id == p.AspNetUserId
                         && p.CompanyId == companyId
                         select new AdminUsersModel()
                         {
                             Course1 = true,
                             Course2 = false,
                             Course3 = false,
                             Department = p.Department.DepartmentName,
                             Firstname = p.Firstname,
                             Lastname = p.Lastname,
                             Username = u.UserName,
                             AspNetUserId = u.Id

                         };

            return result.ToList();
        }

        public List<StatUsersModel> GetStatUserModel()
        {
            var result = from u in _db.AspNetUsers
                         from p in _db.UserProfiles
                         where u.Id == p.AspNetUserId
                         select new StatUsersModel()
                         {

                             Firstname = p.Firstname,
                             Lastname = p.Lastname,
                             Username = u.UserName,
                             AspNetUserId = u.Id
                             

                         };

            return result.ToList();
        }

        public void AddUserProfile(UserProfile userProfile)
        {
            _db.UserProfiles.Add(userProfile);
        }

        public void Personalization(PersonalizationModel model)
        {
            var user = GetUserById(model.Id);
            var enumator = user.UserProfiles.GetEnumerator();
            enumator.MoveNext();
            var profile = enumator.Current;
            profile.Firstname = model.Firstname;
            profile.Lastname = model.Lastname;
            profile.Email = model.Email;
            profile.Phone = model.Phone;
            user.PasswordHash = userManager.PasswordHasher.HashPassword(model.Password);
            user.UserName = model.UserName;
            _db.SaveChanges();
        }


        public void AddUserCourse(UserProfile userProfile, int courseId)
        {
            var userCourse = new UserCours();
            userCourse.UserProfileId = userProfile.UserProfileId;
            userCourse.CourseId = courseId;
            userCourse.Completed = false;
            userCourse.HasPassed = false;
            userCourse.NumTries = 0;
            userCourse.CompletedDate = DateTime.Now;

            _db.UserCourses.Add(userCourse);
            Save();

        }

        public question[] GetQuestionsFlat(int moduleId)
        {
            var moduleQuestions = GetQuestions(moduleId);

            var result = new List<question>();

            foreach (var moduleQuestion in moduleQuestions)
            {
                var q = new question() { alternatives = new alternative[2], completed = false, questionId = moduleQuestion.QuestionId };

                q.alternatives[0] = new alternative() { imgUrl = moduleQuestion.ImageUrl1, isCorrect = moduleQuestion.IsImage1Correct };
                q.alternatives[1] = new alternative() { imgUrl = moduleQuestion.ImageUrl2, isCorrect = moduleQuestion.IsImage2Correct };

                result.Add(q);
            }

            // Markerer siste spørsmål som endelig
            var lastQuestion = result.Last();
            lastQuestion.completed = true;

            return result.ToArray();

        }

    }

    public class question
    {
        public int questionId { get; set; }
        public bool completed { get; set; }
        public alternative[] alternatives { get; set; }
    }

    public class alternative
    {
        public string imgUrl { get; set; }
        public bool isCorrect { get; set; }
    }
}