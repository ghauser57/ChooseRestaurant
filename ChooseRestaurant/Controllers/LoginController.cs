using ChooseRestaurant.Models;
using ChooseRestaurant.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ChooseRestaurant.Controllers
{
    public class LoginController : Controller
    {
        private IDal dal;

        public LoginController() : this(new Dal())
        {

        }

        private LoginController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        public ActionResult Index()
        {
            UserViewModel viewModel = new UserViewModel { Authenticated = HttpContext.User.Identity.IsAuthenticated };
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                viewModel.User = dal.GetUser(HttpContext.User.Identity.Name);
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(UserViewModel viewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User User = dal.Authenticate(viewModel.User.Login, viewModel.User.Password);
                if (User != null)
                {
                    FormsAuthentication.SetAuthCookie(User.Id.ToString(), false);
                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return Redirect("/");
                }
                ModelState.AddModelError("User.Login", "Prénom et/ou mot de passe incorrect(s)");
            }
            return View(viewModel);
        }

        public ActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAccount(User user)
        {
            if (ModelState.IsValid)
            {
                int id = dal.AddUser(user.Login, user.Password);
                FormsAuthentication.SetAuthCookie(id.ToString(), false);
                return Redirect("/");
            }
            return View(user);
        }

        public ActionResult Deconnexion()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
    }
}