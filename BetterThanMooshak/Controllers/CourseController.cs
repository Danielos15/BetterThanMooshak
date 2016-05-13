using BetterThanMooshak.Models;
using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Models.ViewModel;
using BetterThanMooshak.Services;
using BetterThanMooshak.Utilities;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;


namespace BetterThanMooshak.Controllers
{
    public class CourseController : Controller
    {
        private CourseService service = new CourseService(null);

        #region Index Action - Get overview of all Courses
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            if (TempData["errorMessage"] != null)
            {
                ViewBag.errorMessage = TempData["errorMessage"].ToString();
            }
            if (TempData["message"] != null)
            {
                ViewBag.message = TempData["message"].ToString();
            }

            IQueryable<Course> courses = service.GetAllCourses();

            CourseIndexViewModel model = new CourseIndexViewModel { courses = courses};

            return View(model);
        }
        #endregion

        #region Add Action - Add new Courses
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CourseAddViewModel newCourse)
        {
            if (ModelState.IsValid)
            {
                if (!service.Add(newCourse))
                {
                    ModelState.AddModelError("", newCourse.name + " could not be saved!");
                    return View(newCourse);
                }
                TempData["message"] = newCourse.name + " has been added!";
                return RedirectToAction("index");
            }
            ModelState.AddModelError("", "Submission invalid!");
            return View(newCourse);
        }
        #endregion

        #region Import Action - Import list of Courses with .csv

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase inputFileBase)
        {
            if (inputFileBase == null)
            {
                TempData["message"] = "Please select a .csv file!";

                return RedirectToAction("index");
            }

            if (inputFileBase.ContentType != "application/vnd.ms-excel")
            {
                TempData["message"] = "Invalid file type!";

                return RedirectToAction("index");
            }

            var newCourses = service.ImportCourses(inputFileBase);

            foreach (var newCourse in newCourses)
            {
                if (!service.Add(newCourse))
                {
                    ModelState.AddModelError("", newCourse.name + " could not be saved!");
                }
            }

            TempData["message"] = "Courses have been imported!";

            return RedirectToAction("index");
        }
        #endregion

        #region Edit Action - Edit existing Courses
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            CourseAddViewModel viewModel = service.GetCourseEditViewModel(id.Value);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, CourseAddViewModel editCourse)
        {
            if (id != null)
            {
                if (!service.Edit(id.Value, editCourse))
                {
                    ModelState.AddModelError("", "No changes have been made");
                    return View(editCourse);
                }

                return RedirectToAction("index");
            }
            return View("404");

        }
        #endregion

        #region Remove Action - Remove existing Courses
        [CustomAuthorize(Roles = "Admin")]
        public ActionResult Remove(int? id)
        {
            if (id != null)
            {
                Course course = service.GetCourseById(id.Value);
                if (service.CanDeleteCourse(course))
                {
                    if (service.RemoveCourseById(id.Value))
                    {
                        TempData["message"] = course.name + " has been removed!";
                        return RedirectToAction("index");
                    }
                }
                TempData["errorMessage"] = course.name + " could not be removed!";
                return RedirectToAction("index");
            }
            return View("404");
        }
        #endregion

        #region Enrole Action - Link users to courses
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

        #region Usercourses Action - Overview of courses linked to the current user
        public ActionResult UserCourses()
        {
            var userId = User.Identity.GetUserId();
            UserCoursesViewModel viewModel = service.GetUserCourses(userId);

            return View(viewModel);
        }
        #endregion

        #region Details Action - Get details for certain Course
        public ActionResult Details(int? id)
        {
            var userId = User.Identity.GetUserId();
            CourseAssignments viewModel = service.GetCourseAssignments(id.Value, userId);

            return View(viewModel);
        }
        #endregion
    }
}