using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.Entities
{
    public class Solution
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string userId { get; set; }
        public int problemId { get; set; }
        public int score { get; set; }
        public int maxScore { get; set; }
        public DateTime submissionDate { get; set; }
    }
}