using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ParentAgent
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            //---------- Dashboard ----------
            routes.MapRoute(
                "StudentCourse",
                "{controller}/{action}/{StudentId}/{CourseId}",
                new { controller = "Dashboard", action = "Index" }
            );

            routes.MapRoute(
                "ParentStudentCourse",
                "{controller}/{action}/{ParentId}/{StudentId}/{CourseId}",
                new { controller = "Dashboard", action = "Index" }
            );
            //---------- end of Dashboard ----------


            //---------- Staff ----------
            routes.MapRoute(
                "StaffReportName",
                "Staff/{action}/{*queryvalues}",
                new { controller = "Staff", action = "Index" }
            );
            //{ ReportName}

            routes.MapRoute(
                "Staff",
                "Staff",
                new { controller = "Staff", action = "Index" }
            );
            //---------- end of Staff ----------


            //---------- Home ----------
            routes.MapRoute(
                "HomeOnlyAction",
                "{action}",
                new { controller = "Home", action = "Index" }
            );
            //---------- end Home ----------


            //routes.MapRoute(
            //    "Account",
            //    "Account/{action}",
            //    new { controller = "Account", action = "Index" }
            //);

            //routes.MapRoute(
            //    "Manage",
            //    "Manage/{action}",
            //    new { controller = "Manage", action = "Index" }
            //);


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
