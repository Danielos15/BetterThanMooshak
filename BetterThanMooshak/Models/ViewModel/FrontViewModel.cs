using BetterThanMooshak.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.ViewModel
{
    public class FrontViewModel
    {
        public List<Assignment> assignments { get; set; }
        public List<Notification> notifications { get; set; }
        public List<Course> courses { get; set; }
        public List<ProblemGrade> recentGrades { get; set; }
    }
}