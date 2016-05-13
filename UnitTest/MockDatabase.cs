using System.Data.Entity;
using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Models;
using InMemoryDbSet.Tests;

namespace UnitTest.Tests
{
    public class MockDataContext : IAppDataContext
    {
        /// <summary>
        /// Sets up the fake database.
        /// </summary>
        public MockDataContext()
        {
            // We're setting our DbSets to be InMemoryDbSets rather than using SQL Server.
            this.Assignments = new InMemoryDbSet<Assignment>();
            this.Courses = new InMemoryDbSet<Course>();
            this.CourseUsers = new InMemoryDbSet<CourseUser>();
            this.DiscussionComments = new InMemoryDbSet<DiscussionComment>();
            this.DiscussionTopics = new InMemoryDbSet<DiscussionTopic>();
            this.Notifications = new InMemoryDbSet<Notification>();
            this.Problems = new InMemoryDbSet<Problem>();
            this.Grades = new InMemoryDbSet<Grade>();
            this.Solutions = new InMemoryDbSet<Solution>();
            this.Testcases = new InMemoryDbSet<Testcase>();
            this.Users = new InMemoryDbSet<ApplicationUser>();
            this.Hints = new InMemoryDbSet<Hint>();
        }

        public IDbSet<Assignment> Assignments { get; set; }
        public IDbSet<Course> Courses { get; set; }
        public IDbSet<CourseUser> CourseUsers { get; set; }
        public IDbSet<DiscussionComment> DiscussionComments { get; set; }
        public IDbSet<DiscussionTopic> DiscussionTopics { get; set; }
        public IDbSet<Notification> Notifications { get; set; }
        public IDbSet<Problem> Problems { get; set; }
        public IDbSet<Grade> Grades { get; set; }
        public IDbSet<Solution> Solutions { get; set; }
        public IDbSet<Testcase> Testcases { get; set; }
        public IDbSet<ApplicationUser> Users { get; set; }
        public IDbSet<Hint> Hints { get; set; }
        // TODO: bætið við fleiri færslum hér
        // eftir því sem þeim fjölgar í AppDataContext klasanum ykkar!

        public int SaveChanges()
        {
            // Pretend that each entity gets a database id when we hit save.
            int changes = 0;

            return changes;
        }

        public void Dispose()
        {
            // Do nothing!
        }
    }
}
