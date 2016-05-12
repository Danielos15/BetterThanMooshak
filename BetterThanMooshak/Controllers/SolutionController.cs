using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Models.ViewModel;
using BetterThanMooshak.Services;
using BetterThanMooshak.Utilities;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BetterThanMooshak.Controllers
{
    public class SolutionController : Controller
    {
        private ProblemService service = new ProblemService();

        #region Index Action - Get overview of all user Solutions
        public ActionResult Index()
        {
            return View(service.getAllSolutions());
        }
#endregion

        #region Save Action - Save a certain Solution
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(int? id, SolutionPostViewModel model)
        {
            if (id != null)
            {
                model.fileName = "main";
                Compiler compiler = new Compiler();
                compiler.SaveFile(model, User.Identity.GetUserId(), id.Value);
            }
            return View("404");
        }
#endregion

        #region Submit Action - Solution for a problem
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Submit(int? id, SolutionPostViewModel model)
        {
            if (id != null)
            {
                model.fileName = "main";
                List<Testcase> testcases = service.GetTestcasesByProblemId(id.Value);
                Compiler compiler = new Compiler();
                
                var output = compiler.Compile(model, testcases, User.Identity.GetUserId(), id.Value);
            }
            return View("404");
        }
        #endregion
    }
}