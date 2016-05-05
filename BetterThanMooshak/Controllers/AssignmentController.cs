using BetterThanMooshak.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BetterThanMooshak.Controllers
{
    public class AssignmentController : Controller
    {
        private AssignmentService service = new AssignmentService();
        // GET: Assignments
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            var viewModel = service.GetAssignmentsByCourse(id);

            return View(viewModel);
        }
    }
}