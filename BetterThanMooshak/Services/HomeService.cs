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
    public class HomeService
    {
        private ApplicationDbContext db;
        private string currentUser = HttpContext.Current.User.Identity.GetUserId();
        public HomeService()
        {
            db = new ApplicationDbContext();
        }

        public HomeViewModel getAll()
        {
            var user = getUserName();

            var courses = getCourses();

            var assignments = getAssignments(ref courses);

            var notifications = getNotifications(ref courses);

            var grades = getGrades();

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
        private string getUserName()
        {
            var user = (from users in db.Users
                        where users.Id == currentUser
                        select users).SingleOrDefault();

            return user.Name;
        }
        private IQueryable<Course> getCourses()
        {
            var userCourses = from courseusers in db.CourseUsers
                              join courses in db.Courses on courseusers.courseId equals courses.id into result
                              where courseusers.userId == currentUser
                              from x in result
                              orderby x.name ascending
                              select x;

            return userCourses;
        }
        private List<AssignmentViewModel> getAssignments(ref IQueryable<Course> userCourses)
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
        private List<HomeNotification> getNotifications(ref IQueryable<Course> userCourses)
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
        private List<HomeGrade> getGrades()
        {
            var grades = (from g in db.Grades
                   where g.userId == currentUser
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
    }
}