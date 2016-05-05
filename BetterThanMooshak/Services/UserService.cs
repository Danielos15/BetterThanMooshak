using BetterThanMooshak.Models;
using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Models.ViewModel;
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
            ApplicationUser appUser = (from user in db.Users
                                    where user.Id == id
                                    select user).SingleOrDefault();
            return appUser;
        }

        public UsersViewModel GetAllUsers()
        {
            UsersViewModel users = new UsersViewModel();
            users.users = (from user in db.Users
                            select user).ToList();
            return users;
        }
    }
}