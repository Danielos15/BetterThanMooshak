using BetterThanMooshak.Models.ViewModel;
using BetterThanMooshak.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
            CourseViewModel viewModel = service.GetCourseById(1);
            return View(viewModel);
        }

        public ActionResult Overview()
        {
            CourseViewModel viewModel = service.GetAllCourses();

            return View(viewModel);
        }
    }
}