using ParentAgent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net.Mail;
using ParentAgent.Helpers;

namespace ParentAgent.Controllers
{
    [Authorize(Roles = "omniscient, staff")]
    public class StaffController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //private List<Parent> theseParents;

        //public List<Parent> oParents {
        //    get {
        //        if (theseParents == null)
        //            theseParents = (List<Parent>)Session["oParents"];

        //        return theseParents;
        //    }
        //    set {
        //        theseParents = value;
        //        Session["oParents"] = theseParents;
        //    }
        //}


        [Route("{queryvalues:string}")]
        public ActionResult Index(string queryvalues = "")
        {
            ViewBag.Title = "Dashboard - Staff";

            string reportName = queryvalues;

            //-- Parent --
            List<Parent> oParents = new List<Parent>();
            List<QuestionHistory> oQuestionHistorys = new List<QuestionHistory>();

            if (reportName == "ViewParents_withNewQuestions")
                oParents = db.QuestionHistorys.Include(q => q.Parent).Where(q => q.Answer.Length == 0 || q.Answer == null).Select(q => q.Parent).Distinct().ToList();//oParents = db.Parents.Include(p => p.Children.Select(s => s.Courses.Select(c => c.QuestionHistorys.Count > 0))).ToList();

            else if (reportName == "ViewParents_Active") //ToDo: wwwsss Start using DataTables and combine reports where possible.
                oParents = db.Parents.Where(p => p.Active == 1).ToList();

            else if (reportName == "ViewParents_Inactive")
                oParents = db.Parents.Where(p => p.Active != 1).ToList();

            else if (reportName == "ViewParents_All")
                oParents = db.Parents.ToList();

            else if (reportName == "ViewParents_New")
            {
                DateTime rangeDate = DateTime.Now.AddDays(-7);
                oParents = db.Parents.Where(p => p.DateCreated > rangeDate).ToList();
            }
            else if (reportName == "ViewParents_neverAskedQuestions")
            {
                //oParents = db.Parents.Include(p => p.Children.Select(s => s.Courses.Select(c => c.QuestionHistorys))).ToList();
                oParents = db.Parents.Where(p => p.Children.Any(s => s.Courses.Any(c => c.QuestionHistorys.Any(q => q.Parent.ParentId != p.ParentId)))).ToList();//oParents = db.Parents.Include(p => p.Children.Select(s => s.Courses.Select(c => c.QuestionHistorys.Select(q => q.Answer.Length == 0)))).ToList();
            }
            else if (reportName == "ViewQuestions_Pending")
                oQuestionHistorys = db.QuestionHistorys.Include(q => q.Parent).Include(q => q.Student).Include(q => q.Course).Where(q => q.Answer.Length == 0 || q.Answer == null).OrderBy(q => q.Parent.LastName).ThenBy(q => q.Parent.FirstName).ThenBy(q => q.Student.FirstName).ThenBy(q => q.Course.Name).ThenByDescending(q => q.QuestionHistoryId).ToList();

            else if (reportName == "ViewQuestions_All")
                oQuestionHistorys = db.QuestionHistorys.Include(q => q.Parent).Include(q => q.Student).Include(q => q.Course).OrderBy(q => q.Parent.LastName).ThenBy(q => q.Parent.FirstName).ThenBy(q => q.Student.FirstName).ThenBy(q => q.Course.Name).ThenByDescending(q => q.QuestionHistoryId).ToList();


            ResponseReportsViewModel oRVM = new ResponseReportsViewModel()
            {
                NameOfReport = reportName,
                Parents = oParents,
                QuestionHistorys = oQuestionHistorys
            };

            return View(oRVM);
        }


        [Route("{queryvalues:int}")]
        public ActionResult Parent(int queryvalues)
        {
            ViewBag.Title = "Dashboard - Staff";

            int id = queryvalues;

            Parent oParent = db.Parents.SingleOrDefault(p => p.ParentId == id);
            List<QuestionHistory> oQuestionHistorysResponded = db.QuestionHistorys.Include(qh => qh.Student).Include(qh => qh.Course).Where(qh => qh.Parent.ParentId == id && !(qh.Answer == null || qh.Answer.Length == 0)).OrderBy(q => q.Parent.LastName).ThenBy(q => q.Parent.FirstName).ThenBy(q => q.Student.FirstName).ThenBy(q => q.Course.Name).ThenByDescending(q => q.QuestionHistoryId).ToList();
            List<QuestionHistory> oQuestionHistorysPending = db.QuestionHistorys.Include(qh => qh.Student).Include(qh => qh.Course).Where(qh => qh.Parent.ParentId == id && (qh.Answer == null || qh.Answer.Length == 0)).OrderBy(q => q.Parent.LastName).ThenBy(q => q.Parent.FirstName).ThenBy(q => q.Student.FirstName).ThenBy(q => q.Course.Name).ThenByDescending(q => q.QuestionHistoryId).ToList();

            ResponseQuestionsViewModel oRVM = new ResponseQuestionsViewModel()
            {
                Parent = oParent,
                QuestionHistoryVM = new QuestionHistoryViewModel(),
                QuestionHistorys = oQuestionHistorysResponded,
                QuestionHistorysPending = oQuestionHistorysPending
            };

            return View(oRVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendEmailsToTeachers(ResponseQuestionsViewModel oRVM)
        {
            TempData["UserMessage"] = new MessageVM() { IsSuccessful = false, Title = "Error!", Message = "Something went wrong." };

            if (!ModelState.IsValid)
                return View(oRVM);

            //-- Parent --
            Parent oParent = db.Parents
                .Include(p => p.Children.Select(s => s.Courses.Select(c => c.Teacher)))
                .Include(p => p.Children.Select(s => s.Courses.Select(c => c.QuestionHistorys)))
                .SingleOrDefault(p => p.ParentId == oRVM.QuestionHistoryVM.ParentId);

            //Parent oParent = db.Parents.Include(p => p.Children.Select(s => s.Courses.Select(c => c.Teacher))).SingleOrDefault(p => p.ParentId == oRVM.QuestionHistoryVM.ParentId);
            //Parent oParent = db.Parents.Include(p => p.Children.Select(s => s.Courses)).Select(p => new { Parent = p, Children = p.Children, Teacher = p.Children.Select(s => s.Courses.Select(c => c.Teacher)), QuestionHistorys = p.Children.Select(s => s.Courses.Select(c => c.QuestionHistorys)) }).SingleOrDefault(p => p.ParentId == oRVM.QuestionHistoryVM.ParentId);
            //Parent oParent = db.Parents.Include(p => p.Children.Select(s => s.Courses)).SingleOrDefault(p => p.ParentId == oRVM.Parent.ParentId);

            if (oParent == null) //Error. Parent DNE.
                throw new ArgumentException("SendEmailsToTeachers(ResponseQuestionsViewModel oRVM) - Parent DNE.");
            //-- end of Parent --

            try
            {
                string questions;
                int questionNumber;
                foreach (Student iStudent in oParent.Children)
                {
                    foreach (Course iCourse in iStudent.Courses)
                    {
                        if(iCourse.QuestionHistorys.Count > 0)
                        {
                            questions = "";
                            questionNumber = 1;

                            foreach (QuestionHistory iQuestionHistory in iCourse.QuestionHistorys)
                            {
                                questions += questionNumber++ + ". " + iQuestionHistory.QuestionTxt + "<br /><i>your answer here</i><br /><br />";
                            }

                            if(!SendEmail(oParent, iStudent, iCourse, questions))
                                return RedirectToAction("Parent", new { queryvalues = oParent.ParentId });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Parent", new { queryvalues = oParent.ParentId });
            }

            TempData["UserMessage"] = new MessageVM() { IsSuccessful = true, Title = "Success!", Message = "Your question(s) for this parent have been successfully sent." };

            return RedirectToAction("Parent", new { queryvalues = oParent.ParentId });
        }


        public bool SendEmail(Parent oParent, Student oStudent, Course oCourse, string Questions)
        {
            Teacher oTeacher = oCourse.Teacher;
            List<QuestionHistory> oQuestionHistorys = oCourse.QuestionHistorys;
            string body = "<p>Hi,</p>" +
                "<p>" + oParent.FullNameWithHonorific + " has asked us to contact you regarding " + oParent.PronounPossesion + " child " + oStudent.FirstName + " " + oStudent.LastName + ". Please reply to this email with answers to the question(s) below.</p>" +
                "<p>You can simply scroll down and place your answers below each question. If you prefer to talk over the phone, please reply to this email requesting a call.</p>" +
                "<p><h1>Questions</h1>" + Questions + "</p>";

            string footer = "<p><b>About Us</b>: Our group of professionals help parents and teachers communicate more effectively in order to improve children's performance in school.  We have a track record of helping students increase their grades and participation in school.  We absolutely love our communities and cannot wait to work with you!  You can find out more about us <a href='https://ParentAgent.com'>here</a>.</p>";

            body += footer;


            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("admin@parentagent.com");
            mailMessage.To.Add(new MailAddress(oTeacher.Email));
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = oStudent.FirstName + " " + oStudent.LastName + " - " + oCourse.Name + " - Parent Agent";
            mailMessage.Body = body;

            SmtpClient smtpClient = new SmtpClient();

            try
            {
                smtpClient.Send(mailMessage);

                foreach (QuestionHistory iQuestionHistory in oCourse.QuestionHistorys)
                {
                    iQuestionHistory.NumOfTimesSentToTeacher += 1;
                }

                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

    }
}
