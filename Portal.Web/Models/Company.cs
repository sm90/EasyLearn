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
    
    public partial class Company
    {
        public Company()
        {
            this.Departments = new HashSet<Department>();
            this.ModuleQuestionCompanies = new HashSet<ModuleQuestionCompany>();
            this.UserProfiles = new HashSet<UserProfile>();
        }
    
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public string StylesheetUrl { get; set; }
        public string Domain { get; set; }
        public string OrgNummer { get; set; }
        public string BillinAddress { get; set; }
        public Nullable<int> ZipCode { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public string ContactName { get; set; }
        public string ContactSurname { get; set; }
        public string CompanyPhone { get; set; }
        public Nullable<CompanyType> CompanyType { get; set; }
        public string CustomizeCompany { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string ContactPhone { get; set; }
        public string TempPassword { get; set; }
        public Nullable<CompanyStatus> CompanyStatus { get; set; }
        public string AdminId { get; set; }
    
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<ModuleQuestionCompany> ModuleQuestionCompanies { get; set; }
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}
