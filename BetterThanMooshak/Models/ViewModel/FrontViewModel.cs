using BetterThanMooshak.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.ViewModel
{
    public class FrontViewModel
    {
        public ApplicationUser user { get; set; }
        public IQueryable<Assignment> assignments { get; set; }
        public IQueryable<Notification> notifications { get; set; }
        public IQueryable<Course> courses { get; set; }
        public IQueryable<ProblemGrade> recentGrades { get; set; }
    }
}