using System;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Web.Models
{
    public class AjaxRepository
    {
        private Entities _db = new Entities();

        public List<GraphTimeLineModel> GetGraphTimeLineData(int courseId, int companyId)
        {
            List<GraphTimeLineModel> result = new List<GraphTimeLineModel>();

            DateTime fromDate = new DateTime(2014, 05, 01);

            // TODO: Filtrer på brukere som tilhører valgt firma
           // var exams =
           //     _db.Exams.Where(e => e.CourseId == courseId && e.Completed.HasValue && e.Completed.Value > fromDate && e.ResultScore >= e.PassScore).ToList();
            var exams =  (from e in _db.Exams
                         join user in  _db.UserProfiles on e.AspNetUserId equals user.AspNetUserId
                          where user.CompanyId == companyId && e.CourseId == courseId && e.Completed.HasValue && e.Completed.Value > fromDate && e.ResultScore >= e.PassScore
                         select e).ToList();
            DateTime date = new DateTime(2014, 05, 01);

            List<string> addedUsers = new List<string>();
            int passedExamCount = 0;

            while (date <= DateTime.Now)
            {
                var dayModel = new GraphTimeLineModel();
                dayModel.Key = date;

                var examThisDay = exams.Where(e => e.Completed.Value.Year == date.Year && e.Completed.Value.Month == date.Month && e.Completed.Value.Day == date.Day).ToList();

                foreach (var exam in examThisDay)
                {
                    if (!addedUsers.Contains(exam.AspNetUserId))
                    {
                        addedUsers.Add(exam.AspNetUserId);
                        passedExamCount++;
                    }

                }

                if (passedExamCount > 0)
                {
                    if (!result.Any())
                        result.Add(new GraphTimeLineModel() { Key = date.AddDays(-1), Value = 0 });

                    dayModel.Value = passedExamCount;
                    result.Add(dayModel);

                }
                date = date.AddDays(1);


            }

            return result;
        }

        public List<Module> GetModulesForCourse(int id)
        {
            return _db.Modules.Where(m => m.CourseId == id).OrderBy(m => m.ModuleId).ToList();
        }

        public void SetLastSeenModule(string username, int moduleId)
        {
            var user = _db.UserProfiles.FirstOrDefault(u => u.AspNetUser.UserName == username);

            var moduleWatched = user.UserModulesWatcheds.FirstOrDefault(m => m.ModuleId == moduleId);

            if (moduleWatched == null)
            {
                moduleWatched = new UserModulesWatched { ModuleId = moduleId, UserProfileId = user.UserProfileId };
                _db.UserModulesWatcheds.Add(moduleWatched);

                try
                {
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string test = ex.Message;
                }

            }
        }

        public int? GetLastSeenModuleId(string username)
        {
            var user = _db.UserProfiles.FirstOrDefault(u => u.AspNetUser.UserName == username);

            try
            {
                var userModulesWatched = _db.UserModulesWatcheds.OrderByDescending(w => w.UserModulesWatchedId).FirstOrDefault(w => w.UserProfileId == user.UserProfileId);

                if (userModulesWatched !=
                    null)
                    return userModulesWatched.ModuleId;
                return null;
            }
            catch (Exception ex)
            {
                string test = ex.Message;
                return 0;
            }

        }


        public List<CourseModuleAverageScore> GetModulesAverageScore(int courseId, int companyId)
        {
            return (from m in _db.ModuleQuestionCompanies
                    where m.CompanyId == companyId
                          && m.Module.CourseId == courseId
                    orderby m.Module.ModuleId
                    select new CourseModuleAverageScore()
                    {
                        CorrectNum = m.NumCorrect,
                        ModuleId = m.ModuleId,
                        WrongNum = m.NumWrong,
                        OrderNum = m.Module.OrderNum
                    }).ToList();
        }

        public List<CourseModuleStatistics> GetModuleStatistics(int courseId, int companyId)
        {
            var userAssigned = _db.UserCourses.Count(u => u.UserProfile.CompanyId == companyId);

            var result = (from m in _db.Modules
                          where m.CourseId == courseId
                          select new CourseModuleStatistics()
                          {
                              ModuleId = m.ModuleId,
                              UsersAssignedCount = userAssigned
                          }).ToList();

            var usersCompleted = from w in _db.UserModulesWatcheds
                                 where w.UserProfile.CompanyId == companyId
                                 select w;

            foreach (var module in result)
            {
                module.UsersCompleted = usersCompleted.Count(m => m.ModuleId == module.ModuleId);
            }

            return result;
        }
    }

    public class CourseModuleStatistics
    {
        public int ModuleId { get; set; }
        public int UsersCompleted { get; set; }
        public int UsersAssignedCount { get; set; }

        public int PercentCompleted
        {
            get
            {
                var result = (100 / UsersAssignedCount) * UsersCompleted;
                if (result == 99)
                    result = 100;
                return result;

            }
        }
    }

    public class CourseModuleAverageScore
    {
        public int OrderNum { get; set; }
        public int ModuleId { get; set; }
        public int CorrectNum { get; set; }
        public int WrongNum { get; set; }

        public int PercentCorrect
        {
            get
            {

                if (CorrectNum + WrongNum == 0)
                    return 100; // Fikk beskjed om at moduler uten spørsmål skulle vises med 100% score

                decimal resultDecimal = (decimal)100 / (CorrectNum + WrongNum);
                resultDecimal = resultDecimal * CorrectNum;

                int result = (int)Math.Ceiling(resultDecimal);

                if (result == 99)
                    result = 100;
                return result;

            }
        }
    }
}