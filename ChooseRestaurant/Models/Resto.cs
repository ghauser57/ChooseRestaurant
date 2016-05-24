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
        [Required]
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}