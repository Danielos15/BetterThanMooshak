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
    }

    public class ProblemDetailsViewModel
    {
        public string course { get; set; }
        public int courseId { get; set; }
        public string localPath { get; set; }
        public Assignment assignment { get; set; }
        public Problem problem { get; set; }
        public Solution currSolution { get; set; }
        public IQueryable<Testcase> testcases { get; set; }
        public IQueryable<Solution> submissions { get; set; }
        public List<BestSolutionViewModel> allSubmissions { get; set; }
        public IQueryable<Hint> hints { get; set; }
        public DiscussionViewModel discussions { get; set; }
        public Solution answer { get; set; }
        public bool isTeacher { get; set; }
        public bool isAssistant { get; set; }
    }
}