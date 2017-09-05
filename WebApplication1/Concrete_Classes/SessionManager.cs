using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Concrete_Classes
{
    public class SessionManager
    {
        public static int UserId
        {
            get
            {
                if (HttpContext.Current.Session["UserId"] != null)
                    return int.Parse(HttpContext.Current.Session["UserId"].ToString());
                else
                    return 0;
            }
            set
            {
                HttpContext.Current.Session["UserId"] = value;
            }
        }

        public static int ContractorId
        {
            get
            {
                if (HttpContext.Current.Session["ContractorId"] != null)
                    return int.Parse(HttpContext.Current.Session["ContractorId"].ToString());
                else
                    return 0;
            }
            set
            {
                HttpContext.Current.Session["ContractorId"] = value;
            }
        }



        public static int CompanyId
        {
            get
            {
                if (HttpContext.Current.Session["CompanyId"] != null)
                    return int.Parse(HttpContext.Current.Session["CompanyId"].ToString());
                else
                    return 0;
            }
            set
            {
                HttpContext.Current.Session["CompanyId"] = value;
            }
        }

      
        public static int RoleId
        {
            get
            {
                if (HttpContext.Current.Session["RoleId"] != null)
                    return int.Parse(HttpContext.Current.Session["RoleId"].ToString());
                else
                    return 0;
            }
            set
            {
                HttpContext.Current.Session["RoleId"] = value;
            }
        }

        public static string UserName
        {
            get
            {
                if (HttpContext.Current.Session["UserName"] != null)
                    return HttpContext.Current.Session["UserName"].ToString();
                else
                    return string.Empty;
            }
            set
            {
                HttpContext.Current.Session["UserName"] = value;
            }
        }
    }
}