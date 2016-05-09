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
    public class SolutionService
    {
        private ApplicationDbContext db;

        public SolutionService()
        {
            db = new ApplicationDbContext();
        }

        public bool AddSolution(Solution s)
        {

            Solution temp = new Solution { Id = s.Id, userId = s.userId, problemId = s.problemId, program = s.program };
            db.Solutions.Add(temp);

            return Convert.ToBoolean(db.SaveChanges());
        }

        public bool EditSolution(Solution solution)
        {
            var s = GetSolutionById(solution.Id);
            s.Id = solution.Id;
            s.userId = solution.userId;
            s.problemId = solution.problemId;
            s.program = solution.program;

            return Convert.ToBoolean(db.SaveChanges());
        }

        public Solution GetSolutionById(int id)
        {
            return (from s in db.Solutions
                            where s.Id == id
                            select s).SingleOrDefault();
        }

        public SolutionViewModel GetSolutionsByProblemAndUser(int pId, int uId)
        {
            var solution = (from x in db.Solutions
                            where x.Id == pId && x.Id == uId
                            select x).ToList();

            SolutionViewModel result = new SolutionViewModel();
            result.solutions = solution;

            return result;
        }

        public SolutionViewModel GetSolutionsByUser(int uId)
        {
            var solutions = (from x in db.Solutions
                             where x.Id == uId
                             select x).ToList();

            SolutionViewModel result = new SolutionViewModel();
            result.solutions = solutions;

            return result;
        }

        public IQueryable<Solution> getAllSolutions()
        {
            var currentUser = HttpContext.Current.User.Identity.GetUserId();

            var allSolutions = (from cu in db.CourseUsers
                                join c in db.Courses on cu.courseId equals c.id into userCourses
                                where cu.userId == currentUser
                                from course in userCourses
                                join a in db.Assignments on course.id equals a.courseId into assignments
                                from ass in assignments
                                join p in db.Problems on ass.id equals p.assignmentId into problems
                                from prob in problems
                                join s in db.Solutions on prob.Id equals s.problemId into solutions
                                from x in solutions
                                select x).AsQueryable();

            return allSolutions;
        }
    }
}
