//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Portal.Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Exam
    {
        public Exam()
        {
            this.ExamQuestions = new HashSet<ExamQuestion>();
        }
    
        public int ExamId { get; set; }
        public string AspNetUserId { get; set; }
        public int CourseId { get; set; }
        public System.DateTime Started { get; set; }
        public Nullable<System.DateTime> Completed { get; set; }
        public int PassScore { get; set; }
        public Nullable<int> ResultScore { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual ICollection<ExamQuestion> ExamQuestions { get; set; }
        public virtual Course Cours { get; set; }
    }
}
