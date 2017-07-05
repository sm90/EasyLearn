using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Portal.Web.Models
{
    public class StatisticsRepository
    {
        private Entities _db = new Entities();

        #region Course

        /// <summary>
        /// Returnerer antallet brukere som er blitt tildelt kurset.
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public int UsersAssignedToCourseCount(int courseId, int companyId)
        {
            return 1;
        }

        /// <summary>
        /// Returnerer summen av alle brukernes kurs som er tildelt.
        /// Eksempel på et firma med 2 brukere:
        ///     - Ole er tildelt 3 kurs
        ///     - Lise er tildelt 1 kurs
        /// Metoden vil da returnere 4, siden målet da er at 4 kurs skal bestås.
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public int UsersAssignedToCoursesCount(int companyId)
        {
            return _db.UserCourses.Count(u => u.UserProfile.CompanyId == companyId);

        }

        /// <summary>
        /// Returnerer antallet brukere som har startet på et kurs (dvs. startet på modul 1)
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public int UsersStartedCourseCount(int courseId, int companyId)
        {
            return -1; //return _db.UserCourses.Count(u => u.UserProfile.CompanyId == companyI );
        }

        /// <summary>
        /// Returnerer summen av alle kurs som brukerne har startet på.
        /// Eksempel på et firma med 2 brukere:
        ///     - Ole er tildelt 3 kurs og har startet på 1.
        ///     - Lise er tildelt 1 kurs og har startet på 0.
        /// Metoden vil da returnere 1;
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public int UsersStartedCoursesCount(int companyId)
        {
            return (from u in _db.UserProfiles
                    from w in _db.UserModulesWatcheds
                    where u.UserProfileId == w.UserProfileId && u.CompanyId == companyId
                    select u.UserProfileId).Distinct().Count();

        }

        /// <summary>
        /// Returnerer antallet brukere som har fullført et kurs (dvs. fullført og bestått eksamen)
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public int UsersCompletedCourseCount(int courseId, int companyId)
        {
            return 0;
        }

        /// <summary>
        /// Returnerer summe av alle kurs som brukeren har bestått eksamen på.
        /// Eksempel på et firma med 2 brukere:
        ///     - Ole er tildelt 3 kurs, har startet på 2 kurs og bestått 1 eksamen
        ///     - Lise er tildelt 1 kurs, har startet på 0 kurs og bestått 0 eksamen
        /// Metoden vil da returnere 0
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public int UsersCompletedCoursesCount(int companyId)
        {
            return (from u in _db.UserProfiles
                    from e in _db.Exams
                    where u.CompanyId == companyId
                          && e.AspNetUser.Id == u.AspNetUserId
                          && e.Completed.HasValue
                          && e.ResultScore >= e.PassScore
                    select u.AspNetUserId).Distinct().Count();
        }

        /// <summary>
        /// Returnerer en brukers beste kursresultat med prosenter fordelt per modul
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="courseId"></param>
        /// <returns>Returnerer en dictionary med kursmodul - prosentscore. Prosentscore er 0 dersom brukeren ikke har svart på spørsmålene tilhørende en modul</returns>
        public Dictionary<int, int> GetUsersScoreForCourse(int companyId, int courseId)
        {
            Dictionary<int, int> result = new Dictionary<int, int>();

            result.Add(1, 83);
            result.Add(2, 74);
            result.Add(3, 93);
            result.Add(4, 43);
            result.Add(5, 88);
            result.Add(6, 75);
            result.Add(7, 66);

            return result;
        }


        #endregion Course

        #region User

        /// <summary>
        /// Sjekker om en bruker har bestått eksamen for et kurs
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public bool HasUserPassedCourse(int userId, int courseId)
        {
            return false;
        }

        /// <summary>
        /// Sjekker om en bruker har startet på et kurs. (dvs. startet på å se modul 1)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public bool HasUsersStartedCourse(int userId, int courseId)
        {
            return true;
        }

        /// <summary>
        /// Returnerer en brukers beste kursresultat med prosenter fordelt per modul
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <returns>Returnerer en dictionary med kursmodul - prosentscore. Prosentscore er 0 dersom brukeren ikke har svart på spørsmålene tilhørende en modul</returns>
        public List<BarGraphModel> GetUserScoreForCourse(string aspNetUserId, int courseId)
        {
            var result = new List<BarGraphModel>();
            // Finner brukerens beste resultat:
            var exam =
                _db.Exams.Where(e => e.AspNetUserId == aspNetUserId && e.CourseId == courseId && e.Completed.HasValue)
                    .OrderByDescending(e => e.ResultScore)
                    .FirstOrDefault();

            if (exam == null)
                return null;


            foreach (var module in exam.Cours.Modules)
            {
                var bar = new BarGraphModel();
                bar.Text = module.Title;
                bar.Value = 0;

                var answers = exam.ExamQuestions.Where(q => q.ModuleQuestion.ModuleId == module.ModuleId);

                int totalNumQuestions = 0;
                int totaltAnsweredCorrect = 0;

                foreach (var answer in answers)
                {
                    totalNumQuestions++;
                    if (answer.WasCorrect.HasValue && answer.WasCorrect.Value)
                        totaltAnsweredCorrect++;
                }

                if (totalNumQuestions > 0)
                {
                    bar.Value = (int)((decimal)100 / totalNumQuestions) * totaltAnsweredCorrect;
                }

                if (totalNumQuestions > 0) // Tar ikke med moduler uten spørsmål
                    result.Add(bar);

            }


            return result;
        }

        public List<StatUsersModel> GetStatUsers(int companyId, int courseId)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("nb-NO");

            var result = (from uc in _db.UserCourses
                          from u in _db.UserProfiles
                          where u.UserProfileId == uc.UserProfileId
                                && uc.UserProfile.CompanyId == companyId
                                && uc.CourseId == courseId
                          select new StatUsersModel()
                          {
                              Firstname = u.Firstname,
                              Lastname = u.Lastname,
                              NumTries = uc.NumTries,
                              PassDate = uc.CompletedDate,
                              Username = u.AspNetUser.UserName,
                              PercentResult = uc.MaxUserScore,
                              HasPassed = uc.HasPassed,
                              Department = u.Department.DepartmentName,
                              LastLoginDate = u.LastLoginDate,
                              AspNetUserId = u.AspNetUserId

                          }).ToList();

            return result;
        }
        #endregion User
    }
}