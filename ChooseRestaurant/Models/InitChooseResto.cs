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
            context.Restos.Add(new Resto { Id = 1, Name = "Resto pinambour", Phone = "0123265948"/*, Email = "", Location = "17 Fox Hollow", Details = "" */});
            context.Restos.Add(new Resto { Id = 2, Name = "Resto pinière", Phone = "0456154878"/*, Email = "", Location = "17 Fox Hollow", Details = "" */});
            context.Restos.Add(new Resto { Id = 3, Name = "Resto toro", Phone = "0789003654"/*, Email = "", Location = "17 Fox Hollow", Details = "" */});

            context.Users.Add(new User { Id = 1, Login = "aze", Password = "123" });
            context.Users.Add(new User { Id = 2, Login = "qsd", Password = "456" });
            context.Users.Add(new User { Id = 3, Login = "wxc", Password = "789" });

            base.Seed(context);
        }
    }
}