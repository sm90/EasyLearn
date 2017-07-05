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
    
    public partial class Module
    {
        public Module()
        {
            this.ModuleQuestionCompanies = new HashSet<ModuleQuestionCompany>();
            this.ModuleQuestions = new HashSet<ModuleQuestion>();
            this.UserModulesWatcheds = new HashSet<UserModulesWatched>();
        }
    
        public int ModuleId { get; set; }
        public int CourseId { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbUrl { get; set; }
        public int OrderNum { get; set; }
        public string Title { get; set; }
    
        public virtual Course Cours { get; set; }
        public virtual ICollection<ModuleQuestionCompany> ModuleQuestionCompanies { get; set; }
        public virtual ICollection<ModuleQuestion> ModuleQuestions { get; set; }
        public virtual ICollection<UserModulesWatched> UserModulesWatcheds { get; set; }
    }
}
