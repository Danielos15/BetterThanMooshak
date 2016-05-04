using BetterThanMooshak.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.ViewModel
{
    public class CourseViewModel
    {
        public string name { get; set; }
        public List<Assignment> assignments { get; set; }
    }
}