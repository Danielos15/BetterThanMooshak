using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.ViewModel
{
    public class UsersViewModel
    {
        public List<ApplicationUser> users { get; set; }
    }

    public class UserIndexSingleViewModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public bool emailComfirmed { get; set; }
        public bool active { get; set; }
        public bool removable { get; set; }

        UserIndexSingleViewModel(ApplicationUser user)
        {
            id = user.Id;
            name = user.Name;
            email = user.Email;
            emailComfirmed = user.EmailConfirmed;
            active = user.Active;
        }
    }

    public class UserAddViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        [StringLength(100, ErrorMessage = "Type in your name, god damn it!", MinimumLength = 2)]
        public string name { get; set; }

        [Display(Name = "Admin")]
        public bool admin { get; set; }
    }
}