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
    
    public partial class UserModulesWatched
    {
        public int UserModulesWatchedId { get; set; }
        public int UserProfileId { get; set; }
        public int ModuleId { get; set; }
    
        public virtual UserProfile UserProfile { get; set; }
        public virtual Module Module { get; set; }
    }
}
