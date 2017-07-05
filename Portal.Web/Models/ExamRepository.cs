using Portal.Web.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Web.Models
{
    public class ExamRepository
    {
        private Entities _db = new Entities();

        public List<UserCourseList> GetCourseResultsForUser(string aspNetUserId)
        {
            // TODO: Fiks hardcodinger
            List<UserCourseList> result = new List<UserCourseList>();
            UserCourseList courseList = new UserCourseList();


            //int numModulesInCourse = 13;

            // Har vi et fullført kurs? Finn det med høyest score
            var exam =
                _db.Exams.Where(e => e.AspNetUserId == aspNetUserId && e.Completed.HasValue)
                    .OrderByDescending(e => e.ResultScore)
                    .FirstOrDefault();

            if (exam != null)
            {
                courseList.CompletedPercent = 100;
                courseList.CourseId = "1";
                courseList.CourseName = "HMS for bygg og anlegg";
                courseList.ExamResult = exam.ResultScore.Value;
                courseList.RequiredPassResult = 75;

            }
            else
            {
                // Finner kurset som er lengs fullført
                var user = _db.UserProfiles.FirstOrDefault(u => u.AspNetUser.Id == aspNetUserId);
                var userModulesWatched = _db.UserModulesWatcheds.OrderByDescending(w => w.UserModulesWatchedId).FirstOrDefault(w => w.UserProfileId == user.UserProfileId);

                if (userModulesWatched != null)
                {
                    courseList.CompletedPercent = Convert.ToInt32(((decimal)100 / 13) * userModulesWatched.ModuleId);
                }
                else
                {
                    courseList.CompletedPercent = 0;
                }

                if (courseList.CompletedPercent == 99)
                    courseList.CompletedPercent = 100;

                courseList.CourseId = "1";
                courseList.CourseName = "HMS for bygg og anlegg";
                courseList.RequiredPassResult = 75;

            }


            result.Add(courseList);
            return result;


        }

        public ExamQuestion StartExam(int courseId, string username)
        {
            Course course = _db.Courses.FirstOrDefault(c => c.CourseId == courseId);
            var user = _db.AspNetUsers.FirstOrDefault(u => u.UserName == username);

            var userProfile = _db.UserProfiles.FirstOrDefault(u => u.AspNetUserId == user.Id);

            var userCourse =
                _db.UserCourses.FirstOrDefault(c => c.UserProfileId == userProfile.UserProfileId && c.CourseId == courseId);

            if (userCourse == null)
            {
                userCourse = new UserCours();
                userCourse.UserProfileId = userProfile.UserProfileId;
                userCourse.CourseId = courseId;
                userCourse.Completed = false;
                userCourse.HasPassed = false;
                userCourse.NumTries = 1;
                userCourse.AllowedExam = false;

                _db.UserCourses.Add(userCourse);
                _db.SaveChanges();
            }
            else
            {
                userCourse.NumTries++;
                _db.SaveChanges();
            }

            // Setter opp en ny eksamen
            Exam exam = new Exam();
            exam.AspNetUserId = user.Id;
            exam.CourseId = courseId;
            exam.Started = DateTime.Now;
            exam.PassScore = course.RequiredPassScore;

            _db.Exams.Add(exam);
            _db.SaveChanges();

            List<ExamQuestion> examQuestions = new List<ExamQuestion>();

            foreach (var module in course.Modules)
            {
                foreach (var question in module.ModuleQuestions)
                {
                    ExamQuestion examQuestion = new ExamQuestion();

                    examQuestion.ExamId = exam.ExamId;
                    examQuestion.ModuleQuestionId = question.QuestionId;
                    examQuestion.Answered = false;

                    examQuestions.Add(examQuestion);
                }
            }

            if (course.RandomizeQuestionOrder)
            {
                examQuestions.Shuffle();

                for (int i = 0; i < examQuestions.Count; i++)
                {
                    examQuestions[i].SortOrder = i;
                }
            }

            _db.ExamQuestions.AddRange(examQuestions);
            _db.SaveChanges();

            return examQuestions[0];
        }

        public ExamQuestion GetCurrentExamQuestion(string username)
        {
            var user = _db.AspNetUsers.FirstOrDefault(u => u.UserName == username);

            // Henter siste eksamen for brukeren
            var exam = user.Exams.LastOrDefault();

            // Henter første ubesvarte spørsmål
            return exam.ExamQuestions.Where(e => !e.Answered).OrderBy(e => e.SortOrder).FirstOrDefault();

        }

        public ExamQuestion AnswerExamQuestion(int answer, string username)
        {
            // HEnt siste spørsmål fra brukeren
            var question = GetCurrentExamQuestion(username);
            // Lagre resultat
            if (answer == 0 && question.ModuleQuestion.IsImage1Correct)
            {
                question.WasCorrect = true;

            }
            else if (answer == 1 && question.ModuleQuestion.IsImage2Correct)
            {
                question.WasCorrect = true;

            }
            else
            {
                question.WasCorrect = false;
            }

            question.Answered = true;
            _db.SaveChanges();

            // Oppdaterer ModuleQuestionCompany som blir brukt av statistikken
            var userProfile = GetUser(username).UserProfiles.FirstOrDefault();
            UpdateModuleQuestionCompany(userProfile.CompanyId, question.ModuleQuestion.ModuleId, question.WasCorrect.Value);

            // Hent neste spørsmål eller returner null

            var nextQuestion = GetCurrentExamQuestion(username);

            if (nextQuestion == null)
            {
                ExamCompleted(question.ExamId, username);
            }

            return nextQuestion;
        }

        public void UpdateModuleQuestionCompany(int companyId, int moduleId, bool wasCorrect)
        {
            var mqc =
                _db.ModuleQuestionCompanies.FirstOrDefault(m => m.CompanyId == companyId && m.ModuleId == moduleId);

            if (mqc == null)
            {
                mqc = new ModuleQuestionCompany();
                mqc.CompanyId = companyId;
                mqc.ModuleId = moduleId;
                mqc.NumCorrect = 0;
                mqc.NumWrong = 0;
                _db.ModuleQuestionCompanies.Add(mqc);
                _db.SaveChanges();
            }

            if (wasCorrect)
                mqc.NumCorrect++;
            else
            {
                mqc.NumWrong++;
            }

            _db.SaveChanges();
        }


        public bool IsExamStarted(int courseId, int userId)
        {
            return true;
        }

        public bool IsExamCompleted(int courseId, int userId)
        {
            return false;
        }

        /// <summary>
        /// Sjekker om samtlige spørsmål til siste eksamen er besvart
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsAllQuestionAnswered(int courseId, int userId)
        {
            return false;
        }


        /// <summary>
        /// Sjekker rom en bruker har en pågående eksamen
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool HasActiveExam(int courseId, int userId)
        {
            if (IsExamStarted(courseId, userId) && !IsExamCompleted(courseId, userId))
                return true;
            return false;
        }

        private void ExamCompleted(int examId, string username)
        {
            var exam = GetExam(examId);

            exam.Completed = DateTime.Now;

            int totaltQuestions = exam.ExamQuestions.Count();
            int correctQuestions =
                exam.ExamQuestions.Count(q => q.Answered && q.WasCorrect.HasValue && q.WasCorrect.Value);

            int result = 0;
            if(totaltQuestions == correctQuestions)
            {
                result = 100;
            }
            else if (correctQuestions > 0)
            {
                decimal resultDecimal = 100 / (decimal)totaltQuestions;
                resultDecimal = resultDecimal * correctQuestions;
                result = (int)Math.Ceiling(resultDecimal);
            }

            exam.ResultScore = result;

            var userCourse =
                _db.UserCourses.FirstOrDefault(u => u.CourseId == exam.CourseId && u.UserProfile.AspNetUser.UserName == username);

            if (userCourse.MaxUserScore == null || userCourse.MaxUserScore < exam.ResultScore)
            {
                userCourse.MaxUserScore = exam.ResultScore.Value;
                userCourse.CompletedDate = DateTime.Now;

                if (exam.ResultScore >= exam.PassScore)
                    userCourse.HasPassed = true;
            }

            _db.SaveChanges();

        }


        public AspNetUser GetUser(string username)
        {
            return _db.AspNetUsers.FirstOrDefault(u => u.UserName == username);
        }

        public Exam GetExam(int id)
        {
            return _db.Exams.FirstOrDefault(e => e.ExamId == id);
        }

        /// <summary>
        /// Returnerer en brukers siste eksamen
        /// </summary>
        /// <param name="aspNetUserId"></param>
        /// <returns></returns>
        public Exam GetUsersLastExam(string aspNetUserId)
        {
            return
                _db.Exams.Where(u => u.AspNetUserId == aspNetUserId && u.Completed.HasValue)
                    .OrderByDescending(u => u.ExamId)
                    .FirstOrDefault();
        }

        public bool HasCompletedAllModules(int courseId, string username)
        {
            Course course = _db.Courses.FirstOrDefault(c => c.CourseId == courseId);
            var user = _db.AspNetUsers.FirstOrDefault(u => u.UserName == username);

            var lastModuleWatched =
                _db.UserModulesWatcheds.Where(m => m.UserProfile.AspNetUserId == user.Id)
                    .OrderByDescending(m => m.ModuleId)
                    .Take(1).FirstOrDefault();

            if (lastModuleWatched == null)
                return false;

            var lastModuleInCourse =
                course.Modules.Where(m => m.CourseId == courseId).OrderByDescending(m => m.ModuleId).Take(1).FirstOrDefault();

            if (lastModuleInCourse.ModuleId == lastModuleWatched.ModuleId)
                return true;

            return false;

        }

        /// <summary>
        /// Returnerer en brukers første eksamensresultat som er bestått
        /// </summary>
        /// <param name="aspNetUserId"></param>
        /// <returns></returns>
        public Exam GetUsersFirstPassedExam(string aspNetUserId)
        {
            return _db.Exams.Where(u => u.AspNetUserId == aspNetUserId && u.Completed.HasValue && u.ResultScore >= u.PassScore)
                .OrderBy(u => u.ExamId).FirstOrDefault();
        }
    }

    public enum QuestionAnswer
    {
        ImageLeft,
        ImageRight
    }
}