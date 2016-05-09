using BetterThanMooshak.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.ViewModel
{
    public class AssignmentViewModel
    {
        public Course course { get; set; }
        public Assignment assignment { get; set; }
    }

    public class AssignmentIndexViewModel
    {
        public IQueryable<Assignment> oldAssignments;
        public IQueryable<Assignment> newAssignments;
    }

    public class AssignmentProblems
    {
        public Assignment assignment { get; set; }
        public IQueryable<Problem> problems { get; set; }
    }
}