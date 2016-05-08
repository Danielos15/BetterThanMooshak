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
    public class AssignmentService
    {
        private ApplicationDbContext db;

        public AssignmentService()
        {
            db = new ApplicationDbContext();
        }

        public AssignmentViewModel Initialize(int? courseId)
        {
            Course selectedCourse = (from courses in db.Courses
                             where courses.id == courseId
                             select courses).SingleOrDefault();

            Assignment newAss = new Assignment() { courseId = selectedCourse.id };

            AssignmentViewModel result = new AssignmentViewModel() { course = selectedCourse, assignment = newAss};

            return result;
        }

        public bool AddAssignmet(Assignment assignment)
        {
            db.Assignments.Add(assignment);
            return Convert.ToBoolean( db.SaveChanges() );
        }

        public AssignmentIndexViewModel getAll()
        {
            var currentUser = HttpContext.Current.User.Identity.GetUserId();

            var currDate = DateTime.Now;

            var appUser = (from user in db.Users
                           where user.Id == currentUser
                           select user).SingleOrDefault();

            var userCourses = from courseusers in db.CourseUsers
                              join courses in db.Courses on courseusers.courseId equals courses.id into result
                              where courseusers.userId == appUser.Id
                              from x in result
                              select x;

            var newAss = from course in userCourses
                                  join ass in db.Assignments on course.id equals ass.courseId into result
                                  from x in result
                                  where x.endDate > currDate
                                  select x;

            var oldAss = from course in userCourses
                                  join ass in db.Assignments on course.id equals ass.courseId into result
                                  from x in result
                                  where x.endDate < currDate
                                  select x;

            var all = new AssignmentIndexViewModel() { newAssignments = newAss, oldAssignments = oldAss };

            return (all);
        }

        public AssignmentProblems getAssignmentById (int id)
        {
            var assignment = (from assignments in db.Assignments
                           where assignments.id == id
                           select assignments).SingleOrDefault();

            var assignmentProblems = (from problems in db.Problems
                            where problems.assignmentId == assignment.id
                            select problems).AsQueryable();

            var result = new AssignmentProblems() { assignment = assignment, problems = assignmentProblems };

            return result;
        }

        public bool verifyUser(int id)
        {
            var currentUser = HttpContext.Current.User.Identity.GetUserId();

            var appUser = (from user in db.Users
                           where user.Id == currentUser
                           select user).SingleOrDefault();

            var userCourses = from courseusers in db.CourseUsers
                              join courses in db.Courses on courseusers.courseId equals courses.id into result
                              where courseusers.userId == appUser.Id
                              from x in result
                              select x;

            var assignment = (from course in userCourses
                         join ass in db.Assignments on course.id equals ass.courseId into result
                         from x in result
                         where x.id == id
                         select x).SingleOrDefault();

            if (assignment == null)
                return false;
            else
                return true;
        }
    }
}