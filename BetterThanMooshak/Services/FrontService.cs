﻿using BetterThanMooshak.Models;
using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Models.ViewModel;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Services
{
    public class FrontService
    {
        private ApplicationDbContext db;

        public FrontService()
        {
            db = new ApplicationDbContext();
        }

        public FrontViewModel getAll()
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

            var userAssignments = from course in db.Courses
                                  join ass in db.Assignments on course.id equals ass.courseId into result
                                  from x in result
                                  select x;

            var userNotifications = from courses in userCourses
                                    join notifications in db.Notifications on courses.id equals notifications.courseId into result
                                    from x in result
                                    select x;

            var userGrades = from grades in db.ProblemGrades
                             where grades.userId == appUser.Id
                             select grades;

            FrontViewModel front = new FrontViewModel() { user = appUser, courses = userCourses, assignments = userAssignments, notifications = userNotifications, recentGrades = userGrades };

            //COURSE USERCOURSE ASSIGNMENT - JOIN
            //COURSE USERCOURSE - JOIN

            return front;
        }
    }
}