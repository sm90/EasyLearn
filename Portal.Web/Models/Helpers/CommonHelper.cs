using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Web.Models.Helpers
{
    public class CommonHelper
    {
        private  const int PasswordLenght = 6;
        private const int PasswordExtraSubmolsCount = 2;

        private const int KeyUserLenght = 8;

        public static string GeneretedPassword()
        {
            return System.Web.Security.Membership.GeneratePassword(PasswordLenght, PasswordExtraSubmolsCount);
        }

        public static string UserKey()
        {
            return System.Guid.NewGuid().ToString().Replace('-', '_').Substring(0, KeyUserLenght);
        }
    }
}