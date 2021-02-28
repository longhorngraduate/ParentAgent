using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParentAgent.DataAnnotations
{
    public class AllowCrossSiteYouTubeIFrameAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //filterContext.HttpContext.Response.Headers.Remove("X-Frame-Options");
            ////filterContext.HttpContext.Response.AddHeader("X-Frame-Options", "DENY");
            //filterContext.HttpContext.Response.AddHeader("X-Frame-Options", "ALLOWALL");
            ////filterContext.HttpContext.Response.AddHeader("X-Frame-Options", "ALLOW-FROM https://www.youtube.com/");

            filterContext.HttpContext.Response.Headers.Remove("x-frame-options");
            filterContext.HttpContext.Response.AddHeader("x-frame-options", "DENY");//ALLOW-FROM https://www.youtube.com/

            base.OnResultExecuted(filterContext);
        }
    }
}