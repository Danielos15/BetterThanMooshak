using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.Entities
{
    public class Grade
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int assignmentId { get; set; }
        public string userId { get; set; }
        public double grade { get; set; }
        public DateTime gradedDate { get; set; }
    }
}