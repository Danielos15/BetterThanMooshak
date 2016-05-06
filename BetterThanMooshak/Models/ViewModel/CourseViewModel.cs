﻿using BetterThanMooshak.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.ViewModel
{
    public class CourseViewModel
    {
        public List<Course> courses { get; set; }
        public Course course { get; set; }
    }

    public class CourseAddViewModel
    {
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
}