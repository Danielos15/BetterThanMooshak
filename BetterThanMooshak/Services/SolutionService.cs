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

        public bool AddSolution(Solution s)
        {

            Solution temp = new Solution { Id = s.Id, userId = s.userId, problemId = s.problemId, program = s.program };
            db.Solutions.Add(temp);
            var save = db.SaveChanges();
            if (save != 0)
                return true;
            else
                return false;
        }

        public SolutionViewModel EditSolution(SolutionViewModel s)
        {
            return null;
        }

        public SolutionViewModel GetSolutionById(int id)
        {
            var solution = (from s in db.Solutions
                            where s.Id == id
                            select s).SingleOrDefault();

            SolutionViewModel result = new SolutionViewModel();
            result.solution = solution;

            return result;
        }

        public SolutionViewModel GetSolutionsByProblemAndUser(int pId, int uId)
        {
            var solution = (from x in db.Solutions
                            where x.Id == pId && x.Id == uId
                            select x).ToList();

            SolutionViewModel result = new SolutionViewModel();
            result.solutions = solution;

            return result;
        }

        public SolutionViewModel GetSolutionsByUser(int uId)
        {
            var solutions = (from x in db.Solutions
                             where x.Id == uId
                             select x).ToList();

            SolutionViewModel result = new SolutionViewModel();
            result.solutions = solutions;

            return result;
        }
    }
}
