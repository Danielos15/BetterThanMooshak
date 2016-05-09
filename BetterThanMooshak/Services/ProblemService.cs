using BetterThanMooshak.Models;
using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Models.ViewModel;
using Microsoft.AspNet.Identity;
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
        public Problem GetProblemById(int problemId)
        {
            return (from p in db.Problems
                    where p.id == problemId
                    select p).SingleOrDefault();
        }

        internal bool verifyUser(int value)
        {
            var currentUser = HttpContext.Current.User.Identity.GetUserId();

            var problem = (from problems in getAllProblems()
                           where problems.id == value
                           select problems).SingleOrDefault();

            if (problem == null)
                return false;
            else
                return true;
        }

        public ProblemAddViewModel Initialize(int? id)
        {
            if (id != null)
            {
                var assignment = (from assignments in db.Assignments
                                  where assignments.id == id.Value
                                  select assignments).SingleOrDefault();
                var course = (from x in db.Courses
                              where x.id == assignment.courseId
                              select x).SingleOrDefault();

                ProblemAddViewModel model = new ProblemAddViewModel()
                {
                    assignmentId = assignment.id,
                    assignmentName = assignment.name,
                    courseName = course.name,
                };
                return model;
            }
            
            return null;
        }

        public bool Edit(Problem problem)
        {
            var p = GetProblemById(problem.id);

            p.assignmentId = problem.assignmentId;
            p.maxAttempts = problem.maxAttempts;
            p.name = problem.name;

            return Convert.ToBoolean(db.SaveChanges());
        }

        public IQueryable<Problem> getAllProblems()
        {
            var currentUser = HttpContext.Current.User.Identity.GetUserId();

            var problems = from cu in db.CourseUsers
                           join c in db.Courses on cu.courseId equals c.id into userCourses
                           where cu.userId == currentUser
                           from course in userCourses
                           join a in db.Assignments on course.id equals a.courseId into assignments
                           from ass in assignments
                           join p in db.Problems on ass.id equals p.assignmentId into result
                           from x in result
                           select x;
            
            return problems;
        }
        public bool AddProblem(Problem add)
        {
            db.Problems.Add(add);

            return Convert.ToBoolean(db.SaveChanges());
        }

        public ProblemDetailsViewModel getDetails(int value)
        {
            var currentUser = HttpContext.Current.User.Identity.GetUserId();

            var problem = GetProblemById(value);

            var assignment = (from problems in db.Problems
                             join assignments in db.Assignments on problems.assignmentId equals assignments.id into a
                             where problems.id == value
                             from ass in a
                             select ass).SingleOrDefault();

            var course = (from a in db.Assignments
                          join c in db.Courses on a.courseId equals c.id into courses
                          where a.id == assignment.id
                          from co in courses
                          select co).SingleOrDefault();

            var currSolution = new Solution { problemId = problem.id, userId = currentUser };

            var testcases = (from t in db.Testcases
                            where t.problemId == problem.id
                            select t).AsQueryable();

            var submissions = (from s in db.Solutions
                              where s.userId == currentUser && s.problemId == problem.id
                              select s).AsQueryable();

            IQueryable<string> hints = null; //TODO

            var discussions = from d in db.DiscussionTopics
                              select d;

            var answer = new Solution { program = "This is an answer to this problem", problemId = problem.id };

            var viewModel = new ProblemDetailsViewModel () {
                course = course.name,
                assignment = assignment.name,
                problem = problem,
                currSolution = currSolution,
                testcases = testcases,
                submissions = submissions,
                hints = hints,
                discussions = discussions,
                answer = answer
            };

            return viewModel;
        }
    }
}