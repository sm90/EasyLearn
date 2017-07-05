using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Web.Models
{
    public class PasswordModel
    {
        /// <summary>
        /// Eksisterende passord
        /// </summary>
        public string Password0 { get; set; }

        /// <summary>
        /// Nytt passord
        /// </summary>
        public string Password1 { get; set; }

        /// <summary>
        /// Nytt passord igjen
        /// </summary>
        public string Password2 { get; set; }


    }
}