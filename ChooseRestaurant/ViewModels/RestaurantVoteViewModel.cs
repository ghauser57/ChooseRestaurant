using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChooseRestaurant.ViewModels
{
    public class RestaurantVoteViewModel : IValidatableObject
    {
        public List<RestaurantCheckBoxViewModel> ListOfResto { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ListOfResto.Any(r => r.IsSelected))
                yield return new ValidationResult("You have to choose at least one restaurant", new[] { "ListOfResto" });
        }
    }
}