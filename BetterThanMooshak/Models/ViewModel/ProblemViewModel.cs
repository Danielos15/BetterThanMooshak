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

    public class ProblemAddViewModel
    {
        public string courseName { get; set; }
        public string assignmentName { get; set; }
        public string name { get; set; }
        public int assignmentId { get; set; }
        public int maxAttempts { get; set; }
        public int percentOfGrade { get; set; }
        public string description { get; set; }

        //Json string of testcases
        // Save for each obj in array
        public ProblemTestCasesViewModel testcases { get; set; }
    }

    public class ProblemTestCasesViewModel
    {
        public List<Testcase> cases { get; set; }
    }
}