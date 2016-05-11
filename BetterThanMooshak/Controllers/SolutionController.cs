using BetterThanMooshak.Models.ViewModel;
using BetterThanMooshak.Services;
using System.Web.Mvc;

namespace BetterThanMooshak.Controllers
{
    public class SolutionController : Controller
    {
        private SolutionService service = new SolutionService();

        #region Submit Action - Solution for a problem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Submit(int? id, SolutionPostViewModel model)
        {
            if (id != null)
            {

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