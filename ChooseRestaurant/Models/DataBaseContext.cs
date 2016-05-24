using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ChooseRestaurant.Models
{
    public class DataBaseContext : DbContext
    {
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Resto> Restos { get; set; }
        public DbSet<User> Users { get; set; }
    }
}