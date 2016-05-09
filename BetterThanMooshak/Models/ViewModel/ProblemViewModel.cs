using BetterThanMooshak.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.ViewModel
{
    public class ProblemViewModel
    {
        public Assignment assignment { get; set; }
        public Problem problem { get; set; }
    }
    public class ProblemDetailsViewModel
    {
        public string course { get; set; }
        public string assignment { get; set; }
        public Problem problem { get; set; }
        public Solution currSolution { get; set; }
        public IQueryable<Testcase> testcases { get; set; }
        public IQueryable<Solution> submissions { get; set; }
        public IQueryable<string> hints { get; set; }
        public IQueryable<DiscussionTopic> discussions { get; set; }
        public Solution answer { get; set; }
    }
}