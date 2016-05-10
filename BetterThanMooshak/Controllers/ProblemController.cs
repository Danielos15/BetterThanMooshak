using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Models.ViewModel;
using BetterThanMooshak.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                    return View(service.getDetails(id.Value));
                else
                {
                    return RedirectToAction("index", "home");
                }
            }
            return RedirectToAction("notfound", "error");
        }
        public ActionResult Add(int? id)
        {
            return View(service.Initialize(id));
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
            return View(service.GetProblemById(id.Value));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Problem problem)
        {
            if (!service.Edit(problem))
            {
                ModelState.AddModelError("", "Could not edit this Assignment!");
                return View(problem);
            }

            return RedirectToAction("details", "assignment", new { id = problem.assignmentId});
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
    }
}