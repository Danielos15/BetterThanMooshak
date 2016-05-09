using BetterThanMooshak.Models.ViewModel;
using BetterThanMooshak.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BetterThanMooshak.Controllers
{
    public class SolutionController : Controller
    {
        private SolutionService service = new SolutionService();
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CourseAddViewModel course)
        {
            return RedirectToAction("index");
        }

        public ActionResult Index()
        {
            return View(service.getAllSolutions());
        }

    }
}