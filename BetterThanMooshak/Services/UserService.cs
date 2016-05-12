using BetterThanMooshak.Models;
using BetterThanMooshak.Models.ViewModel;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace BetterThanMooshak.Services
{
    public class UserService
    {
        private ApplicationDbContext db;

        public UserService()
        {
            db = new ApplicationDbContext();
        }

        public ApplicationUser GetUserById(string id)
        {
            ApplicationUser appUser = (from user in db.Users
                where user.Id == id
                select user).SingleOrDefault();
            return appUser;
        }

        public List<ApplicationUser> GetAllUsersAsEntity()
        {
            var users = (from user in db.Users
                orderby user.Name ascending
                select user).ToList();
            return users;
        }

        public UsersViewModel GetAllUsers()
        {
            UsersViewModel model = new UsersViewModel();
            model.users = (from user in db.Users
                orderby user.Name ascending
                select user).ToList();
            return model;
        }

        public bool IfRoleExists(string role)
        {
            var roles = (from Role in db.Roles
                where Role.Name == role
                select role).FirstOrDefault();

            return !(roles == null);
        }

        public void AddRole(string role)
        {
            db.Roles.Add(new IdentityRole(role));
            db.SaveChangesAsync();
        }

        public bool CanDeleteUser(ApplicationUser user)
        {
            var exists = (from x in db.CourseUsers
                where x.userId == user.Id
                select x).FirstOrDefault();
            if (exists != null)
            {
                return false;
            }

            return true;
        }

        public List<UserAddViewModel> ImportUsers(HttpPostedFileBase file)
        {
            var reader = new StreamReader(file.InputStream);

            int rowCount = 0;
            List<string> usernameList = new List<string>();
            int usernameIndex = 0;
            List<string> emaiList = new List<string>();
            int emailIndex = 0;
            List<string> adminList = new List<string>();
            int adminIndex = 0;

            var firstLine = reader.ReadLine();
            var types = firstLine.Split(';');

            for (int i = 0; i < types.Length; i++)
            {
                switch (types[i])
                {
                    case "name":
                        usernameIndex = i;
                        break;
                    case "email":
                        emailIndex = i;
                        break;
                    case "admin":
                        adminIndex = i;
                        break;
                }
            }

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                usernameList.Add(values[usernameIndex]);
                emaiList.Add(values[emailIndex]);
                adminList.Add(values[adminIndex]);

                rowCount++;
            }

            List<UserAddViewModel> users = new List<UserAddViewModel>();

            for (int i = 0; i < rowCount; i++)
            {
                users.Add(new UserAddViewModel
                {
                    email = emaiList.ElementAt(i),
                    admin = (adminList.ElementAt(i) == "1"),
                    name = usernameList.ElementAt(i)
                });

            }

            return users;
        }
    }
}