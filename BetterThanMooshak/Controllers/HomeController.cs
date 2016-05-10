using System.Web.Mvc;
using BetterThanMooshak.Services;
using BetterThanMooshak.Models.ViewModel;

namespace BetterThanMooshak.Controllers
{
    public class HomeController : Controller
    {
        private HomeService service = new HomeService();
        public ActionResult Index()
        {
            HomeViewModel viewModel = service.getAll();

            return View(viewModel);
        }
    }
}