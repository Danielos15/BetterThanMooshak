using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Models.ViewModel;
using BetterThanMooshak.Services;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace BetterThanMooshak.Controllers
{
    public class ProblemController : Controller
    {
        private ProblemService service = new ProblemService();
        // GET: Problem
        public ActionResult Index()
        {
            return View(service.getAllProblems());
        }

        public ActionResult Details (int? id)
        {
            if (id != null)
            {
                if (service.verifyUser(id.Value))
                {
                    ProblemDetailsViewModel model = service.getDetails(id.Value);
                    model.localPath = User.Identity.GetUserId() + model.courseId + model.assignment.id + model.problem.id;
                    return View(model);
                }
                else
                {
                    return RedirectToAction("unauthorizederror", "error");
                }
            }
            return RedirectToAction("notfound", "error");
        }
        public ActionResult Add(int? id)
        {
            ProblemAddViewModel model = service.Initialize(id.Value);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(int? id, ProblemAddViewModel model)
        {
            if (id != null)
            {
                Problem problem = new Problem()
                {
                    assignmentId = id.Value,
                    name = model.name,
                    maxAttempts = model.maxAttempts,
                    description = model.description,
                    percentOfGrade = model.percentOfGrade
                };

                if (!service.AddProblem(problem))
                {
                    ModelState.AddModelError("", "The Problem could not be added to the database");
                    return View(model);
                }
                return RedirectToAction("details", "assignment", new { id = id.Value});
            }
            return RedirectToAction("notfound", "error");
        }
        public ActionResult Edit (int? id)
        {
            ProblemAddViewModel viewModel = service.GetProblemEditViewModel(id.Value);

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, ProblemAddViewModel problem)
        {
            if(id != null)
            {
                if (!service.Edit(id.Value, ref problem))
                {
                    ModelState.AddModelError("", "Could not edit this Assignment!");
                    return View(problem);
                }

                return RedirectToAction("details", "assignment", new { id = problem.assignmentId });
            }

            return View("404");

        }

        public ActionResult AddTestcase (int? id)
        {
            if (id != null)
            {
                TestcaseAddViewModel model = new TestcaseAddViewModel()
                {
                    problemId = id.Value
                };
                return View(model);
            }
            return RedirectToAction("notfound", "error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTestcase(int? id, TestcaseAddViewModel model)
        {
            if (id != null)
            {
                model.problemId = id.Value;
                if (ModelState.IsValid)
                {
                    if (service.AddTestcase(model))
                    {
                        return RedirectToAction("details", "problem", new { id = id.Value });
                    }
                    ModelState.AddModelError("", "Unable to save to database, try again");
                    return View(model);
                }
                return View(model);
            }
            return RedirectToAction("notfound", "error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddTopic(int? id, DisscussionAddTopicViewModel model)
        {
            if (id != null)
            {
                if (ModelState.IsValid)
                {
                    service.AddTopic(model, id.Value, User.Identity.GetUserId());
                    return RedirectToAction("details", "problem", new { id = id.Value });
                }
            }
            return RedirectToAction("notfound", "error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddComment(int? id, int? problemId, DisscussionAddCommentViewModel model)
        {
            if (id != null)
            {
                if (ModelState.IsValid)
                {
                    service.AddComment(model, id.Value, User.Identity.GetUserId());
                    return RedirectToAction("details", "problem", new { id = problemId.Value });
                }
            }
            return RedirectToAction("notfound", "error");
        }

    }
}