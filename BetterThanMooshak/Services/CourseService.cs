using BetterThanMooshak.Models;
using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Models.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace BetterThanMooshak.Services
{
    public class CourseService
    {
        private readonly IAppDataContext db;
        //private string currentUser = "1";
        private string currentUser = HttpContext.Current.User.Identity.GetUserId();

        public CourseService(IAppDataContext context)
        {
            db = context ?? new ApplicationDbContext();
        }

        public Course GetCourseById(int id)
        {
            return      (from c in db.Courses
                        where c.id == id
                        select c).SingleOrDefault();
        }

        public IQueryable<Course> GetAllCourses()
        {
            return      (from c in db.Courses
                        select c);
        }

        public bool Add(CourseAddViewModel newCourse)
        {
            db.Courses.Add(new Course()
            {
                name = newCourse.name,
                startDate = newCourse.startDate,
                endDate = newCourse.endDate
            });

            return Convert.ToBoolean(db.SaveChanges());
        }

        public CourseAddViewModel GetCourseEditViewModel(int value)
        {
            Course course = GetCourseById(value);

            CourseAddViewModel viewModel = new CourseAddViewModel()
            {
                name = course.name,
                startDate = course.startDate,
                endDate = course.endDate
            };

            return viewModel;
        }

        public bool Edit(int id, CourseAddViewModel editCourse)
        {
            var item = (from course in db.Courses
                        where course.id == id
                        select course).SingleOrDefault();

            item.name =         editCourse.name;
            item.startDate =    editCourse.startDate;
            item.endDate =      editCourse.endDate;

            return Convert.ToBoolean(db.SaveChanges());
        }

        public bool CanDeleteCourse(Course course)
        {
            var exists = (from x in db.CourseUsers
                          where x.courseId == course.id
                          select x).FirstOrDefault();
            if (exists != null)
            {
                return false;
            }

            return true;
        }
        public bool RemoveCourseById(int id)
        {
            db.Courses.Remove(GetCourseById(id));

            return Convert.ToBoolean(db.SaveChanges());
        }

        public UserCoursesViewModel GetUserCourses()
        {
            var newCourses = (from courseusers in db.CourseUsers
                              join courses in db.Courses on courseusers.courseId equals courses.id
                              where courseusers.userId == currentUser && courses.endDate > DateTime.Now
                              orderby courses.endDate ascending
                              select courses).ToList();

            var newCoursesRoles = (from courses in newCourses
                                   join roles in db.CourseUsers on courses.id equals roles.courseId
                                   where roles.userId == currentUser
                                   select roles).ToList();

            List<CourseWithRoles> activeCourses = new List<CourseWithRoles> { };

            for (int i = 0; i < newCourses.Count; i++)
            {
                activeCourses.Add(new CourseWithRoles {
                    course = newCourses.ElementAt(i),
                    courseUser = newCoursesRoles.ElementAt(i)
                });
            }

            var oldCourses = (from courseusers in db.CourseUsers
                              join courses in db.Courses on courseusers.courseId equals courses.id into result
                              where courseusers.userId == currentUser
                              from x in result
                              where x.endDate < DateTime.Now
                              select x).ToList();

            var oldCoursesRoles = (from courses in oldCourses
                                   join roles in db.CourseUsers on courses.id equals roles.courseId
                                   where roles.userId == currentUser
                                   select roles).ToList();

            List<CourseWithRoles> inactiveCourses = new List<CourseWithRoles> { };

            for (int i = 0; i < oldCourses.Count; i++)
            {
                inactiveCourses.Add(new CourseWithRoles {
                    course = oldCourses.ElementAt(i),
                    courseUser = oldCoursesRoles.ElementAt(i)
                });
            }
            
            return new UserCoursesViewModel { activeCourses = activeCourses, inactiveCourses = inactiveCourses };
        }

        public CourseAssignments GetCourseAssignments(int? id)
        {
            var isTeacher = (from cu in db.CourseUsers
                where cu.courseId == id.Value && cu.userId == currentUser
                select cu).SingleOrDefault().role;

            IQueryable<Assignment> newAssignments;

            if (isTeacher == 3)
            {
                newAssignments =    from course in db.Courses
                                        join ass in db.Assignments on course.id equals ass.courseId into result
                                        where course.id == id
                                        from x in result
                                        where x.endDate > DateTime.Now
                                        select x;
            }
            else
            {
                newAssignments = from course in db.Courses
                                     join ass in db.Assignments on course.id equals ass.courseId into result
                                     where course.id == id
                                     from x in result
                                     where x.endDate > DateTime.Now && x.startDate < DateTime.Now
                                     select x;
            }

            var oldAssignments = from course in db.Courses
                                 join ass in db.Assignments on course.id equals ass.courseId into result
                                 where course.id == id
                                 from x in result
                                 where x.endDate < DateTime.Now
                                 select x;

            var role = (from uc in db.CourseUsers
                        where uc.userId == currentUser && uc.courseId == id.Value
                        select uc).SingleOrDefault();

            var viewModel = new CourseAssignments {
                course = GetCourseById(id.Value),
                newAssignments = newAssignments,
                oldAssignments = oldAssignments,
                role = role
            };

            return viewModel;
        }

        #region Functions for getting the Enrole viewModel
        public IQueryable<ApplicationUser> GetAvalibleUsersForCourse(int courseId)
        {
            var usersInCourse = from link in db.CourseUsers
                                 join users in db.Users on link.userId equals users.Id into result
                                 where link.courseId == courseId
                                 from user in result
                                 select user;

            var allUsers = from users in db.Users
                            select users;

            var usersNotInCourse = allUsers.Except(usersInCourse);


            return usersNotInCourse;
        }
        public IQueryable<ApplicationUser> GetTeachersForCourse(int courseId)
        {
            var teachers = from link in db.CourseUsers
                            join users in db.Users on link.userId equals users.Id into result
                            where link.courseId == courseId
                            && link.role == 3
                            from user in result
                            select user;

            return teachers;
        }
        public IQueryable<ApplicationUser> GetAssistantsForCourse(int courseId)
        {
            var assistants = from link in db.CourseUsers
                              join users in db.Users on link.userId equals users.Id into result
                              where link.courseId == courseId
                              && link.role == 2
                              from user in result
                              select user;

            return assistants;
        }
        public IQueryable<ApplicationUser> GetStudentsForCourse(int courseId)
        {
            var students =   from link in db.CourseUsers
                              join users in db.Users on link.userId equals users.Id into result
                              where link.courseId == courseId
                              && link.role == 1
                              from user in result
                              select user;

            return students;
        }
        public CourseUserEnroleViewModel GetEnroleViewModel(int value)
        {
            Course course = GetCourseById(value);

            CourseUserEnroleViewModel model = new CourseUserEnroleViewModel()
            {
                courseId = course.id,
                courseName = course.name,
                availableUsers = GetAvalibleUsersForCourse(value),
                teachers = GetTeachersForCourse(value),
                assistants = GetAssistantsForCourse(value),
                students = GetStudentsForCourse(value)
            };

            return model;
        }
        #endregion

        //Save new User Roles
        public bool RemoveAllFromCourse(int courseId)
        {
            IQueryable<CourseUser> courses = from link in db.CourseUsers
                                             where link.courseId == courseId
                                             select link;

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

        #region Functions for adding users to course
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
        #endregion

        /// <summary>Import courses with csv
        /// <para>Input : Filebase file containing the course data. Output : List with CourseAddViewModels</para>
        /// </summary>
        public List<CourseAddViewModel> ImportCourses(HttpPostedFileBase file)
        {
            var reader = new StreamReader(file.InputStream);

            int rowCount = 0;
            List<string> nameList = new List<string>();
            int nameIndex = 0;
            List<DateTime> startDateList = new List<DateTime>();
            int startDateIndex = 0;
            List<DateTime> endDateList = new List<DateTime>();
            int endDateIndex = 0;

            var firstLine = reader.ReadLine();
            var types = firstLine.Split(';');

            for (int i = 0; i < types.Length; i++)
            {
                switch (types[i])
                {
                    case "name":
                        nameIndex = i;
                        break;
                    case "startdate":
                        startDateIndex = i;
                        break;
                    case "enddate":
                        endDateIndex = i;
                        break;
                }
            }

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                nameList.Add(values[nameIndex]);
                startDateList.Add(Convert.ToDateTime(values[startDateIndex]));
                endDateList.Add(Convert.ToDateTime(values[endDateIndex]));

                rowCount++;
            }

            List<CourseAddViewModel> courses = new List<CourseAddViewModel>();

            for (int i = 0; i < rowCount; i++)
            {
                courses.Add(new CourseAddViewModel
                {
                    name = nameList.ElementAt(i),
                    startDate = startDateList.ElementAt(i),
                    endDate = endDateList.ElementAt(i)
                });

            }

            return courses;
        }
    }
}
