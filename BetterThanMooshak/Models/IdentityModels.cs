using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Services;

namespace BetterThanMooshak.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        private static UserService test = null;
        private static UserService Test()
        {
            if (test == null)
            {
                test = new UserService();
            }
            return test;
        }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool Removable()
        {
            return Test().CanDeleteUser(this);
        }
    }

    public interface IAppDataContext
    {
        IDbSet<Assignment> Assignments { get; set; }
        IDbSet<BestSolution> BestSolutions { get; set; }
        IDbSet<Course> Courses { get; set; }
        IDbSet<CourseUser> CourseUsers { get; set; }
        IDbSet<DiscussionComment> DiscussionComments { get; set; }
        IDbSet<DiscussionTopic> DiscussionTopics { get; set; }
        IDbSet<Notification> Notifications { get; set; }
        IDbSet<Problem> Problems { get; set; }
        IDbSet<Grade> Grades { get; set; }
        IDbSet<Solution> Solutions { get; set; }
        IDbSet<Testcase> Testcases { get; set; }
        IDbSet<ApplicationUser> Users { get; set; }
        int SaveChanges();

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IAppDataContext
    {
        public ApplicationDbContext()
            : base("MooshakDB", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        
        public IDbSet<Assignment> Assignments { get; set; }
        public IDbSet<BestSolution> BestSolutions { get; set; }
        public IDbSet<Course> Courses { get; set; }
        public IDbSet<CourseUser> CourseUsers { get; set; }
        public IDbSet<DiscussionComment> DiscussionComments { get; set; }
        public IDbSet<DiscussionTopic> DiscussionTopics { get; set; }
        public IDbSet<Notification> Notifications { get; set; }
        public IDbSet<Problem> Problems { get; set; }
        public IDbSet<Grade> Grades { get; set; }
        public IDbSet<Solution> Solutions { get; set; }
        public IDbSet<Testcase> Testcases { get; set; }
        public IDbSet<Hint> Hints { get; set; }
    }
}