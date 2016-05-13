﻿using BetterThanMooshak.Models;
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

        

        public Solution GetSolutionById(int id)
        {
            return (from s in db.Solutions
                            where s.Id == id
                            select s).SingleOrDefault();
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

        public SolutionViewModel GetSolutionsByProblemAndUser(int pId, int uId)
        {
            var solution = (from x in db.Solutions
                            where x.Id == pId 
                            && x.Id == uId
                            select x).ToList();

            SolutionViewModel result = new SolutionViewModel();
            result.solutions = solution;

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
                                join s in db.Solutions on prob.id equals s.problemId into solutions
                                from x in solutions
                                select x).AsQueryable();

            return allSolutions;
        }

        public bool SaveSolution(int id, string userId, SolutionPostViewModel model)
        {
            /*
            Solution solution = new Solution()
            {
                problemId = id,
                program = model.code,
                userId = userId
            };
            db.Solutions.Add(solution);
            return Convert.ToBoolean(db.SaveChanges());
            */
            return false;
        }
    }
}
