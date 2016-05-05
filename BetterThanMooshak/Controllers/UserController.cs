using BetterThanMooshak.Models.ViewModel;
using BetterThanMooshak.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BetterThanMooshak.Controllers
{
    
    public class UserController : Controller
    {
        private UserService service = new UserService();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Overview()
        {
            UserViewModel viewModel = service.GetAllUsers();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Import()
        {
            return View();
        }

        public ActionResult Changepassword()
        {
            return View();
        }

        [HttpPost] //TODO: changePassword viewModel to pass inn.
        public ActionResult Changepassword(UserViewModel model)
        {
            return View();
        }
    }
}