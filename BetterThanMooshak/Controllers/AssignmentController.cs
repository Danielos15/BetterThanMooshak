using BetterThanMooshak.Models.ViewModel;
using BetterThanMooshak.Services;
using System.Web.Mvc;

namespace BetterThanMooshak.Controllers
{
    public class AssignmentController : Controller
    {
        private AssignmentService service = new AssignmentService();
        // GET: Assignments
        public ActionResult Index()
        {
            return View(service.GetAll());
        }

        public ActionResult Add(int? id)
        {
            if (id != null)
            {
                return View();
            }
            return View("404");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(int? id, AssignmentAddViewModel model)
        {
            if (id != null)
            {
                if (!service.AddAssignmet(id.Value, model))
                {
                    ModelState.AddModelError("", "The Assignment could not be added to the database");
                    return View(model);
                }
                return RedirectToAction("details", "course", new { id = id.Value });
            }
            return View("404");
        }

        public ActionResult Details(int? id)
        {
            if(id != null) { 
                if (service.verifyUser(id.Value))
                    return View(service.GetAssignmentProblems(id.Value));
                else
                {
                    ModelState.AddModelError("", "User not authorized");
                    return RedirectToAction("index", "home");
                }
            }
            return RedirectToAction("notfound", "error");
        }

        public ActionResult Edit (int? id)
        {
            if (id != null)
            {
                AssignmentAddViewModel model = service.GetAssignmentAddViewModelById(id.Value);

                return View(model);
            }
            return View("404");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, AssignmentAddViewModel model)
        {
            if (id != null)
            {
                if (!service.Edit(id.Value, model))
                {
                    ModelState.AddModelError("", "No changes have been made");
                    return View(model);
                }
                int returnId = service.GetCourseIdByAssignmentId(id.Value);
                return RedirectToAction("details", "course", new { id = returnId });
            }
            return View("404");
        }

    }
}