using BetterThanMooshak.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BetterThanMooshak.Models.ViewModel
{
    public class AssignmentViewModel
    {
        public Course course { get; set; }
        public Assignment assignment { get; set; }
    }

    public class AssignmentAddViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name...")]
        [StringLength(100, ErrorMessage = "Please enter valid assignment name", MinimumLength = 2)]
        public string name { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:yyyy-dd-MM}", ApplyFormatInEditMode = true)]
        public DateTime startDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:yyyy-dd-MM}", ApplyFormatInEditMode = true)]
        public DateTime endDate { get; set; }

        [Display(Name = "Description")]
        public string description { get; set; }
    }

    public class AssignmentIndexViewModel
    {
        public List<AssignmentViewModel> oldAssignments;
        public List<AssignmentViewModel> newAssignments;
    }

    public class AssignmentProblems
    {
        public Course course { get; set; }
        public Assignment assignment { get; set; }
        public IQueryable<Problem> problems { get; set; }
        public CourseUser courseUser {get; set;}
    }
}