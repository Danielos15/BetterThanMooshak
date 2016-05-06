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
            service.Add(newCourse);

            if(!service.Add(newCourse))
            {
                ModelState.AddModelError("", "brah wtf");
                return View(newCourse);
            }

            return RedirectToAction("index");
        }

    }
}