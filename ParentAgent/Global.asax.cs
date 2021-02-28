using ParentAgent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ParentAgent
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //System.Web.Helpers.AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
        }

        //protected void Application_PreSendRequestHeaders()
        //{
        //    //Some say it should be in this function:  protected void Application_BeginRequest()
        //    Response.Headers.Remove("X-Frame-Options");
        //    Response.AddHeader("X-Frame-Options", "AllowAll");//
        //    //Response.AddHeader("X-Frame-Options", "ALLOW-FROM https://youtube.com/");//AllowAll
        //}

        protected void Application_Error()
        {
            Exception ex = Server.GetLastError();

            if(ex != null)
                ErrorLogService.LogError(ex);
        }


        //---------------------------------------- HELPER FUNCTIONS ----------------------------------------
        //common service to be used for logging errors
        private static class ErrorLogService
        {
            private static ApplicationDbContext db = new ApplicationDbContext();

            public static void LogError(Exception ex)
            {
                //----- Store to DB -----
                Error oError = new Error()
                {
                    Source = ex.Source != null ? ex.Source: "",
                    Message = ex.Message != null ? ex.Message: "",
                    InnerException = ex.InnerException != null ? ex.InnerException.ToString() : "",
                    StackTrace = ex.StackTrace != null ? ex.StackTrace : "",
                    Type = ex.GetType() != null ? ex.GetType().ToString() : "",
                    DateCreated = DateTime.Now
                };

                db.Errors.Add(oError);
                db.SaveChanges();
                //----- end of Store to DB -----

                //----- Email developers -----
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("no-reply@parentagent.com");
                SetRecipients(mailMessage);//.To
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = "Parent Agent - ERROR";
                mailMessage.Body = "<b>Date</b>: " + oError.DateCreated + "<br />" +
                    "<b>Source</b>: " + oError.Source + "<br />" +
                    "<b>Message</b>:<br />" +
                    oError.Message + "<br />" +
                    "<b>Inner Exception</b>:<br />" +
                    oError.InnerException + "<br />" +
                    "<b>StackTrace</b>:<br />" +
                    oError.StackTrace + "<br />" +
                    "<b>Type</b>: " + oError.Type;
                SmtpClient smtpClient = new SmtpClient();

                try
                {
                    smtpClient.Send(mailMessage);
                }
                catch (Exception)
                {

                }
                //----- end of Email developers -----
            }

            private static void SetRecipients(MailMessage mailMessage)
            {
                string emails_str = System.Web.Configuration.WebConfigurationManager.AppSettings["WilliesEmail"];
                emails_str = emails_str.Replace(" ", "");
                emails_str = emails_str.Replace(",", ";");
                List<string> emails_list = emails_str.Split(';').ToList<string>();
                foreach (string iEmail in emails_list)
                {
                    mailMessage.To.Add(new MailAddress(iEmail));
                }
            }
            //---------------------------------------- end of HELPER FUNCTIONS ----------------------------------------
        }
    }
}
