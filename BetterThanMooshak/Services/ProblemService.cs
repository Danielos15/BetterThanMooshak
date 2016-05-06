using BetterThanMooshak.Models;
using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Services
{
    public class ProblemService
    {
        private ApplicationDbContext db;

        public ProblemService()
        {
            db = new ApplicationDbContext();
        }

        public void AddProblem(Problem add)
        {
            db.Problems.Add(add);
            db.SaveChanges();
        }
        public ProblemViewModel EditProblem(ProblemViewModel edit)
        {
            return null;
        }
        public ProblemViewModel GetProblemById(int problemId)
        {
            var problem = (from p in db.Problems
                           where p.Id == problemId
                           select p).SingleOrDefault();

            ProblemViewModel result = new ProblemViewModel();
            result.problem = problem;

            return result;
        }
        public ProblemViewModel GetProblemsByAssignment(int assignmentId)
        {
            var problem = (from p in db.Problems
                           where p.Id == assignmentId
                           select p).ToList();

            ProblemViewModel result = new ProblemViewModel();
            result.problems = problem;

            return result;
        }

        public ProblemViewModel AddTestcase(TestcaseViewModel addTest)
        {
            return null;
        }

        public ProblemViewModel EditTestcase(TestcaseViewModel editTest)
        {
            return null;
        }
    }
}