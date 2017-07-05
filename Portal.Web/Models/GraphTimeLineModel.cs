using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Web.Models
{
    public class GraphTimeLineModel
    {
        public DateTime Key { get; set; }
        public int Value { get; set; }

        public int Day
        {
            get { return Key.Day; }
        }

        public int Month
        {
            get { return Key.Month; }
        }

        public int Year
        {
            get { return Key.Year; }
        }
    }
}