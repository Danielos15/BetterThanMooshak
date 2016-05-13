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
    public class ProblemService
    {
        private ApplicationDbContext db;

        public ProblemService()
        {
            db = new ApplicationDbContext();
        }
        public Problem GetProblemById(int problemId)
        {
            return (from p in db.Problems
                    where p.id == problemId
                    select p).SingleOrDefault();
        }

        internal bool verifyUser(int value, string userId)
        {
            var problem = (from problems in getAllProblems(userId)
                           where problems.id == value
                           select problems).SingleOrDefault();

            if (problem == null)
                return false;
            else
                return true;
        }

        public ProblemAddViewModel Initialize(int? id)
        {
            if (id != null)
            {
                var assignment = (from assignments in db.Assignments
                                  where assignments.id == id.Value
                                  select assignments).SingleOrDefault();

                var course = (from x in db.Courses
                              where x.id == assignment.courseId
                              select x).SingleOrDefault();

                ProblemAddViewModel model = new ProblemAddViewModel()
                {
                    assignmentId = assignment.id,
                    assignmentName = assignment.name,
                    courseName = course.name,
                };
                return model;
            }
            
            return null;
        }

        public bool Edit(int id, ref ProblemAddViewModel problem)
        {
            var p = GetProblemById(id);

            p.name = problem.name;
            p.maxAttempts = problem.maxAttempts;
            p.percentOfGrade = problem.percentOfGrade;
            p.description = problem.description;

            problem.assignmentId = p.assignmentId;

            return Convert.ToBoolean(db.SaveChanges());
        }

        public ProblemAddViewModel GetProblemEditViewModel(int id)
        {
            Problem problem = GetProblemById(id);

            ProblemAddViewModel viewModel = new ProblemAddViewModel()
            {
                name = problem.name,
                maxAttempts = problem.maxAttempts,
                percentOfGrade = problem.percentOfGrade,
                description = problem.description,
                assignmentId = problem.assignmentId
            };

            return viewModel;
        }

        public IQueryable<Problem> getAllProblems(string userId)
        {
            var problems = from cu in db.CourseUsers
                           join c in db.Courses on cu.courseId equals c.id into userCourses
                           where cu.userId == userId
                           from course in userCourses
                           join a in db.Assignments on course.id equals a.courseId into assignments
                           from ass in assignments
                           join p in db.Problems on ass.id equals p.assignmentId into result
                           from x in result
                           select x;
            
            return problems;
        }

        internal bool canDeleteProblem(Problem problem)
        {
            //TODO when hints are implemented
            // var existHints = ...

            var existDiscussion = (from t in db.DiscussionTopics
                          where t.problemId == problem.id
                          select t).FirstOrDefault();

            var existSolution = (from s in db.Solutions
                                 where s.problemId == problem.id
                                 select s).FirstOrDefault();

            var existTestcase = (from t in db.Testcases
                                 where t.problemId == problem.id
                                 select t).FirstOrDefault();

            if ((existDiscussion != null) || (existSolution != null) || (existTestcase != null))
            {
                return false;
            }

            return true;
        }
    

    public bool deleteProblem(Problem problem)
        {
            db.Problems.Remove(GetProblemById(problem.id));

            return Convert.ToBoolean(db.SaveChanges());
        }
        
        public bool AddProblem(Problem add)
        {
            db.Problems.Add(add);

            return Convert.ToBoolean(db.SaveChanges());
        }

        public ProblemDetailsViewModel getDetails(int value, string userId)
        {
            var problem = GetProblemById(value);

            var assignment = (from problems in db.Problems
                              join assignments in db.Assignments on problems.assignmentId equals assignments.id into a
                              where problems.id == value
                              from ass in a
                              select ass).SingleOrDefault();

            var course = (from a in db.Assignments
                          join c in db.Courses on a.courseId equals c.id into courses
                          where a.id == assignment.id
                          from co in courses
                          select co).SingleOrDefault();

            var userRole = (from cu in db.CourseUsers
                where cu.courseId == course.id && cu.userId == userId
                            select cu).SingleOrDefault();

            var isTeacher = userRole.role == 3;

            var currSolution = new Solution { problemId = problem.id, userId = userId };

            var testcases = (from t in db.Testcases
                             where t.problemId == problem.id
                             && t.visible == true
                             select t).AsQueryable();

            var submissions = (from s in db.Solutions
                               where s.userId == userId && s.problemId == problem.id
                               orderby s.submissionDate descending
                               select s).AsQueryable();

            IQueryable<string> hints = null; //TODO

            var topics = (from d in db.DiscussionTopics
                          where d.problemId == problem.id
                          select d).ToList();

            DiscussionViewModel discussions = new DiscussionViewModel() {
                problemId = problem.id,
                topics = new List<DiscussionTopicViewModel>()
            };
            foreach (var topic in topics)
            {
                DiscussionTopicViewModel topicModel = new DiscussionTopicViewModel()
                {
                    problemId = problem.id,
                    topicId = topic.id,
                    title = topic.title,
                    message = topic.message,
                    userName = (from user in db.Users
                                where user.Id == topic.userId
                                select user.Name).SingleOrDefault(),

                    comments = from comment in db.DiscussionComments
                               where comment.discussionTopicId == topic.id
                               select comment
                };
                discussions.topics.Add(topicModel);
            }

            //var answer = "";

            var viewModel = new ProblemDetailsViewModel()
            {
                course = course.name,
                courseId = course.id,
                assignment = assignment,
                problem = problem,
                currSolution = currSolution,
                testcases = testcases,
                submissions = submissions,
                hints = hints,
                discussions = discussions,
                //answer = answer,
                isTeacher = isTeacher
            };

            return viewModel;
        }

        public bool AddTestcase(TestcaseAddViewModel model)
        {
            Testcase testcase = new Testcase()
            {
                input = model.input,
                output = model.output,
                problemId = model.problemId,
                score = model.score,
                visible = model.visible
            };
            db.Testcases.Add(testcase);
            return Convert.ToBoolean(db.SaveChanges());
        }

        public bool AddTopic(DisscussionAddTopicViewModel model, int problemId, string userId)
        {
            DiscussionTopic topic = new DiscussionTopic()
            {
                title = model.title,
                message = model.message,
                userId = userId,
                problemId = problemId
            };
            db.DiscussionTopics.Add(topic);
            return Convert.ToBoolean(db.SaveChanges());
        }

        public bool AddComment(DisscussionAddCommentViewModel model, int topicId, string userId)
        {
            DiscussionComment comment = new DiscussionComment()
            {
                message = model.message,
                userId = userId,
                discussionTopicId = topicId
            };
            db.DiscussionComments.Add(comment);
            return Convert.ToBoolean(db.SaveChanges());
        }

        public List<Testcase> GetTestcasesByProblemId(int id)
        {
            var testcases = (from testcase in db.Testcases
                             where testcase.problemId == id
                             select testcase).ToList();
            return testcases;
        }

        public bool AddSolution(Solution solution)
        {
            db.Solutions.Add(solution);
            return Convert.ToBoolean(db.SaveChanges());
        }
    }
}