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

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("MooshakDB", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<BestSolution> BestSolutions { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseUser> CourseUsers { get; set; }
        public DbSet<DiscussionComment> DiscussionComments { get; set; }
        public DbSet<DiscussionTopic> DiscussionTopics { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<ProblemGrade> ProblemGrades { get; set; }
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<Testcase> Testcases { get; set; }
    }
}