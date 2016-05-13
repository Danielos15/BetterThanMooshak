using BetterThanMooshak.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Models.ViewModel
{
    public class SolutionViewModel
    {
        public List<Solution> solutions { get; set; }
        public Solution solution { get; set; }
    }

    public class SolutionPostViewModel
    {
        public string code { get; set; }
        public string fileName { get; set; }
    }

    public class SoultionCompareViewMode
    {
        public string input { get; set; }
        public string output { get; set; }
        public string expectedOutput { get; set; }
        public int score { get; set; }
        public bool isVisible { get; set; }
        public bool isCorrect { get; set; }
    }
    public class SolutionPostJson
    {
        public List<SoultionCompareViewMode> tests { get; set; }
        public string errorMessage { get; set; }
        public bool hasCompileError { get; set; }
        public int totalScore { get; set; }
        public int maxScore { get; set; }
    }
}