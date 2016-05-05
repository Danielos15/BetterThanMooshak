using BetterThanMooshak.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.ViewModel
{
    public class CourseViewModel
    {
        public List<Course> courses { get; set; }
        public Course course { get; set; }
    }
}