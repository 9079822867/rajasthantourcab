using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RajasthanTourCabN
{
    public static class CacheHelper
    {
        public static void ClearPagesCache()
        {
            HttpContext.Current.Cache.Remove("pages");
            HttpContext.Current.Cache.Remove("CabPricing");
        }

        public static void ClearMenuCache()
        {
            HttpContext.Current.Cache.Remove("adminmenu");
        }

        public static void ClearFooterCache()
        {
            HttpContext.Current.Cache.Remove("footer");
        }
    }
}