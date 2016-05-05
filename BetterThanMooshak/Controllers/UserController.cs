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
            UsersViewModel viewModel = service.GetAllUsers();
            return View(viewModel);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(UserAddViewModel newUser)
        {
            return View();
        }
        
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(UserEditViewModel user)
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
        public ActionResult Changepassword(UsersViewModel model)
        {
            return View();
        }
    }
}