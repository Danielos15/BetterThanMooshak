using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using BetterThanMooshak.Services;
using BetterThanMooshak.Models.ViewModel;

namespace BetterThanMooshak.Controllers
{
    public class HomeController : Controller
    {
        private FrontService service = new FrontService();

        public ActionResult Index()
        {
            FrontViewModel viewModel = service.getAll();

            return View(viewModel);
        }

        public ActionResult About()
        {
            // Diff from master
        
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}