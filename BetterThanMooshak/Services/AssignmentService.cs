using BetterThanMooshak.Models;
using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Services
{
    public class AssignmentService
    {
        private ApplicationDbContext db;

        public AssignmentService()
        {
            db = new ApplicationDbContext();
        }

        public AssignmentViewModel GetAssignmentsByCourse (int courseId)
        {
            var course = (from x in db.Courses where x.id == courseId select x).SingleOrDefault();

            var result = (from x in db.Assignments where x.courseId == courseId select x).ToList();

            var assignments = new AssignmentViewModel() { name = course.name, assignments = result };

            return assignments;
        }

        public AssignmentViewModel GetAssignmentById (int? assignmentId)
        {
            return null;
        }
    }
}