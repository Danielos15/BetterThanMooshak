using BetterThanMooshak.Models;
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

        public List<AssignmentViewModel> GetAssignmentsByCourse (int courseId)
        {
            return null;
        }

        public AssignmentViewModel GetAssignmentById (int? assignmentId)
        {
            var assignment = db.Assignments.SingleOrDefault (x => x.Id == assignmentId.Value);

            if (assignment == null)
            {
                // TODO: kasta villu!
            }

            var problems = db.Problems
                .Where(x => x.assignmentId == assignmentId.Value)
                .Select(x => new ProblemViewModel
                {
                    name = x.name
                })
                .ToList();

            var viewModel = new AssignmentViewModel
            {
                name = assignment.name,
                Problems = problems
            };

            return viewModel;
        }
    }
}