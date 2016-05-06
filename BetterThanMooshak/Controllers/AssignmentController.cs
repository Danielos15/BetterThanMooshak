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
            return View();
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
            /*
            Assignment assignment = new Assignment()
            {
                name = newAss.assignment.name,
                description = newAss.assignment.description,
                courseId = newAss.assignment.courseId,
                startDate = newAss.assignment.startDate,
                endDate = newAss.assignment.endDate
            };*/

            if(!service.AddAssignmet(assignment))
            {
                ModelState.AddModelError("", "The Assignment could not be added to the database");
                return View(newAss);
            }

            return RedirectToAction("index", "course");
        }
    }
}