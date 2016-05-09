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
            if(service.verifyUser(id.Value))
                return View(service.getDetails(id.Value));
            else
            {
                ModelState.AddModelError("", "Insufficient permissions");
                return RedirectToAction("index", "home");
            }
        }
        public ActionResult Add(int? id)
        {
            return View(service.Initialize(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ProblemViewModel newProb)
        {
            var problem = newProb.problem;

            if (!service.AddProblem(problem))
            {
                ModelState.AddModelError("", "The Problem could not be added to the database");
                return View(newProb);
            }

            return RedirectToAction("details", "assignment", newProb.assignment.id);
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

            return RedirectToAction("index");
        }
    }
}