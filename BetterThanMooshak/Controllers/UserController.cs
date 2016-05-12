using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BetterThanMooshak.Models;
using BetterThanMooshak.Models.ViewModel;
using BetterThanMooshak.Services;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using BetterThanMooshak.Utilities;
using System.Threading.Tasks;

namespace BetterThanMooshak.Controllers
{
    //So only Admins can view these Actions
    [CustomAuthorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private UserService service = new UserService();        
        private ApplicationUserManager _userManager;

        #region UserManager setup
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
        #endregion

        #region Index of all users for Admins
        // GET: User
        public ActionResult Index()
        {
            if (TempData["errorMessage"] != null)
            {
                ViewBag.errorMessage = TempData["errorMessage"].ToString();
            }
            if (TempData["message"] != null)
            {
                ViewBag.message = TempData["message"].ToString();
            }

            UsersViewModel viewModel = service.GetAllUsers();

            return View(viewModel);
        }
        #endregion

        #region Add user Actions
        // GET: Add User
        public ActionResult Add()
        {
            return View();
        }

        // POST: Add User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(UserAddViewModel newUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser {
                    UserName = newUser.email,
                    Email = newUser.email,
                    Name = newUser.name,
                    Active = true
                };
                IdentityResult result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    if (newUser.admin)
                    {
                        if (!service.IfRoleExists("Admin"))
                            service.AddRole("Admin");

                        UserManager.AddToRole(user.Id, "Admin");
                    }
                    TempData["message"] = newUser.name + " has been added.";
                    return RedirectToAction("index", "user");
                }
                AddErrors(result);
            }
            return View(newUser);
        }
        #endregion

        #region Edit user Actions
        // GET: Edit User
        public ActionResult Edit(string id)
        {
            if (id != null)
            {
                ApplicationUser user = service.GetUserById(id);
                UserAddViewModel model = new UserAddViewModel()
                {
                    name = user.Name,
                    email = user.Email,
                    admin = UserManager.IsInRole(user.Id, "Admin")
                };
                return View(model);
            }
            return View("404");
        }

        // POST: Edit User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, UserAddViewModel editUser)
        {
            if (id != null)
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser user = await UserManager.FindByIdAsync(id);
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
            return View("404");
        }
        #endregion

        #region Send Email Validation to all that are not Validated
        // GET: Send Email Validation
        public async Task<ActionResult> SendEmailValidation()
        {
            TempData["message"] = "Validation E-mail sent to : ";
            var users = service.GetAllUsersAsEntity();
            foreach (var user in users)
            {
                if (!(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    TempData["message"] += user.Email + ", ";
                }
            }

            return RedirectToAction("index", "user");
        }
        #endregion

        #region Remove user Action
        // Get: Remove User
        public async Task<ActionResult> Remove(string id)
        {
            if (id != null)
            {
                ApplicationUser user = await UserManager.FindByIdAsync(id);
                if (service.CanDeleteUser(user))
                {
                    var result = UserManager.Delete(user);
                    if (result.Succeeded)
                    {
                        TempData["message"] = user.Name + " has be deleted from the system";
                    }
                    else
                    {
                        TempData["errorMessage"] = "An error occured while trying to delete " + user.Name;
                    }
                }
                return RedirectToAction("index", "user");
            }
            return View("404");
        }
        #endregion

        #region Activate or Deactivate User
        // GET: Activate User
        public async Task<ActionResult> Active(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user.Active)
            {
                user.Active = false;
                UserManager.Update(user);
                TempData["message"] = user.Name + " has be deactivated and can no longer login.";
            } 
            else
            {
                user.Active = true;
                UserManager.Update(user);
                TempData["message"] = user.Name + " has be activated and can now login.";
            }
            
            return RedirectToAction("index", "user");
        }
        #endregion

        #region Import users from .csv file
        [HttpPost]
        public async Task<ActionResult> Import(HttpPostedFileBase inputFileBase)
        {
            var users = service.ImportUsers(inputFileBase);

            foreach (var newUser in users)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = newUser.email,
                    Email = newUser.email,
                    Name = newUser.name,
                    Active = true
                };

                IdentityResult result = await UserManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    if (newUser.admin)
                    {
                        UserManager.AddToRole(user.Id, "Admin");
                    }
                }
                else
                {
                    AddErrors(result);
                }
            }

            TempData["message"] = "Users have been imported!";

            return RedirectToAction("index");
        }
        #endregion

        #region Admin can confirm e-mail for users if needed
        public async Task<ActionResult> EmailConfirm(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user.EmailConfirmed)
            {
                user.EmailConfirmed = false;
                UserManager.Update(user);
                TempData["message"] = user.Name + "'s Email confirmation disabled";
            }
            else
            {
                user.EmailConfirmed = true;
                UserManager.Update(user);
                TempData["message"] = user.Name + "'s Email has been confirmed";
            }

            return RedirectToAction("index", "user");
        }
        #endregion

        #region Helper function to add errors to ModelState
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        #endregion
    }
}