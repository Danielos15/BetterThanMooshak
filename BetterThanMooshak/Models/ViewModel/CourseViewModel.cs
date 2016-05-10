using BetterThanMooshak.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.ViewModel
{
    public class CourseAddViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        [StringLength(100, ErrorMessage = "Please enter valid course name", MinimumLength = 2)]
        public string name { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:yyyy-dd-MM}", ApplyFormatInEditMode = true)]
        public DateTime startDate { get; set; }

        [Required]
        public DateTime endDate { get; set; }
    }

    public class CourseEditViewModel
    {
        public int id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        [StringLength(100, ErrorMessage = "Please enter valid course name", MinimumLength = 2)]
        public string name { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime startDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime endDate { get; set; }
    }
    
    public class CourseUserEnroleViewModel
    {
        public int courseId { get; set; }
        public string courseName { get; set; }
        public IQueryable<ApplicationUser> availableUsers { get; set; }
        public IQueryable<ApplicationUser> teachers { get; set; }
        public IQueryable<ApplicationUser> assistants { get; set; }
        public IQueryable<ApplicationUser> students { get; set; }
    }

    public class CourseUserEnroleSaveViewModel
    {
        public List<string> teachers { get; set; }
        public List<string> assistants { get; set; }
        public List<string> students { get; set; }
    }

    public class CourseAssignments
    {
        public Course course { get; set; }
        public IQueryable<Assignment> newAssignments { get; set; }
        public IQueryable<Assignment> oldAssignments { get; set; }
    }
    public class UserCoursesViewModel
    {
        public List<CourseWithRoles> activeCourses { get; set; }
        public List<CourseWithRoles> inactiveCourses { get; set; }
    }
    public class CourseWithRoles
    {
        public Course course { get; set; }
        public CourseUser courseUser { get; set; }
    }
}