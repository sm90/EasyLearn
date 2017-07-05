using System;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace Portal.Web.Models.DataObject
{
    public class ComanyJsonDto
    {
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Contact { get; set; }

        public string StartVidio { get; set; }

        protected string category;

        public string Category
        {
            get { return category; }
        }

        public Nullable<CompanyType> SetCompanyType
        {
            set  { category = value.HasValue ? value.ToString() : "None"; }
        }
        protected string logo;

        public string Logo
        {
            get { return logo; }
            set
            {
                if(value != null)logo = JsonConvert.DeserializeObject<CustomizeCompany>(value).LogoUrl;
            }
        }

        public void SetLogo(string json)
        {
      }
        public int CompanyId { get; set; }

     


    }
}