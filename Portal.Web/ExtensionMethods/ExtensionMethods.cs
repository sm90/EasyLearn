﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Web.ExtensionMethods
{
    public static class ExtensionMethods
    {
        // http://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}