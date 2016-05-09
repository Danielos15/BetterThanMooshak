using BetterThanMooshak.Models;
using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Models.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Services
{
    public class CourseService
    {
        private ApplicationDbContext db;

        public CourseService()
        {
            db = new ApplicationDbContext();
        }

        public Course GetCourseById(int id)
        {
            var course = (from c in db.Courses
                          where c.id == id
                          select c).SingleOrDefault();

            Course result = new Course();
            result = course;

            return result;
        }

        public IQueryable<Course> GetAllCourses()
        {
            var courses = from c in db.Courses
                           select c;

            return courses;
        }

        public bool Add(CourseAddViewModel newCourse)
        {
            Course temp = new Course() { name = newCourse.name, startDate = newCourse.startDate, endDate = newCourse.endDate };
            db.Courses.Add(temp);
            return Convert.ToBoolean(db.SaveChanges());
        }

        public bool RemoveCourseById (int id)
        {
            db.Courses.Remove(GetCourseById(id));

            return Convert.ToBoolean(db.SaveChanges());
        }
        public bool Edit(CourseEditViewModel editCourse)
        {
            var item = (from course in db.Courses
                        where course.id == editCourse.id
                        select course).SingleOrDefault();

            item.name = editCourse.name;
            item.startDate = editCourse.startDate;
            item.endDate = editCourse.endDate;

            return Convert.ToBoolean(db.SaveChanges());
        }

        public UserCoursesViewModel GetUserCourses()
        {
            var currentUser = HttpContext.Current.User.Identity.GetUserId();

            var newCourses = (from courseusers in db.CourseUsers
                              join courses in db.Courses on courseusers.courseId equals courses.id into result
                              where courseusers.userId == currentUser
                              from x in result
                              where x.endDate > DateTime.Now
                              select x).ToList();

            var newCoursesRoles = (from courses in newCourses
                                   join roles in db.CourseUsers on courses.id equals roles.courseId into result
                                   from x in result
                                   select x).ToList();

            List<CourseWithRoles> activeCourses = new List<CourseWithRoles> { };

            for (int i = 0; i < newCourses.Count; i++)
            {
                activeCourses.Add(new CourseWithRoles { course = newCourses.ElementAt(i), courseUser = newCoursesRoles.ElementAt(i) });
            }

            var oldCourses = (from courseusers in db.CourseUsers
                              join courses in db.Courses on courseusers.courseId equals courses.id into result
                              where courseusers.userId == currentUser
                              from x in result
                              where x.endDate < DateTime.Now
                              select x).ToList();

            var oldCoursesRoles = (from courses in oldCourses
                                   join roles in db.CourseUsers on courses.id equals roles.courseId into result
                                   from x in result
                                   select x).ToList();

            List<CourseWithRoles> inactiveCourses = new List<CourseWithRoles> { };

            for (int i = 0; i < oldCourses.Count; i++)
            {
                inactiveCourses.Add(new CourseWithRoles { course = oldCourses.ElementAt(i), courseUser = oldCoursesRoles.ElementAt(i) });
            }
            
            return new UserCoursesViewModel { activeCourses = activeCourses, inactiveCourses = inactiveCourses };
        }

        public CourseAssignments GetCourseAssignments(int? id)
        {
            CourseAssignments viewModel = new CourseAssignments();
            

            var userAssignments = (from course in db.Courses
                                  join ass in db.Assignments on course.id equals ass.courseId into result
                                  where course.id == id
                                  from x in result
                                  select x).ToList();

            viewModel.assignments = userAssignments;
            viewModel.course = GetCourseById(id.Value);
            return viewModel;
        }

        public void SearchCourse(string searchString)
        {
            var searchCourse = (from c in db.Courses
                                where c.name.Contains(searchString)
                                select c).ToList();
        }

        //Get current User Roles
        public List<ApplicationUser> GetAvalibleUsersForCourse(int courseId)
        {
            var usersInCourse = (from link in db.CourseUsers
                                 join users in db.Users on link.userId equals users.Id into result
                                 where link.courseId == courseId
                                 from user in result
                                 select user).ToList();

            var allUsers = (from users in db.Users
                            select users).ToList();

            var usersNotInCourse = allUsers.Except(usersInCourse).ToList();


            return usersNotInCourse;
        }
        public List<ApplicationUser> GetTeachersForCourse(int courseId)
        {
            var teachers = (from link in db.CourseUsers
                            join users in db.Users on link.userId equals users.Id into result
                            where link.courseId == courseId
                            && link.role == 3
                            from user in result
                            select user).ToList();

            return teachers;
        }
        public List<ApplicationUser> GetAssistantsForCourse(int courseId)
        {
            var assistants = (from link in db.CourseUsers
                              join users in db.Users on link.userId equals users.Id into result
                              where link.courseId == courseId
                              && link.role == 2
                              from user in result
                              select user).ToList();

            return assistants;
        }
        public List<ApplicationUser> GetStudentsForCourse(int courseId)
        {
            var students =   (from link in db.CourseUsers
                              join users in db.Users on link.userId equals users.Id into result
                              where link.courseId == courseId
                              && link.role == 1
                              from user in result
                              select user).ToList();

            return students;
        }

        //Save new User Roles
        public bool RemoveAllFromCourse(int courseId)
        {
            List<CourseUser> courses = (from link in db.CourseUsers
                                        where link.courseId == courseId
                                        select link).ToList();
            if (courses != null)
            {
                foreach (CourseUser link in courses)
                {
                    db.CourseUsers.Remove(link);
                }
                return Convert.ToBoolean(db.SaveChanges());
            }
            return false;
        }
        public bool AddTeacherToCourse(string userId, int courseId)
        {
            CourseUser link = new CourseUser()
            {
                userId = userId,
                courseId = courseId,
                role = 3
            };
            db.CourseUsers.Add(link);
            return Convert.ToBoolean(db.SaveChanges()); ;
        }
        public bool AddAssistantToCourse(string userId, int courseId)
        {
            CourseUser link = new CourseUser()
            {
                userId = userId,
                courseId = courseId,
                role = 2
            };
            db.CourseUsers.Add(link);
            return Convert.ToBoolean(db.SaveChanges()); ;
        }
        public bool AddStudentToCourse(string userId, int courseId)
        {
            CourseUser link = new CourseUser()
            {
                userId = userId,
                courseId = courseId,
                role = 1
            };
            db.CourseUsers.Add(link);
            return Convert.ToBoolean(db.SaveChanges()); ;
        }
    }
}
