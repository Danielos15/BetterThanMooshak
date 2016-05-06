using BetterThanMooshak.Models;
using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Models.ViewModel;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

            
            ApplicationUser appUser =   (from user in db.Users
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
    }
}