﻿using BetterThanMooshak.Models.Entities;
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
    public class AssignmentController : Controller
    {
        private AssignmentService service = new AssignmentService();
        // GET: Assignments
        public ActionResult Index()
        {
            return View(service.getAll());
        }

        public ActionResult Add(int? id)
        {
            return View(service.Initialize(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AssignmentViewModel newAss)
        {
            var assignment = newAss.assignment;

            if(!service.AddAssignmet(assignment))
            {
                ModelState.AddModelError("", "The Assignment could not be added to the database");
                return View(newAss);
            }

            return RedirectToAction("index", "course");
        }

        public ActionResult Details(int? id)
        {
            if (service.verifyUser(id.Value))
                return View(service.getAssignmentProblems(id.Value));
            else
            {
                ModelState.AddModelError("", "User not authorized");
                return RedirectToAction("index", "home");
            }
        }

        public ActionResult Edit (int? id)
        {
            return View(service.getAssignmentById(id.Value));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Assignment assignment)
        {
            if (!service.Edit(assignment))
            {
                ModelState.AddModelError("", "Could not edit this Assignment!");
                return View(assignment);
            }

            return RedirectToAction("index");
        }

    }
}