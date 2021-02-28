using ParentAgent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Globalization;
using System.Security.Claims;
using Microsoft.Owin.Security;
using ParentAgent.Helpers;
using System.Security.Principal;

namespace ParentAgent.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Dashboard
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Index(int StudentId = 0, int CourseId = 0)
        {
            ViewBag.Title = "Dashboard";

            if (Helpers.Helpers.IsStaff(User))
                return RedirectToAction("Index", "Staff");

            //-- Parent --
            Parent oParent = SessionSingleton.Current.oParent;

            if (oParent == null) //This is a new parent.
                return RedirectToAction("NewParent");

            oParent.LastConnect = DateTime.Now;
            db.SaveChanges();
            //-- end of Parent --

            Parent currParent = db.Parents.Include(p => p.Children).SingleOrDefault(p => p.ParentId == oParent.ParentId);//User.Identity.Name //-- Parent --
            Student currStudent = GetStudentOrDefault(StudentId);//-- Student --
            Course currCourse = GetCourseOrDefault(CourseId, currStudent.StudentId);//-- Course --
            Teacher currTeacher = currCourse.Teacher;//-- Teacher --

            ResponseMainViewModel oRVM = new ResponseMainViewModel()
            {
                Parent = currParent,
                Student = currStudent,
                Course = currCourse,
                Teacher = currTeacher
            };

            ViewBag.Message = "Welcome back " + oParent.FirstName;

            return View(oRVM);
        }


        #region Parent

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult NewParent()
        {
            ViewBag.Title = "New Parent";

            if (Helpers.Helpers.IsStaff(User))
                return RedirectToAction("Index", "Staff");

            if (SessionSingleton.Current.oParent != null)
                throw new ArgumentException("NewParent() - Parent already exists.");

            ParentViewModel oParentVM = new ParentViewModel();

            //-- Add Parent's default values --
            oParentVM.Username = User.Identity.Name;
            oParentVM.Email = User.Identity.Name;
            //-- end of Add Parent's default values --

            return View(oParentVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateParent(ParentViewModel oParentVM)
        {
            TempData["UserMessage"] = new MessageVM() { IsSuccessful = false, Title = "Error!", Message = "Something went wrong." };

            if (Helpers.Helpers.IsStaff(User))
                return RedirectToAction("Index", "Staff");

            if (!ModelState.IsValid || SessionSingleton.Current.oParent != null)
                return View(oParentVM);//Incorrect data OR Parent already exists

            Parent oParent = new Parent()
            {
                //-- Default values --
                Username = User.Identity.Name,
                Email = User.Identity.Name,
                Active = 1,
                DateCreated = DateTime.Now,
                LastConnect = DateTime.Now,
                //-- end of Default values --

                //-- VM to Model --
                FirstName = oParentVM.FirstName,
                LastName = oParentVM.LastName,
                Gender = oParentVM.Gender,
                Phone = oParentVM.Phone,
                Street1 = oParentVM.Street1,
                Street2 = oParentVM.Street2,
                City = oParentVM.City,
                State = oParentVM.State,
                Zip = oParentVM.Zip,
                NumOfChildren = oParentVM.NumOfChildren,
                Children = new List<Student>()
                //-- end of VM to Model --
            };

            db.Parents.Add(oParent);

            try
            {
                if (db.SaveChanges() <= 0)
                    return View(oParent);
            }
            catch (Exception e)
            {
                return View(oParent);
            }

            //----- Set Session Variable -----
            SessionSingleton.Current.ParentId = oParent.ParentId;
            SessionSingleton.Current.oParent = oParent;
            //----- end of Set Session Variable -----

            TempData["UserMessage"] = new MessageVM() { IsSuccessful = true, Title = "Success!", Message = "Welcome to our team." };

            return RedirectToAction("Index", new { StudentId = 0, CourseId = 0 });
        }
        #endregion


        #region Student

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult NewStudent()
        {
            ViewBag.Title = "New Student";

            if (Helpers.Helpers.IsStaff(User))
                return RedirectToAction("Index", "Staff");

            Parent oParent = SessionSingleton.Current.oParent;
            if (oParent == null) //Error. Parent DNE.
                throw new ArgumentException("NewStudent() - Parent DNE.");

            StudentViewModel oStudentVM = new StudentViewModel();

            //-- Add Student's default values --
            oStudentVM.Username = User.Identity.Name;
            oStudentVM.Email = User.Identity.Name;
            //-- end of Add Student's default values --

            return View(oStudentVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateStudent(StudentViewModel oStudentVM)
        {
            TempData["UserMessage"] = new MessageVM() { IsSuccessful = false, Title = "Error!", Message = "Something went wrong." };

            if (Helpers.Helpers.IsStaff(User))
                return RedirectToAction("Index", "Staff");

            if (!ModelState.IsValid)
                return View(oStudentVM);

            Parent oParent = db.Parents.Include("Children").SingleOrDefault(p => p.ParentId == SessionSingleton.Current.ParentId);
            if (oParent == null) //Error. Parent DNE.
                throw new ArgumentException("CreateStudent(StudentViewModel oStudentVM) - Parent DNE");

            Student oStudent = new Student()
            {
                //-- Default values --
                Username = User.Identity.Name,
                Email = User.Identity.Name,
                Active = 1,
                DateCreated = DateTime.Now,
                LastActive = DateTime.Now,
                //-- end of Default values --

                //-- VM to Model --
                FirstName = oStudentVM.FirstName,
                LastName = oStudentVM.LastName,
                Gender = oStudentVM.Gender,
                Phone = oStudentVM.Phone,
                Courses = new List<Course>(),
                Teachers = new List<Teacher>(),
                //WeeklyQuestions = new List<QuestionHistory>(),
                //QuestionHistorys = new List<QuestionHistory>()
                //-- end of VM to Model --
            };

            oParent.Children.Add(oStudent);//Add Student to Parent

            try
            {
                if (db.SaveChanges() <= 0)
                    return View(oStudent);
            }
            catch (Exception e)
            {
                return View(oStudent);
            }

            TempData["UserMessage"] = new MessageVM() { IsSuccessful = true, Title = "Success!", Message = "Your child has been successfully added." };

            return RedirectToAction("Index", new { StudentId = oStudent.StudentId, CourseId = 0 });
        }


        public ActionResult EditStudent(int id)
        {
            ViewBag.Title = "Edit Student";

            if (Helpers.Helpers.IsStaff(User))
                return RedirectToAction("Index", "Staff");

            Student oStudent = db.Students.SingleOrDefault(s => s.StudentId == id);
            if (oStudent == null)//Error. Student DNE.
                throw new ArgumentException("EditStudent(int id) - Student DNE.");

            Parent oParent = db.Parents.Include(p => p.Children).SingleOrDefault(p => p.ParentId == SessionSingleton.Current.ParentId && p.Children.Any(s => s.StudentId == oStudent.StudentId));
            if (oParent == null) //Error. This Student/Child doesn't belong to this Parent.
                throw new ArgumentException("EditStudent(int id) - Parent DNE.");

            StudentViewModel oStudentVM = new StudentViewModel()
            {
                StudentId = oStudent.StudentId,
                Username = oStudent.Username,
                Email = oStudent.Email,
                FirstName = oStudent.FirstName,
                LastName = oStudent.LastName,
                Gender = oStudent.Gender,
                Phone = oStudent.Phone
            };

            return View(oStudentVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AlterStudent(StudentViewModel oStudentVM)
        {
            TempData["UserMessage"] = new MessageVM() { IsSuccessful = false, Title = "Error!", Message = "Something went wrong." };

            if (Helpers.Helpers.IsStaff(User))
                return RedirectToAction("Index", "Staff");

            if (!ModelState.IsValid)
                return View(oStudentVM);

            Student oStudent = db.Students.SingleOrDefault(s => s.StudentId == oStudentVM.StudentId);
            if (oStudent == null)//Error. Student DNE.
                throw new ArgumentException("AlterStudent(StudentViewModel oStudentVM) - Student DNE.");

            Parent oParent = db.Parents.Include(p => p.Children).SingleOrDefault(p => p.ParentId == SessionSingleton.Current.ParentId && p.Children.Any(s => s.StudentId == oStudent.StudentId));
            if (oParent == null) //Error. This Student/Child doesn't belong to this Parent.
                throw new ArgumentException("AlterStudent(StudentViewModel oStudentVM) - Parent DNE.");

            oStudent.StudentId = oStudentVM.StudentId;
            oStudent.Username = oStudentVM.Username;
            oStudent.Email = oStudentVM.Email;
            oStudent.FirstName = oStudentVM.FirstName;
            oStudent.LastName = oStudentVM.LastName;
            oStudent.Gender = oStudentVM.Gender;
            oStudent.Phone = oStudentVM.Phone;
            oStudent.LastActive = DateTime.Now;

            try
            {
                //if (db.SaveChanges() <= 0)
                //    return View(oStudentVM);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return View(oStudentVM);
            }

            TempData["UserMessage"] = new MessageVM() { IsSuccessful = true, Title = "Success!", Message = "Your child has been successfully updated." };

            return RedirectToAction("Index", new { StudentId = oStudent.StudentId, CourseId = 0 });
        }
        #endregion


        #region Course

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult NewCourse(int id)
        {
            ViewBag.Title = "New Course";

            if (Helpers.Helpers.IsStaff(User))
                return RedirectToAction("Index", "Staff");

            Student oStudent = db.Students.SingleOrDefault(s => s.StudentId == id);
            if (oStudent == null)//Error. Student DNE.
                throw new ArgumentException("NewCourse(int id) - Student DNE.");

            Parent oParent = db.Parents.Include(p => p.Children).SingleOrDefault(p => p.ParentId == SessionSingleton.Current.ParentId && p.Children.Any(s => s.StudentId == oStudent.StudentId));
            if (oParent == null) //Error. This Student/Child doesn't belong to this Parent.
                throw new ArgumentException("NewCourse(int id) - Parent DNE.");

            ResponseCreateCourseViewModel oRVM = new ResponseCreateCourseViewModel()
            {
                CourseVM = new CourseViewModel()
                {
                    StudentId = id
                },
                TeacherVM = new TeacherViewModel()
            };

            return View(oRVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCourse(ResponseCreateCourseViewModel oRVM)
        {
            TempData["UserMessage"] = new MessageVM() { IsSuccessful = false, Title = "Error!", Message = "Something went wrong." };

            if (Helpers.Helpers.IsStaff(User))
                return RedirectToAction("Index", "Staff");

            if (!ModelState.IsValid)
                return View(oRVM);

            CourseViewModel oCourseVM = oRVM.CourseVM;
            TeacherViewModel oTeacherVM = oRVM.TeacherVM;

            Student oStudent = db.Students.Include(s => s.Courses).Include(s => s.Teachers).SingleOrDefault(p => p.StudentId == oCourseVM.StudentId);
            if (oStudent == null)//Error. Student DNE.
                throw new ArgumentException("CreateCourse(ResponseCreateCourseViewModel oRVM) - Student DNE.");

            Parent oParent = db.Parents.Include(p => p.Children).SingleOrDefault(p => p.ParentId == SessionSingleton.Current.ParentId && p.Children.Any(s => s.StudentId == oStudent.StudentId));
            if (oParent == null) //Error. This Student/Child doesn't belong to this Parent.
                throw new ArgumentException("CreateCourse(ResponseCreateCourseViewModel oRVM) - Parent DNE.");

            Teacher oTeacher = new Teacher()
            {
                Email = oTeacherVM.Email,
                FirstName = oTeacherVM.FirstName,
                LastName = oTeacherVM.LastName,
                Gender = oTeacherVM.Gender,
                Phone = oTeacherVM.Phone,
                PreferredMethodOfContact = oTeacherVM.PreferredMethodOfContact
            };

            Course oCourse = new Course()
            {
                Name = oCourseVM.Name,
                TimeStart = oCourseVM.TimeStart,
                TimeEnd = oCourseVM.TimeEnd,
                ClassType = oCourseVM.ClassType,
                SchoolName = oCourseVM.SchoolName,
                Teacher = oTeacher
            };

            oStudent.Courses.Add(oCourse);//Add Course to Student
            oStudent.Teachers.Add(oTeacher);//Add Teacher to Student

            try
            {
                if (db.SaveChanges() <= 0)
                    return View(oRVM);
            }
            catch (Exception e)
            {
                return View(oRVM);
            }

            TempData["UserMessage"] = new MessageVM() { IsSuccessful = true, Title = "Success!", Message = "Your child's course has been successfully added." };

            return RedirectToAction("Index", new { StudentId = oStudent.StudentId, CourseId = oCourse.CourseId });
        }


        public ActionResult EditCourse(int StudentId, int CourseId)
        {
            ViewBag.Title = "Edit Course";

            if (Helpers.Helpers.IsStaff(User))
                return RedirectToAction("Index", "Staff");

            Course oCourse = db.Courses.Include(c => c.Teacher).SingleOrDefault(c => c.CourseId == CourseId);
            if (oCourse == null)//Error. Course DNE.
                throw new ArgumentException("EditCourse(int StudentId, int CourseId) - Course DNE.");

            Teacher oTeacher = oCourse.Teacher;

            Student oStudent = db.Students.SingleOrDefault(s => s.StudentId == StudentId && s.Courses.Any(c => c.CourseId == oCourse.CourseId));
            if (oStudent == null)//Error. Student DNE OR doesn't take this Course.
                throw new ArgumentException("EditCourse(int StudentId, int CourseId) - Student DNE.");

            Parent oParent = db.Parents.Include(p => p.Children).SingleOrDefault(p => p.ParentId == SessionSingleton.Current.ParentId && p.Children.Any(s => s.StudentId == oStudent.StudentId));
            if (oParent == null) //Error. This Student/Child doesn't belong to this Parent.
                throw new ArgumentException("EditCourse(int StudentId, int CourseId) - Parent DNE.");

            ResponseCreateCourseViewModel oRVM = new ResponseCreateCourseViewModel()
            {
                CourseVM = new CourseViewModel()
                {
                    CourseId = oCourse.CourseId,
                    Name = oCourse.Name,
                    TimeStart = oCourse.TimeStart,
                    TimeEnd = oCourse.TimeEnd,
                    ClassType = oCourse.ClassType,
                    SchoolName = oCourse.SchoolName,
                    StudentId = StudentId
                },
                TeacherVM = new TeacherViewModel()
                {
                    TeacherId = oTeacher.TeacherId,
                    Email = oTeacher.Email,
                    FirstName = oTeacher.FirstName,
                    LastName = oTeacher.LastName,
                    Gender = oTeacher.Gender,
                    Phone = oTeacher.Phone,
                    PreferredMethodOfContact = oTeacher.PreferredMethodOfContact
                }
            };

            return View(oRVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AlterCourse(ResponseCreateCourseViewModel oRVM)
        {
            TempData["UserMessage"] = new MessageVM() { IsSuccessful = false, Title = "Error!", Message = "Something went wrong." };

            if (Helpers.Helpers.IsStaff(User))
                return RedirectToAction("Index", "Staff");

            if (!ModelState.IsValid)
                return View(oRVM);

            CourseViewModel oCourseVM = oRVM.CourseVM;
            Course oCourse = db.Courses.Include(c => c.Teacher).SingleOrDefault(c => c.CourseId == oCourseVM.CourseId);
            if (oCourse == null)//Error. Course DNE.
                throw new ArgumentException("AlterCourse(ResponseCreateCourseViewModel oRVM) - Course DNE.");

            TeacherViewModel oTeacherVM = oRVM.TeacherVM;
            Teacher oTeacher = oCourse.Teacher;

            Student oStudent = db.Students.SingleOrDefault(s => s.StudentId == oCourseVM.StudentId && s.Courses.Any(c => c.CourseId == oCourse.CourseId));
            if (oStudent == null)//Error. Student DNE OR doesn't take this Course.
                throw new ArgumentException("AlterCourse(ResponseCreateCourseViewModel oRVM) - Student DNE.");

            Parent oParent = db.Parents.Include(p => p.Children).SingleOrDefault(p => p.ParentId == SessionSingleton.Current.ParentId && p.Children.Any(s => s.StudentId == oStudent.StudentId));
            if (oParent == null) //Error. This Student/Child doesn't belong to this Parent.
                throw new ArgumentException("AlterCourse(ResponseCreateCourseViewModel oRVM) - Parent DNE.");

            oCourse.Name = oCourseVM.Name;
            oCourse.TimeStart = oCourseVM.TimeStart;
            oCourse.TimeEnd = oCourseVM.TimeEnd;
            oCourse.ClassType = oCourseVM.ClassType;
            oCourse.SchoolName = oCourseVM.SchoolName;

            oTeacher.Email = oTeacherVM.Email;
            oTeacher.FirstName = oTeacherVM.FirstName;
            oTeacher.LastName = oTeacherVM.LastName;
            oTeacher.Gender = oTeacherVM.Gender;
            oTeacher.Phone = oTeacherVM.Phone;
            oTeacher.PreferredMethodOfContact = oTeacherVM.PreferredMethodOfContact;

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return View(oRVM);
            }

            TempData["UserMessage"] = new MessageVM() { IsSuccessful = true, Title = "Success!", Message = "Your child's course has been successfully updated." };

            return RedirectToAction("Index", new { StudentId = oCourseVM.StudentId, CourseId = oCourse.CourseId });
        }
        #endregion


        #region Helper Functions

        /// <summary>
        /// Return the Parent's Child at id, or the Parent's first Child if Child at id DNE, or empty Student otherwise.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Student GetStudentOrDefault(int id)
        {
            Student oStudent = db.Students.Include(s => s.Courses).SingleOrDefault(s => s.StudentId == id);//.Select(c => c.Teacher)
            if (oStudent == null)
            {
                //Get the Parent's 1st Child
                List<Student> oStudents = db.Parents.Include(p => p.Children.Select(s => s.Courses)).SingleOrDefault(p => p.Email == User.Identity.Name).Children;//.Select(c => c.Teacher)
                if (oStudents.Count > 0)
                    oStudent = oStudents.First();

                if (oStudent == null)
                    oStudent = new Student();//This parent has no children yet
            }

            return oStudent;
        }

        /// <summary>
        /// Returns the Student's Course at id, or the Student's first Course if Course at id DNE, or empty Course otherwise.
        /// </summary>
        /// <param name="CourseId"></param>
        /// <param name="StudentId"></param>
        /// <returns></returns>
        private Course GetCourseOrDefault(int CourseId, int StudentId)
        {
            Course oCourse = db.Courses.Include(c => c.Teacher).SingleOrDefault(c => c.CourseId == CourseId);
            if (oCourse == null)
            {
                //Get the Student's 1st Course
                Student oStudent = db.Students.Include(s => s.Courses.Select(c => c.Teacher)).SingleOrDefault(s => s.StudentId == StudentId);

                if(oStudent != null)
                {
                    List<Course> oCourses = oStudent.Courses;
                    if (oCourses.Count > 0)
                        oCourse = oCourses.First();
                }

                if (oCourse == null)
                {
                    oCourse = new Course()
                    {
                        Teacher = new Teacher()
                    };//This student has no courses yet
                }
            }

            return oCourse;
        }
        #endregion
    }
}