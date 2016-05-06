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
            var assignment = (from a in db.Assignments
                              where a.id == courseId
                              select a).SingleOrDefault();

            AssignmentViewModel result = new AssignmentViewModel();
            result.assignment = assignment;

            return result;
        }

        public AssignmentViewModel GetAssignmentById (int? assignmentId)
        {
            var assignment = (from a in db.Assignments
                          where a.id == assignmentId
                          select a).SingleOrDefault();

            AssignmentViewModel result = new AssignmentViewModel();
            result.assignment = assignment;

            return result;
        }
    }
}