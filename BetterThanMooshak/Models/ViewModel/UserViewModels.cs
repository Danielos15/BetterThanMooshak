using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.ViewModel
{
    public class UsersViewModel
    {
        public List<ApplicationUser> users { get; set; }
    }

    public class UserAddViewModel
    {
        public string name { get; set; }
        public string email { get; set; }
    }

    public class UserEditViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }
}