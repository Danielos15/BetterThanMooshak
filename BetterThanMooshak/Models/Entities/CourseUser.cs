using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.Entities
{
    public class CourseUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string userId { get; set; }
        public int courseId { get; set; }
        public int role { get; set; }
    }
}