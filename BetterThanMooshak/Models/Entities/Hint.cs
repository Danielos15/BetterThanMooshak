using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.Entities
{
    public class Hint
    {
        public int id { get; set; }
        public int problemId { get; set; }
        public string message { get; set; }
        public string title { get; set; }
        public DateTime date { get; set; }
    }
}