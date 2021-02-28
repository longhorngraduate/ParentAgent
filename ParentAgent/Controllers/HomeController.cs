using ParentAgent.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ParentAgent.Controllers
{
    public class HomeController : Controller
    {
        [AllowCrossSiteYouTubeIFrame]
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Title = "Index";
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Title = "About";
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Title = "Contact";
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Send(string name, string phone, string email, string message)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("no-reply@parentagent.com");
            SetRecipients(mailMessage);//.To
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = "Parent Agent - Contact Us";
            mailMessage.Body = "<table><tr><td>Name</td><td>" + name + "</td></tr>" +
                "<tr><td>Phone</td><td>" + phone + "</td></tr>" +
                "<tr><td>Email</td><td>" + email + "</td></tr>" +
                "<tr><td>Message</td><td>" + message + "</td></tr>";
            SmtpClient smtpClient = new SmtpClient();
            
            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception)
            {
                return Content("Error");
            }

            return Content("OK");
        }

        private void SetRecipients(MailMessage mailMessage)
        {
            string emails_str = System.Web.Configuration.WebConfigurationManager.AppSettings["OurEmails"];
            emails_str = emails_str.Replace(" ", "");
            emails_str = emails_str.Replace(",", ";");
            List<string> emails_list = emails_str.Split(';').ToList<string>();
            foreach (string iEmail in emails_list)
            {
                mailMessage.To.Add(new MailAddress(iEmail));
            }
        }
    }
}