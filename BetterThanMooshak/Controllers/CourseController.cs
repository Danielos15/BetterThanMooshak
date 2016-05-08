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

namespace BetterThanMooshak.Controllers
{
    public class CourseController : Controller
    {
        private CourseService service = new CourseService();
        
        // GET: Course
        public ActionResult Index()
        {
            CourseViewModel viewModel = service.GetAllCourses();
            return View(viewModel);
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
            CourseViewModel user = service.GetCourseById(id);
            CourseEditViewModel model = new CourseEditViewModel
            {
                id = user.course.id,
                name = user.course.name,
                startDate = user.course.startDate,
                endDate = user.course.endDate
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
    }
}