using BetterThanMooshak.Models.Entities;
using BetterThanMooshak.Models.ViewModel;
using BetterThanMooshak.Services;
using BetterThanMooshak.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;

namespace BetterThanMooshak.Controllers
{
    public class SolutionController : Controller
    {
        private ProblemService service = new ProblemService(null);

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
                var totalSubmissions = service.CountSubmissions(User.Identity.GetUserId(), id.Value);
                var problem = service.GetProblemById(id.Value);
                if (totalSubmissions >= problem.maxAttempts)
                {
                    SolutionPostJson jSon = new SolutionPostJson()
                    {
                        errorMessage = "Max attempts reached",
                        maxAttemptsReach = true
                    };
                    return Json(jSon);
                }

                model.fileName = "main";
                List<Testcase> testcases = service.GetTestcasesByProblemId(id.Value);
                Compiler compiler = new Compiler();

                SolutionPostJson jsonObject = compiler.Compile(model, testcases, User.Identity.GetUserId(), id.Value);

                Solution solution = new Solution()
                {
                    problemId = id.Value,
                    userId = User.Identity.GetUserId(),
                    score = jsonObject.totalScore,
                    maxScore = jsonObject.maxScore,
                    submissionDate = DateTime.Now
                };
                service.AddSolution(solution);

                var visibleTest = (from test in jsonObject.tests
                               where test.isVisible == true
                               select test).ToList();
                jsonObject.tests = visibleTest;
                if (Request.IsAjaxRequest())
                {
                    return Json(jsonObject);
                }
            }
            return View("404");
        }
        #endregion

        [HttpPost]
        public ActionResult Load(int? id)
        {
            if (id != null)
            {
                string fileName = "main.cpp";
                FileHandler handler = new FileHandler();
                string json = handler.GetFileContentByUserAndProblem(User.Identity.GetUserId(), id.Value, fileName);
                
                if (Request.IsAjaxRequest())
                {
                    return Json(json);
                }
            }
            return View("404");
        }
    }
}