using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Web.Models
{
    public class StatUsersModel
    {
       
        public string Icon
        {
            get
            {
                if (NumTries == 0)
                    return "";
                if (HasPassed.HasValue && HasPassed.Value)
                    return "Ja";
                else return "Nei";
            }
        }

        public string Department { get; set; }

        public DateTime? PassDate { get; set; }

        public string PassDateDisplay
        {
            get
            {
                if (!HasPassed.HasValue || !HasPassed.Value)
                    return "";
                if (!PassDate.HasValue)
                    return "";
                return PassDate.Value.ToShortDateString();
            }
        }

        public bool? HasPassed { get; set; }

        public string Name
        {
            get { return String.Format("{0} {1}", Firstname, Lastname); }
            set { }
        }

        public string LastLoginDateDisplay
        {
            get
            {
                if (!LastLoginDate.HasValue)
                    return "";
                return LastLoginDate.Value.ToShortDateString();
            }
        }

        public DateTime? LastLoginDate { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public string AspNetUserId { get; set; }
        public int? NumTries { get; set; }
        public int PercentResult { get; set; }
        
        public string PercentText { get {
            if (NumTries == 0)
                return "";
            return PercentResult.ToString() + "%"; } }
    }

}