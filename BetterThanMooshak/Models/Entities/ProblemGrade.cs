using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.Entities
{
    public class ProblemGrade
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int problemId { get; set; }
        public string userId { get; set; }
        public double grade { get; set; }
        public DateTime gradedDate { get; set; }

    }
}