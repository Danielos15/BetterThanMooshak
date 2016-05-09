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
        public IQueryable<Problem> GetProblemsByAssignment(int assignmentId)
        {
            return (from p in db.Problems
                           where p.id == assignmentId
                           select p).AsQueryable();
        }
    }
}