using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChooseRestaurant.Models
{
    [Table("Restos")]
    public class Resto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The restaurant need a name")]
        public string Name { get; set; }
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "The phone number is not correct")]
        public string Phone { get; set; }
    }
}