using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChooseRestaurant.Models
{
    [Table("Restos")]
    public class Resto// : IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The restaurant need a name")]
        public string Name { get; set; }
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "The phone number is not correct")]
        public string Phone { get; set; }
        /*public string Email { get; set; }
        [Required]
        public string Location { get; set; }
        public string Details { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Phone) && string.IsNullOrWhiteSpace(Email))
                yield return new ValidationResult("You have to enter at least one way to contact the restaurant", new[] { "Telephone", "Email" });
            // etc.
        }*/
    }
}