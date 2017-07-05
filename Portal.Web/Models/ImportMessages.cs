using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Web.Models
{
    public class ImportMessages
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public DateTime RegisteredDate { get; set; }


        public ImportMessages(string message, bool error)
        {
            Message = message;
            Error = error;
            RegisteredDate = DateTime.Now;
        }
    }
}