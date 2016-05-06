using BetterThanMooshak.Models;
using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterThanMooshak.Services
{
    public class SolutionService
    {
        private ApplicationDbContext db;

        public SolutionService()
        {
            db = new ApplicationDbContext();
        }

        public SolutionViewModel AddSolution(SolutionViewModel s)
        {
            return null;
        }

        public SolutionViewModel EditSolution(SolutionViewModel s)
        {
            return null;
        }

        public SolutionViewModel GetSolutionById(int id)
        {
            var solution = (from s in db.Solutions where s.Id == id select s).SingleOrDefault();

            SolutionViewModel result = new SolutionViewModel();
            result.solution = solution;

            return result;
        }

        public SolutionViewModel GetSolutionsByProblemAndUser(int pId, int uId)
        {
            var solution = (from x in db.Solutions where x.Id == pId && x.Id == uId select x).SingleOrDefault();

            SolutionViewModel result = new SolutionViewModel();
            result.solution = solution;

            return result;
        }
    }
}
