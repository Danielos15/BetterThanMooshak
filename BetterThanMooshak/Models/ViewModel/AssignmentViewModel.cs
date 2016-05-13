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
        public Grade grade { get; set; }
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
        public CourseUser courseUser { get; set; }
        public Grade grade { get; set; }
        public List<AssignmentDetailsProblemViewModel> problems { get; set; }
    }

    public class AssignmentDetailsProblemViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int percentOfGrade { get; set; }
        public int maxAttempts { get; set; }
        public int assignmentId { get; set; }

        public int maxScore { get; set; }
        public int currentScore { get; set; }
        public int currentAttempts { get; set; }

    }

    public class GradeViewModel
    {
        public Assignment assignment { get; set; }
        public List<GradeUserViewModel> students { get; set; }
    }
    public class GradeUserViewModel
    {
        public ApplicationUser user { get; set; }
        public List<GradeProblemViewModel> problems { get; set; }
        public Grade assignmentGrade { get; set; }
    }
    public class GradeProblemViewModel
    {
        public Solution submission { get; set; }
        public string problemName { get; set; }
        public int problemId { get; set; }
    }
    public class GradeProblemAddViewModel
    {
        public string userId { get; set; }
        public int assignmentId { get; set; }
        public float grade { get; set; }
    }
}