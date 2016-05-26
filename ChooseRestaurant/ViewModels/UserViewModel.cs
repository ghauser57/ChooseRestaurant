using ChooseRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChooseRestaurant.ViewModels
{
    public class UserViewModel
    {
        public User User { get; set; }
        public bool Authenticated { get; set; }
    }
}