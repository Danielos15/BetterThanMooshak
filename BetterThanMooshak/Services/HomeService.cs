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
    public class HomeService
    {
        private readonly IAppDataContext db;

        public HomeService(IAppDataContext context)
        {
            db = context ?? new ApplicationDbContext();
        }

        #region Get functions for Home view
        public HomeViewModel GetAll(string userId)
        {
            var user = GetUserName(userId);

            var courses = GetCourses(userId);

            var assignments = GetAssignments(ref courses);

            var notifications = GetNotifications(ref courses);

            var grades = GetGrades(userId);

            HomeViewModel viewModel = new HomeViewModel()
            {
                userName = user,
                courses = courses,
                assignments = assignments,
                notifications = notifications,
                grades = grades
            };
            
            return viewModel;
        }
        private string GetUserName(string userId)
        {
            var user = (from users in db.Users
                        where users.Id == userId
                        select users).SingleOrDefault();

            return user.Name;
        }
        private IQueryable<Course> GetCourses(string userId)
        {
            var userCourses = (from cu in db.CourseUsers
                              join c in db.Courses on cu.courseId equals c.id into result
                              where cu.userId == userId
                               from x in result
                              where x.endDate > DateTime.Now
                              orderby x.endDate
                              select x).Take(5);

            return userCourses;
        }
        private List<AssignmentViewModel> GetAssignments(ref IQueryable<Course> userCourses)
        {
            var assignments = (from course in userCourses
                               join ass in db.Assignments on course.id equals ass.courseId into result
                               from x in result
                               where x.endDate > DateTime.Now
                               orderby x.endDate ascending
                               select x).Take(5).ToList();

            var assignmentCourses = (from a in assignments
                                     join c in db.Courses on a.courseId equals c.id into courses
                                     from x in courses
                                     select x).ToList();

            var userAssignments = new List<AssignmentViewModel> { };

            for (int i = 0; i < assignments.Count; i++)
            {
                userAssignments.Add(
                    new AssignmentViewModel
                    {
                        assignment = assignments.ElementAt(i),
                        course = assignmentCourses.ElementAt(i)
                    });
            }

            return userAssignments;
        }
        private List<HomeNotification> GetNotifications(ref IQueryable<Course> userCourses)
        {
            var notifications = (from c in userCourses
                                 join a in db.Assignments on c.id equals a.courseId into x
                                 from y in x
                                 join n in db.Notifications on y.id equals n.assignmentId into result
                                 from z in result
                                 select z).Take(5).ToList();

            var assignments = (from n in notifications
                               join a in db.Assignments on n.assignmentId equals a.id into result
                               from x in result
                               select x).ToList();

            var homeNotifications = new List<HomeNotification> { };

            for (int i = 0; i < notifications.Count; i++)
            {
                homeNotifications.Add(new HomeNotification
                {
                    notification = notifications.ElementAt(i),
                    assignment = assignments.ElementAt(i)
                });
            }

            return homeNotifications;
        }
        private List<HomeGrade> GetGrades(string userId)
        {
            var grades = (from g in db.Grades
                   where g.userId == userId
                          select g).Take(5).ToList();

            var assignments = (from g in grades
                               join a in db.Assignments on g.assignmentId equals a.id
                               select a).ToList();

            var courses = (from a in assignments
                           join c in db.Courses on a.courseId equals c.id
                           select c).ToList();

            var homeGrades = new List<HomeGrade> { };

            for (int i = 0; i < grades.Count; i++)
            {
                homeGrades.Add(new HomeGrade
                {
                    grade = grades.ElementAt(i),
                    assignment = assignments.ElementAt(i),
                    course = courses.ElementAt(i)
                });
            }

            return homeGrades;
        }
        #endregion
    }
}