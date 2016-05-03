using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.ViewModel
{
    public class AssignmentViewModel
    {
        public string name { get; set; }
        public List<ProblemViewModel> Problems { get; set; }
    }
}