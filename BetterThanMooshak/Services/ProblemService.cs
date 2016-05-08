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
                    where p.Id == problemId
                    select p).SingleOrDefault();
        }
        public IQueryable<Problem> getAll()
        {
            var currentUser = HttpContext.Current.User.Identity.GetUserId();
            /*
            var appUser = (from user in db.Users
                           where user.Id == currentUser
                           select user).SingleOrDefault();
            
            var userCourses = from courseusers in db.CourseUsers
                              join courses in db.Courses on courseusers.courseId equals courses.id into result
                              where courseusers.userId == appUser.Id
                              from x in result
                              select x;

            var assignments = from course in userCourses
                         join ass in db.Assignments on course.id equals ass.courseId into result
                         from x in result
                         select x;

            var problems = from a in assignments
                           join p in db.Problems on a.id equals p.assignmentId into result
                           from x in result
                           select x;
                           */

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
        public void AddProblem(Problem add)
        {
            db.Problems.Add(add);
            db.SaveChanges();
        }
        public ProblemViewModel EditProblem(ProblemViewModel edit)
        {
            return null;
        }
        public IQueryable<Problem> GetProblemsByAssignment(int assignmentId)
        {
            return (from p in db.Problems
                           where p.Id == assignmentId
                           select p).AsQueryable();
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