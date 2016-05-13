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

        #region Function that gets all assignments
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
        #endregion

        #region Function that gets all grades for each user
        public bool AddGrade(GradeProblemAddViewModel newGrade)
        {
            if ((newGrade.grade < 0) || (newGrade.grade > 10))
                return false;

            var exists = (from g in db.Grades
                          where (g.assignmentId == newGrade.assignmentId) && (g.userId == newGrade.userId)
                          select g).SingleOrDefault();

            if (exists != null)
            {
                exists.grade = newGrade.grade;
            }
            else
            {
                var grade = new Grade
                {
                    assignmentId = newGrade.assignmentId,
                    grade = newGrade.grade,
                    userId = newGrade.userId,
                    gradedDate = DateTime.Now
                };

                db.Grades.Add(grade);
            }

            return Convert.ToBoolean(db.SaveChanges());
        }
        public GradeViewModel GetGradeViewModel(int value)
        {
            //Get assignment
            var assignment = GetAssignmentById(value);

            //Get List<GradeUserViewModel>
            List<GradeUserViewModel> students = new List<GradeUserViewModel>();
            
            var assignmentProblems = (from p in db.Problems
                           where p.assignmentId == assignment.id
                           select p).ToList();
                              
            var users = (from cu in db.CourseUsers
                             where (cu.courseId == assignment.courseId && (cu.role == 1))
                             join u in db.Users on cu.userId equals u.Id
                             select u).ToList();

            foreach (var user in users)
            {
                //Get List<GradeProblemViewModel>
                var problems = new List<GradeProblemViewModel>();

                var userSolutions = (from s in db.Solutions
                                    where s.userId == user.Id
                                    select s).ToList();

                                var assignmentGrade = (from g in db.Grades
                                       where (g.assignmentId == assignment.id) && (g.userId == user.Id)
                                       select g).SingleOrDefault();

                foreach (var problem in assignmentProblems)
                {
                    var submission = (from s in userSolutions
                                        where s.problemId == problem.id
                                        orderby s.score descending, s.submissionDate descending
                                        select s).FirstOrDefault();

                    problems.Add(new GradeProblemViewModel { submission = submission, problemName = problem.name, problemId = problem.id });
                }

                students.Add(new GradeUserViewModel { user = user, problems = problems, assignmentGrade = assignmentGrade });
            }

            var viewModel = new GradeViewModel {
                assignment = assignment,
                students = students
            };

            return viewModel;
        }

        #endregion

        /// <summary>
        /// Check whether a User is a Teacher for a Course
        /// with a given Assignment
        /// </summary>
        /// <param User="userId"></param>
        /// <param Assignment="assignmentId"></param>
        /// <returns>True if it's a teacher, false otherwise</returns>
        public bool isTeacher(string userId, int assignmentId)
        {
            var courseId = GetAssignmentById(assignmentId).courseId;

            var course = (from c in db.Courses
                          where c.id == courseId
                          select c).SingleOrDefault();

            var role = (from cu in db.CourseUsers
                        where (cu.courseId == course.id) && (cu.userId == userId)
                        select cu).SingleOrDefault();

            if (role == null)
            {
                return false;
            }
            else
            {
                return (role.role == 3);
            }
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

        #region Function that gets all problems within assignment
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
                            select problems).ToList();

            var listOfProblems = new List<AssignmentDetailsProblemViewModel>();
            foreach (var assignProb in assignmentProblems)
            {
                var best = (from x in db.Solutions
                            where x.userId == userId
                            && x.problemId == assignProb.id
                            orderby x.score descending,
                            x.submissionDate descending
                            select x).FirstOrDefault();

                var count = (from x in db.Solutions
                             where x.userId == userId
                             && x.problemId == assignProb.id
                             select x).Count();

                var maxScore = (from x in db.Testcases
                                where x.problemId == assignProb.id
                                select x.score).DefaultIfEmpty().Sum();

                var prob = new AssignmentDetailsProblemViewModel()
                {
                    assignmentId = assignProb.assignmentId,
                    description = assignProb.description,
                    maxAttempts = assignProb.maxAttempts,
                    id = assignProb.id,
                    percentOfGrade = assignProb.percentOfGrade,
                    name = assignProb.name,
                    currentAttempts = count,
                    currentScore = 0,
                    maxScore = maxScore
                };
                if (best != null)
                {
                    prob.currentScore = best.score;
                }
                listOfProblems.Add(prob);
            }

            var grade = (from g in db.Grades
                         where g.assignmentId == assignment.id && g.userId == userId
                         select g).SingleOrDefault();

            var result = new AssignmentProblems()
            {
                courseUser = courseRole,
                course = currentCourse,
                assignment = assignment,
                problems = listOfProblems,
                grade = grade
            };

            return result;
        }
        #endregion

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