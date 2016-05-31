using ChooseRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChooseRestaurant.Controllers
{
    public class RestaurantController : Controller
    {
        private IDal dal;

        public RestaurantController() : this(new Dal())
        {
        }

        public RestaurantController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        public ActionResult Index()
        {
            List<Resto> listeDesRestaurants = dal.GetAllRestaurants();
            return View(listeDesRestaurants);
        }

        public ActionResult CreateRestaurant()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateRestaurant(Resto resto)
        {
            if (dal.RestaurantExist(resto.Name))
            {
                ModelState.AddModelError("Name", "This name of restaurant already exist");
                return View(resto);
            }
            if (!ModelState.IsValid)
                return View(resto);
            dal.CreateRestaurant(resto.Name, resto.Phone);
            return RedirectToAction("Index");
        }

        public ActionResult ModifyRestaurant(int? id)
        {
            if (id.HasValue)
            {
                Resto restaurant = dal.GetAllRestaurants().FirstOrDefault(r => r.Id == id.Value);
                if (restaurant == null)
                    return View("Error");
                return View(restaurant);
            }
            else
                return HttpNotFound();
        }

        [HttpPost]
        public ActionResult ModifyRestaurant(Resto resto)
        {
            if (!ModelState.IsValid)
                return View(resto);
            dal.ModifyRestaurant(resto.Id, resto.Name, resto.Phone);
            return RedirectToAction("Index");
        }

        public ActionResult ShowDetailsRestaurant(Resto resto)
        {
            if (!ModelState.IsValid)
                return View(resto);
            dal.ModifyRestaurant(resto.Id, resto.Name, resto.Phone);
            return RedirectToAction("Index");
        }
    }
}