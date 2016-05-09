using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.Entities
{
    public class Problem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int percentOfGrade { get; set; }
        public int maxAttempts { get; set; }
        public int assignmentId { get; set; }
    }
}