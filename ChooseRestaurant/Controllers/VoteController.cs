using ChooseRestaurant.Models;
using ChooseRestaurant.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChooseRestaurant.Controllers
{
    public class VoteController : Controller
    {
        // GET: Vote
        private IDal dal;

        public VoteController() : this(new Dal())
        {
        }

        public VoteController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        public ActionResult Index(int id)
        {
            RestaurantVoteViewModel viewModel = new RestaurantVoteViewModel
            {
                ListOfResto = dal.GetAllRestaurants().Select(r => new RestaurantCheckBoxViewModel { Id = r.Id, NameAndPhone = string.Format("{0} ({1})", r.Name, r.Phone) }).ToList()
            };
            if (dal.HasVoted(id, Request.Browser.Browser))
            {
                return RedirectToAction("ShowResults", new { id = id });
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(RestaurantVoteViewModel viewModel, int id)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            User User = dal.GetUser(Request.Browser.Browser);
            if (User == null)
                return new HttpUnauthorizedResult();
            foreach (RestaurantCheckBoxViewModel restaurantCheckBoxViewModel in viewModel.ListOfResto.Where(r => r.IsSelected))
            {
                dal.AddVote(id, restaurantCheckBoxViewModel.Id, User.Id);
            }
            return RedirectToAction("ShowResults", new { id = id });
        }

        public ActionResult ShowResults(int id)
        {
            if (!dal.HasVoted(id, Request.Browser.Browser))
            {
                return RedirectToAction("Index", new { id = id });
            }
            List<Results> Results = dal.GetAllResults(id);
            return View(Results.OrderByDescending(r => r.NumberOfVotes).ToList());
        }
    }
}