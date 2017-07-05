using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Web.Models
{
    public class UserCourseList
    {
        public string CourseName { get; set; }

        // Prosentvis hvor ferdig er brukeren med kurs (i forhold til antall moduler)
        public int CompletedPercent { get; set; }
        public int RequiredPassResult { get; set; }

        public bool Passed
        {
            get
            {
                if (ExamResult >= RequiredPassResult)
                    return true;
                return false;
            }
            set
            { }
        }

        public bool IsCompleted
        {
            get
            {
                if (CompletedPercent == 100)
                    return true;
                return false;
            }
        }

        public string CourseId { get; set; }

        public int _examResult = 0;

        /// <summary>
        /// Brukerens beste eksamensresultat
        /// </summary>
        public int ExamResult
        {
            get { return _examResult; }
            set { _examResult = value; }
        }
    }
} 