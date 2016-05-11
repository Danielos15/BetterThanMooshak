using BetterThanMooshak.Models.ViewModel;
using BetterThanMooshak.Services;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace BetterThanMooshak.Controllers
{
    public class SolutionController : Controller
    {
        private SolutionService service = new SolutionService();


        [HttpPost]
        public ActionResult Save(int? id, SolutionPostViewModel model)
        {
            if (id != null)
            {
                string userId = User.Identity.GetUserId();
                if (!service.SaveSolution(id.Value, userId, model))
                {
                    return View("404");
                }
            }
            return View("404");
        }

        #region Submit Action - Solution for a problem
        [HttpPost]
        public ActionResult Submit(int? id, SolutionPostViewModel model)
        {
            if (id != null)
            {
                //service.AddSolution(id.Value);
            }
            return View("404");
        }

        public ActionResult Index()
        {
            return View(service.getAllSolutions());
        }
        #endregion

    }
}