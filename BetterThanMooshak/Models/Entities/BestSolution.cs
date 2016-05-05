using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.Entities
{
    public class BestSolution
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int problemId { get; set; }
        public string userId { get; set; }
        public int solutionId { get; set; }
        public float grade { get; set; }
    }
}