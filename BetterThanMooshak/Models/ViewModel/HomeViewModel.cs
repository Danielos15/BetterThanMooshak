using BetterThanMooshak.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.ViewModel
{
    public class HomeViewModel
    {
        public string userName { get; set; }
        public List<AssignmentViewModel> assignments { get; set; }
        public List<HomeNotification> notifications { get; set; }
        public IQueryable<Course> courses { get; set; }
        public List<HomeGrade> grades { get; set; }
    }
    public class HomeNotification
    {
        public Notification notification { get; set; }
        public Assignment assignment { get; set; }
    }
    public class HomeGrade
    {
        public Grade grade { get; set; }
        public Assignment assignment { get; set; }
        public Course course { get; set; }
    }
}