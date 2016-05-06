using BetterThanMooshak.Models;
using BetterThanMooshak.Models.ViewModel;
using BetterThanMooshak.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BetterThanMooshak.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private UserService service = new UserService();        
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: User
        public ActionResult Index()
        {
            UsersViewModel viewModel = service.GetAllUsers();
            return View(viewModel);
        }

        // GET: Add User
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(UserAddViewModel newUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { UserName = newUser.email, Email = newUser.email, Name = newUser.name };
                IdentityResult result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    if (newUser.admin)
                    {
                        if (!service.IfRoleExists("Admin"))
                            service.AddRole("Admin");

                        UserManager.AddToRole(user.Id, "Admin");
                    }

                    return RedirectToAction("index", "user");
                }
                AddErrors(result);
            }
            return View(newUser);
        }
        
        // GET: Edit User
        public ActionResult Edit(string id)
        {
            ApplicationUser user = service.GetUserById(id);
            UserEditViewModel model = new UserEditViewModel()
            {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                admin = UserManager.IsInRole(user.Id, "Admin")
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserEditViewModel editUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(editUser.id);
                user.UserName = editUser.email;
                user.Email = editUser.email;
                user.Name = editUser.name;
                IdentityResult result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    if (editUser.admin)
                    {
                        if (!service.IfRoleExists("Admin"))
                            service.AddRole("Admin");

                        UserManager.AddToRole(user.Id, "Admin");
                    }
                    else
                    {
                        if (UserManager.IsInRole(user.Id, "Admin"))
                        UserManager.RemoveFromRole(user.Id, "Admin");
                    }
                    return RedirectToAction("index", "user");
                }
                AddErrors(result);
            }
            return View(editUser);
        }

        public async Task<ActionResult> SendEmailValidation()
        {

            var users = service.GetAllUsersAsEntity();
            foreach (var user in users)
            {
                if (!(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //TODO: change message content??
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                }
            }

            return RedirectToAction("index", "user");
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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}