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

    public class UserEditViewModel
    {
        public string id { get; set; }

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