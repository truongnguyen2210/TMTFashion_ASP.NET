using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopThoiTrang
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Session_Start()
        {
            Session["UserAdmin"] = "";
            Session["UserIdAdmin"] = "";
            Session["FullNameAdmin"] = "";
            Session["ImgAdmin"] = "";
            Session["EmailAdmin"] = "";
            Session["PhoneAdmin"] = "";



            Session["UserCustomer"] = "";
            Session["UserId"] = "";
            Session["FullName"] = "";
            Session["Img"] = "";
            Session["Email"] = "";
            Session["Phone"] = "";
        }
    }
}
