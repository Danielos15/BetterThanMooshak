using BetterThanMooshak.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.ViewModel
{
    public class TestcaseViewModel
    {
        public List<Testcase> testcases { get; set; }
    }

    public class TestcaseAddViewModel
    {
        public int problemId { get; set; }
        public string input { get; set; }
        public string output { get; set; }
        public int score { get; set; }
        public bool visible { get; set; }
    }
}