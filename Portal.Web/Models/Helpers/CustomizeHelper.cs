using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Portal.Web.Models;
using Portal.Web.Models.DataObject;

namespace Portal.Web.Models.Helpers
{
    public class CustomizeHelper
    {
        public const string sessionMarcker = "isCustomize";
        public static readonly CustomizeCompany deff = new CustomizeCompany()
        {
            //BackgroundColor = "transparent",
            ButtonsBackgroundColor = "#ea7235",
            ButtonsActiveColor = "#000000",
            HeaderColor = "#f5f5f5",
            MenuActiveColor = "#000000",
            MenuPassiveColor = "#717b94",
            StartVidio = @"//player.vimeo.com/video/83791193?api=1&player_id=vimeoplayer",
            LoginLogoUrl = "/Content/logo_logginn.jpg",
            LogoUrl = "/Content/easylearn_logo.png"
        };

        public static bool checkSession(HttpSessionStateBase session)
        {
            return session[sessionMarcker] == null;
        }

        public static void SetSession(HttpSessionStateBase session, Company company)
        {

            var customize = company.CustomizeCompany != null
                ? JsonConvert.DeserializeObject<CustomizeCompany>(company.CustomizeCompany)
                : new CustomizeCompany();
            
            session["logoUrl"] = customize.LogoUrl ?? deff.LogoUrl;
            session["loginLogoUrl"] = customize.LoginLogoUrl ?? deff.LoginLogoUrl;
           // session["backgroundColor"] = (customize.BackgroundColor == null || customize.BackgroundColor == "") ? deff.BackgroundColor : customize.BackgroundColor;
            //session["backgroundColor"] = customize.BackgroundColor ?? deff.BackgroundColor;
            //session["buttonsBackgroundColor"] = customize.ButtonsBackgroundColor ?? deff.ButtonsBackgroundColor;
            session["menuActiveColor"] = (customize.MenuActiveColor == null || customize.MenuActiveColor == "") ? deff.MenuActiveColor : customize.MenuActiveColor;
            session["buttonsActiveColor"] = (customize.ButtonsActiveColor == null || customize.ButtonsActiveColor == "") ? deff.ButtonsActiveColor : customize.ButtonsActiveColor;
            session["menuPassiveColor"] = (customize.MenuPassiveColor == null || customize.MenuPassiveColor == "") ? deff.MenuPassiveColor : customize.MenuPassiveColor;
            session["buttonsBackgroundColor"] = (customize.ButtonsBackgroundColor == null || customize.ButtonsBackgroundColor == "") ? deff.ButtonsBackgroundColor : customize.ButtonsBackgroundColor;
            //session["headerColor"] = customize.HeaderColor ?? deff.HeaderColor;
            session["headerColor"] = (customize.HeaderColor == null || customize.HeaderColor == "") ? deff.HeaderColor : customize.HeaderColor;
            Uri temp;
            session["startVidio"] = Uri.TryCreate(customize.StartVidio, UriKind.Absolute, out temp) ? customize.StartVidio : deff.StartVidio;
   
        
            session[sessionMarcker] = true;
        }
    }
}