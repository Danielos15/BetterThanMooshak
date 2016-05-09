using BetterThanMooshak.Models;
using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Models.ViewModel;
using BetterThanMooshak.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BetterThanMooshak.Controllers
{
    public class CourseController : Controller
    {
        private CourseService service = new CourseService();
        
        // GET: Course
        public ActionResult Index()
        {
            return View(service.GetAllCourses());
        }

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

        public ActionResult Remove(int? id)
        {
            if(!service.RemoveCourseById(id.Value))
            {
                ModelState.AddModelError("", "Could not remove this Course!");

                return RedirectToAction("index");
            }

            return RedirectToAction("index");
        }

        public ActionResult Edit(int id)
        {
            Course user = service.GetCourseById(id);
            CourseEditViewModel model = new CourseEditViewModel
            {
                id = user.id,
                name = user.name,
                startDate = user.startDate,
                endDate = user.endDate
            };
            return View(model);
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

        public ActionResult Enrole(int? id)
        {
            if (id != null)
            {
                int coursId = id.Value;
                CourseUserEnroleViewModel model = new CourseUserEnroleViewModel()
                {
                    courseId = id.Value,
                    availableUsers = service.GetAvalibleUsersForCourse(coursId),
                    teachers = service.GetTeachersForCourse(coursId),
                    assistants = service.GetAssistantsForCourse(coursId),
                    students = service.GetStudentsForCourse(coursId)
                };
                return View(model);
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

        public ActionResult UserCourses()
        {
            return View(service.GetUserCourses());
        }

        public ActionResult Details(int? id)
        {
            return View(service.GetCourseAssignments(id.Value));
        }

        public ActionResult Search(string searchString)
        {
            return View();
        }
    }
}