using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Web.Models.DataObject
{
    public class CustomizeCompany
    {
        public string LogoUrl { get; set; }
        public string LoginLogoUrl { get; set; }
        public string HeaderColor { get; set; }
      //  [Obsolete("Feild  is deprecated, please dont use it. Stored only to compatibility", false)]
     //   public string BackgroundColor { get; set; }
        public string MenuActiveColor { get; set; }
        public string MenuPassiveColor { get; set; }
        public string ButtonsBackgroundColor { get; set; }
        public string ButtonsActiveColor { get; set; }
        public string StartVidio { get; set; }

        public CustomizeCompany()
        {
        }

        public CustomizeCompany(string logoUrl, string loginLogoUrl, string headerColor, string backgroundColor, string menuActiveColor, string menuPassiveColor, string buttonsActiveColor, string buttonsBackgroundColor, string startVidio)
        {
            LogoUrl = logoUrl;
            LoginLogoUrl = loginLogoUrl;
            HeaderColor = headerColor;
          //  BackgroundColor = backgroundColor;
            MenuActiveColor = menuActiveColor;
            ButtonsActiveColor = buttonsActiveColor;
            MenuPassiveColor = menuPassiveColor;
            ButtonsBackgroundColor = buttonsBackgroundColor;
            StartVidio = startVidio;
        }



    }
}