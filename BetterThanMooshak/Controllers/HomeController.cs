using System.Web.Mvc;
using BetterThanMooshak.Services;
using BetterThanMooshak.Models.ViewModel;
using Microsoft.AspNet.Identity;

namespace BetterThanMooshak.Controllers
{
    public class HomeController : Controller
    {
        private HomeService service = new HomeService(null);
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            HomeViewModel viewModel = service.GetAll(userId);

            return View(viewModel);
        }
    }
}