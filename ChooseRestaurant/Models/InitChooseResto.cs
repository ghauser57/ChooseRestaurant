using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ChooseRestaurant.Models
{
    public class InitChooseResto : DropCreateDatabaseAlways<DataBaseContext>
    {
        protected override void Seed(DataBaseContext context)
        {
            context.Restos.Add(new Resto { Id = 1, Name = "Resto pinambour", Phone = "0123265948" });
            context.Restos.Add(new Resto { Id = 2, Name = "Resto pinière", Phone = "0456154878" });
            context.Restos.Add(new Resto { Id = 3, Name = "Resto toro", Phone = "0789003654" });

            base.Seed(context);
        }
    }
}