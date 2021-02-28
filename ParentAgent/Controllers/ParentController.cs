using ParentAgent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using ParentAgent.Helpers;

namespace ParentAgent.Controllers
{
    public class ParentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Questions(int ParentId = 0, int StudentId = 0, int CourseId = 0)
        {
            ViewBag.Title = "Questions";

            if (Helpers.Helpers.IsStaff(User))
                return RedirectToAction("Index", "Staff");

            Course oCourse = db.Courses.SingleOrDefault(c => c.CourseId == CourseId);
            if (oCourse == null)//Error. Course DNE.
                throw new ArgumentException("Questions(int ParentId = 0, int StudentId = 0, int CourseId = 0) - Course DNE.");

            Student oStudent = db.Students.SingleOrDefault(s => s.StudentId == StudentId && s.Courses.Any(c => c.CourseId == oCourse.CourseId));
            if (oStudent == null)//Error. Student DNE OR doesn't take this Course.
                throw new ArgumentException("Questions(int ParentId = 0, int StudentId = 0, int CourseId = 0) - Student DNE.");

            Parent oParent = db.Parents.SingleOrDefault(p => p.ParentId == SessionSingleton.Current.ParentId && p.Children.Any(s => s.StudentId == oStudent.StudentId));
            if (oParent == null) //Error. This Student/Child doesn't belong to this Parent.
                throw new ArgumentException("Questions(int ParentId = 0, int StudentId = 0, int CourseId = 0) - Parent DNE.");

            List<QuestionHistory> oQuestionHistorys = db.QuestionHistorys.Include(q => q.Parent).Where(q => q.Course.CourseId == CourseId).OrderByDescending(q => q.DateAnswered).ThenByDescending(q => q.QuestionHistoryId).ToList();//.Include(q => q.Course)

            if (oQuestionHistorys == null)
                oQuestionHistorys = new List<QuestionHistory>();

            ResponseQuestionsViewModel RVM = new ResponseQuestionsViewModel()
            {
                QuestionHistorys = oQuestionHistorys,
                QuestionHistoryVM = new QuestionHistoryViewModel(),
                Parent = oParent,
                Student = oStudent,
                Course = oCourse
            };

            return View(RVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateQuestionHistory(ResponseQuestionsViewModel oRVM)
        {
            TempData["UserMessage"] = new MessageVM() { IsSuccessful = false, Title = "Error!", Message = "Something went wrong." };

            if (Helpers.Helpers.IsStaff(User))
                return RedirectToAction("Index", "Staff");

            if (!ModelState.IsValid)
                return View(oRVM);

            QuestionHistoryViewModel oQuestionHistoryVM = oRVM.QuestionHistoryVM;

            Course oCourse = db.Courses.Include(c => c.QuestionHistorys).SingleOrDefault(c => c.CourseId == oQuestionHistoryVM.CourseId);
            if (oCourse == null)//Error. Course DNE.
                throw new ArgumentException("CreateQuestionHistory(ResponseQuestionsViewModel oRVM) - Course DNE.");

            Student oStudent = db.Students.SingleOrDefault(s => s.StudentId == oQuestionHistoryVM.StudentId && s.Courses.Any(c => c.CourseId == oCourse.CourseId));
            if (oStudent == null)//Error. Student DNE OR doesn't take this Course.
                throw new ArgumentException("CreateQuestionHistory(ResponseQuestionsViewModel oRVM) - Student DNE.");

            Parent oParent = db.Parents.SingleOrDefault(p => p.ParentId == SessionSingleton.Current.ParentId && p.Children.Any(s => s.StudentId == oStudent.StudentId));
            if (oParent == null) //Error. This Student/Child doesn't belong to this Parent.
                throw new ArgumentException("CreateQuestionHistory(ResponseQuestionsViewModel oRVM) - Parent DNE.");

            QuestionHistory oQuestionHistory = new QuestionHistory()
            {
                //-- Default values --
                //-- end of Default values --

                //-- VM to Model --
                QuestionTxt = oQuestionHistoryVM.QuestionTxt,
                Parent = oParent,
                Student = oStudent,
                Course = oCourse
                //-- end of VM to Model --
            };

            //oParent.QuestionHistorys.Add(oQuestionHistory);
            //oStudent.QuestionHistorys.Add(oQuestionHistory);
            oCourse.QuestionHistorys.Add(oQuestionHistory);
            db.QuestionHistorys.Add(oQuestionHistory);

            try
            {
                if (db.SaveChanges() <= 0)
                    return View(oQuestionHistoryVM);
            }
            catch (Exception e)
            {
                return View(oQuestionHistoryVM);
            }

            TempData["UserMessage"] = new MessageVM() { IsSuccessful = true, Title = "Success!", Message = "Your question for this course has been successfully added." };

            return RedirectToAction("Questions", new { ParentId = oQuestionHistoryVM.ParentId, StudentId = oQuestionHistoryVM.StudentId, CourseId = oQuestionHistoryVM.CourseId });
        }


        //This is for Staffer use also.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditQuestionHistory(ResponseQuestionsViewModel oRVM)
        {
            TempData["UserMessage"] = new MessageVM() { IsSuccessful = false, Title = "Error!", Message = "Something went wrong." };

            //This is for Staffer use also so commented out:
            //if (Helpers.Helpers.IsStaff(User))
            //    return RedirectToAction("Index", "Staff");

            if (!ModelState.IsValid)
                return View(oRVM);

            QuestionHistoryViewModel oQuestionHistoryVM = oRVM.QuestionHistoryVM;

            Course oCourse = db.Courses.Include(c => c.QuestionHistorys).SingleOrDefault(c => c.CourseId == oQuestionHistoryVM.CourseId);
            if (oCourse == null)//Error. Course DNE.
                throw new ArgumentException("EditQuestionHistory(ResponseQuestionsViewModel oRVM) - Course DNE.");

            Student oStudent = db.Students.SingleOrDefault(s => s.StudentId == oQuestionHistoryVM.StudentId && s.Courses.Any(c => c.CourseId == oCourse.CourseId));
            if (oStudent == null)//Error. Student DNE OR doesn't take this Course.
                throw new ArgumentException("EditQuestionHistory(ResponseQuestionsViewModel oRVM) - Student DNE.");

            Parent oParent = db.Parents.SingleOrDefault(p => p.ParentId == oQuestionHistoryVM.ParentId && p.Children.Any(s => s.StudentId == oStudent.StudentId));
            if (oParent == null) //Error. This Student/Child doesn't belong to this Parent.
                throw new ArgumentException("EditQuestionHistory(ResponseQuestionsViewModel oRVM) - Parent DNE.");

            //-- QuestionHistory --
            QuestionHistory oQuestionHistory = db.QuestionHistorys.SingleOrDefault(qh => qh.QuestionHistoryId == oQuestionHistoryVM.QuestionHistoryId);
            if (oQuestionHistory == null) //Error. This Question DNE.
                throw new ArgumentException("EditQuestionHistory(ResponseQuestionsViewModel oRVM) - QuestionHistory DNE.");

            oQuestionHistory.QuestionTxt = oQuestionHistoryVM.QuestionTxt;
            //-- end of QuestionHistory --

            try
            {
                if (db.SaveChanges() <= 0)
                    return View(oQuestionHistoryVM);
            }
            catch (Exception e)
            {
                return View(oQuestionHistoryVM);
            }

            TempData["UserMessage"] = new MessageVM() { IsSuccessful = true, Title = "Success!", Message = "Your question for this course has been successfully updated." };

            return RedirectToAction("Parent", "Staff", new { queryvalues = oQuestionHistoryVM.ParentId });//, StudentId = oQuestionHistoryVM.StudentId, CourseId = oQuestionHistoryVM.CourseId
        }
    }
}