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
        private string currentUser = HttpContext.Current.User.Identity.GetUserId();

        public AssignmentService()
        {
            db = new ApplicationDbContext();
        }

        public Assignment GetAssignmentById (int id)
        {
            return (from assignments in db.Assignments
                              where assignments.id == id
                              select assignments).SingleOrDefault();
        }

        public AssignmentViewModel Initialize(int? courseId)
        {
            Course selectedCourse = (from courses in db.Courses
                             where courses.id == courseId
                             select courses).SingleOrDefault();

            Assignment newAss = new Assignment() { courseId = selectedCourse.id, startDate = DateTime.Now , endDate = DateTime.Now.AddDays(20) };

            AssignmentViewModel result = new AssignmentViewModel() { course = selectedCourse, assignment = newAss};

            return result;
        }

        public AssignmentAddViewModel GetAssignmentAddViewModelById(int id)
        {
            Assignment assignment = (from assignments in db.Assignments
                                where assignments.id == id
                                select assignments).SingleOrDefault();
            AssignmentAddViewModel model = new AssignmentAddViewModel()
            {
                name = assignment.name,
                description = assignment.description,
                startDate = assignment.startDate,
                endDate = assignment.endDate
            };

            return model;
        }

        public int GetCourseIdByAssignmentId(int assignmentId)
        {
            var courseId = (from ass in db.Assignments
                           where ass.id == assignmentId
                           select ass.courseId).SingleOrDefault();

            return courseId;
        }

        public bool AddAssignmet(int id, AssignmentAddViewModel model)
        {
            Assignment assignment = new Assignment()
            {
                courseId = id,
                name = model.name,
                startDate = model.startDate,
                endDate = model.endDate,
                description = model.description,
            };
            db.Assignments.Add(assignment);
            return Convert.ToBoolean( db.SaveChanges() );
        }

        public AssignmentIndexViewModel GetAll()
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
                                  orderby x.endDate ascending
                                  select x;

            var oldAss = from course in userCourses
                                  join ass in db.Assignments on course.id equals ass.courseId into result
                                  from x in result
                                  where x.endDate < currDate
                                  select x;

            var all = new AssignmentIndexViewModel() { newAssignments = newAss, oldAssignments = oldAss };

            return (all);
        }

        public bool Edit(int id, AssignmentAddViewModel model)
        {
            var item = (from assignments in db.Assignments
                        where assignments.id == id
                        select assignments).SingleOrDefault();

            item.startDate = model.startDate;
            item.name = model.name;
            item.description = model.description;
            item.endDate = model.endDate;
            
            return Convert.ToBoolean(db.SaveChanges());
        }

        public AssignmentProblems GetAssignmentProblems (int id)
        {

            var assignment = GetAssignmentById(id);

            var currentCourse = (from course in db.Courses
                                 where course.id == assignment.courseId
                                 select course).SingleOrDefault();

            var courseRole = (from role in db.CourseUsers
                              where currentCourse.id == role.courseId && role.userId == currentUser
                              select role).SingleOrDefault();

            var assignmentProblems = (from problems in db.Problems
                            where problems.assignmentId == assignment.id
                            select problems).AsQueryable();

            var result = new AssignmentProblems()
            {
                courseUser = courseRole,
                course = currentCourse,
                assignment = assignment,
                problems = assignmentProblems
            };

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