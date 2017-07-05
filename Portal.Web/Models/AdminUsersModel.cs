using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Web.Models
{
    public class AdminUsersModel
    {
        private string _username;

        public string Username
        {
            get
            {
                if (String.IsNullOrEmpty(_username))
                    return "#";
                return _username;
            }
            set { _username = value; }
        }
        public string AspNetUserId;
        public string Name
        {
            get { return String.Format("{0} {1}", Firstname, Lastname); }
            set { }
        }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Department { get; set; }

        public bool Course1 { get; set; }
        public bool Course2 { get; set; }
        public bool Course3 { get; set; }
       
    }
}