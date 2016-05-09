using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.Entities
{
    public class Testcase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int problemId { get; set; }
        public string input { get; set; }
        public string output { get; set; }
        public int score { get; set; }
        public bool visible { get; set; }
    }
}