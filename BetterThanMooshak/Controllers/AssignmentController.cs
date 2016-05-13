using BetterThanMooshak.Models.ViewModel;
using BetterThanMooshak.Services;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace BetterThanMooshak.Controllers
{
    public class AssignmentController : Controller
    {
        private AssignmentService service = new AssignmentService(null);

        #region Index Action - Get overview off all Assignments
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            return View(service.GetAll(userId));
        }
        #endregion

        #region Details Action - Get details for a single Assignment
        public ActionResult Details(int? id)
        {
            var userId = User.Identity.GetUserId();

            if (id != null)
            {
                if (service.verifyUser(id.Value, userId))
                {
                    if (TempData["errorMessage"] != null)
                    {
                        ViewBag.errorMessage = TempData["errorMessage"].ToString();
                    }
                    if (TempData["message"] != null)
                    {
                        ViewBag.message = TempData["message"].ToString();
                    }

                    return View(service.GetAssignmentProblems(id.Value, userId));
                }
                else
                {
                    return RedirectToAction("index", "home");
                }
            }
            return RedirectToAction("notfound", "error");
        }
        #endregion

        #region Add Action - New Assignment
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
        #endregion

        #region Edit Action - Edit Assignment
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
        #endregion

        #region Grade Action - Teachers grade students solution
        public ActionResult Grade(int? id)
        {
            if (id != null)
            {
                var userId = User.Identity.GetUserId();

                if (service.isTeacher(userId, id.Value))
                {
                    var model = service.GetGradeViewModel(id.Value);

                    return View(model);
                }

                return RedirectToAction("unauthorizederror", "error");
            }

            return View("404");
        }

        [HttpPost]
        public ActionResult Grade(GradeProblemAddViewModel newGrade)
        {
            service.AddGrade(newGrade);
            
            return RedirectToAction("grade", "assignment", new { id = newGrade.assignmentId });
        }
        #endregion
    }
}