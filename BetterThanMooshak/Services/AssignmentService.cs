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
        private readonly IAppDataContext db;

        public AssignmentService(IAppDataContext context)
        {
            db = context ?? new ApplicationDbContext();
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

        public AssignmentIndexViewModel GetAll(string userId)
        {
            var teacherCourses = from cu in db.CourseUsers
                              join c in db.Courses on cu.courseId equals c.id into result
                              where cu.userId == userId && cu.role == 3
                              from x in result
                              select x;

            var teacherAss = (from course in teacherCourses
                              join ass in db.Assignments on course.id equals ass.courseId into result
                              from x in result
                              where x.endDate > DateTime.Now
                              orderby x.endDate ascending
                              select x).ToList();

            var userCourses = from cu in db.CourseUsers
                              join c in db.Courses on cu.courseId equals c.id into result
                              where cu.userId == userId && cu.role < 3
                              from x in result
                              select x;

            var userAss = (from course in userCourses
                                  join ass in db.Assignments on course.id equals ass.courseId into result
                                  from x in result
                                  where x.endDate > DateTime.Now && x.startDate < DateTime.Now
                                  orderby x.endDate ascending
                                  select x).ToList();

            userAss.AddRange(teacherAss);

            userAss = (from a in userAss
                       orderby a.endDate ascending
                       select a).ToList();

            var newCourses = (from a in userAss
                join c in db.Courses on a.courseId equals c.id
                select c).ToList();
           

            var newAssignments = new List<AssignmentViewModel> {};

            for (int i = 0; i < userAss.Count; i++)
            {
                newAssignments.Add(new AssignmentViewModel
                {
                    assignment = userAss.ElementAt(i),
                    course = newCourses.ElementAt(i)
                });
            }

            var oldAss = (from course in userCourses
                                  join ass in db.Assignments on course.id equals ass.courseId into result
                                  from x in result
                                  where x.endDate < DateTime.Now
                                  select x).ToList();

            var oldCourses = (from a in oldAss
                              join c in db.Courses on a.courseId equals c.id
                              select c).ToList();

            var oldGrades = (from a in oldAss
                             join g in db.Grades on a.id equals g.assignmentId
                             where g.userId == userId
                             select g).ToList();

            var oldAssignments = new List<AssignmentViewModel> { };

            for (int i = 0; i < oldAss.Count; i++)
            {
                Grade currGrade = new Grade { grade = -1 };
                
                foreach (var g in oldGrades)
                {
                    if (g.assignmentId == oldAss.ElementAt(i).id)
                        currGrade = g;
                }

                oldAssignments.Add(new AssignmentViewModel
                {
                    assignment = oldAss.ElementAt(i),
                    course = oldCourses.ElementAt(i),
                    grade = currGrade
                });
            }

            var model = new AssignmentIndexViewModel() { newAssignments = newAssignments, oldAssignments = oldAssignments };

            return (model);
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

        public AssignmentProblems GetAssignmentProblems (int id, string userId)
        {
            var assignment = GetAssignmentById(id);

            var currentCourse = (from course in db.Courses
                                 where course.id == assignment.courseId
                                 select course).SingleOrDefault();

            var courseRole = (from role in db.CourseUsers
                              where currentCourse.id == role.courseId && role.userId == userId
                              select role).SingleOrDefault();

            var assignmentProblems = (from problems in db.Problems
                            where problems.assignmentId == assignment.id
                            select problems).AsQueryable();

            var grade = (from g in db.Grades
                         where g.assignmentId == assignment.id && g.userId == userId
                         select g).SingleOrDefault();

            var result = new AssignmentProblems()
            {
                courseUser = courseRole,
                course = currentCourse,
                assignment = assignment,
                problems = assignmentProblems,
                grade = grade
            };

            return result;
        }

        public bool verifyUser(int id, string userId)
        {
            var appUser = (from user in db.Users
                           where user.Id == userId
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