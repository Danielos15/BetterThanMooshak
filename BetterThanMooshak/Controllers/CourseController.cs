using BetterThanMooshak.Models;
using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Models.ViewModel;
using BetterThanMooshak.Services;
using BetterThanMooshak.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;


namespace BetterThanMooshak.Controllers
{
    public class CourseController : Controller
    {
        private CourseService service = new CourseService();

        #region Index view
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            IQueryable<Course> viewModel = service.GetAllCourses();

            return View(viewModel);
        }
        #endregion

        #region Add view
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CourseAddViewModel newCourse)
        {
            if(!service.Add(newCourse))
            {
                ModelState.AddModelError("", "Could not add this Course!");
                return View(newCourse);
            }

            return RedirectToAction("index");
        }
        #endregion

        #region Remove view
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Remove(int? id)
        {
            if(!service.RemoveCourseById(id.Value))
            {
                ModelState.AddModelError("", "Could not remove this Course!");

                return RedirectToAction("index");
            }

            return RedirectToAction("index");
        }
        #endregion

        #region Edit view
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            CourseEditViewModel viewModel = service.GetCourseEditViewModel(id.Value);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CourseEditViewModel editCourse)
        {
            if (!service.Edit(editCourse))
            {
                ModelState.AddModelError("", "Could not add this Course!");
                return View(editCourse);
            }

            return RedirectToAction("index");

        }
        #endregion

        #region Enrole view
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Enrole(int? id)
        {
            if (id != null)
            {
                CourseUserEnroleViewModel viewModel = service.GetEnroleViewModel(id.Value);

                return View(viewModel);
            }

            return RedirectToAction("notfound","error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Enrole(FormCollection form)
        {
            int courseId;
            
            if(int.TryParse(form["courseId"], out courseId))
            {
                var roles = form["roles"];

                if (roles != null)
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    CourseUserEnroleSaveViewModel enroles = js.Deserialize<CourseUserEnroleSaveViewModel>(roles);

                    service.RemoveAllFromCourse(courseId);

                    foreach (var teacher in enroles.teachers)
                    {
                        service.AddTeacherToCourse(teacher, courseId);
                    }

                    foreach (var assistant in enroles.assistants)
                    {
                        service.AddAssistantToCourse(assistant, courseId);
                    }

                    foreach (var student in enroles.students)
                    {
                        service.AddStudentToCourse(student, courseId);
                    }
                }
            }
            
            return RedirectToAction("index", "course");
        }
        #endregion

        #region Usercourses view
        public ActionResult UserCourses()
        {
            UserCoursesViewModel viewModel = service.GetUserCourses();

            return View(viewModel);
        }
        #endregion

        #region Details view
        public ActionResult Details(int? id)
        {
            CourseAssignments viewModel = service.GetCourseAssignments(id.Value);

            return View(viewModel);
        }
        #endregion
    }
}