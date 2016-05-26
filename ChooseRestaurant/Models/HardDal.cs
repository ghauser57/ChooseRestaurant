using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChooseRestaurant.Models
{
    public class HardDal : IDal
    {
        private List<Resto> ListOfRestaurants;
        private List<User> ListOfUsers;
        private List<Survey> ListOfSurveys;

        public HardDal()
        {
            ListOfRestaurants = new List<Resto>
            {
                new Resto { Id = 1, Name = "Resto pinambour", Phone = "0102030405"},
                new Resto { Id = 2, Name = "Resto pinière", Phone = "0102030405"},
                new Resto { Id = 3, Name = "Resto toro", Phone = "0102030405"},
            };
            ListOfUsers = new List<User>();
            ListOfSurveys = new List<Survey>();
        }

        public List<Resto> GetAllRestaurants()
        {
            return ListOfRestaurants;
        }

        public void CreateRestaurant(string Name, string Phone)
        {
            int id = ListOfRestaurants.Count == 0 ? 1 : ListOfRestaurants.Max(r => r.Id) + 1;
            ListOfRestaurants.Add(new Resto { Id = id, Name = Name, Phone = Phone });
        }

        public void ModifyRestaurant(int id, string Name, string Phone)
        {
            Resto resto = ListOfRestaurants.FirstOrDefault(r => r.Id == id);
            if (resto != null)
            {
                resto.Name = Name;
                resto.Phone = Phone;
            }
        }

        public bool RestaurantExist(string Name)
        {
            return ListOfRestaurants.Any(resto => string.Compare(resto.Name, Name, StringComparison.CurrentCultureIgnoreCase) == 0);
        }

        public int AddUser(string Name, string Password)
        {
            int id = ListOfUsers.Count == 0 ? 1 : ListOfUsers.Max(u => u.Id) + 1;
            ListOfUsers.Add(new User { Id = id, Login = Name, Password = Password });
            return id;
        }

        public User Authenticate(string Name, string Password)
        {
            return ListOfUsers.FirstOrDefault(u => u.Login == Name && u.Password == Password);
        }

        public User GetUser(int id)
        {
            return ListOfUsers.FirstOrDefault(u => u.Id == id);
        }

        public User GetUser(string idStr)
        {
            int id;
            if (int.TryParse(idStr, out id))
                return GetUser(id);
            return null;
        }

        public int CreateSurvey()
        {
            int id = ListOfSurveys.Count == 0 ? 1 : ListOfSurveys.Max(s => s.Id) + 1;
            ListOfSurveys.Add(new Survey { Id = id, Date = DateTime.Now, Votes = new List<Vote>() });
            return id;
        }

        public void AddVote(int idSurvey, int idResto, int idUser)
        {
            Vote vote = new Vote
            {
                Resto = ListOfRestaurants.First(r => r.Id == idResto),
                User = ListOfUsers.First(u => u.Id == idUser)
            };
            Survey Survey = ListOfSurveys.First(s => s.Id == idSurvey);
            Survey.Votes.Add(vote);
        }

        public bool HasVoted(int idSurvey, string idStr)
        {
            User User = GetUser(idStr);
            if (User == null)
                return false;
            Survey Survey = ListOfSurveys.First(s => s.Id == idSurvey);
            return Survey.Votes.Any(v => v.User.Id == User.Id);
        }

        public List<Results> GetAllResults(int idSurvey)
        {
            List<Resto> restaurants = GetAllRestaurants();
            List<Results> Results = new List<Results>();
            Survey Survey = ListOfSurveys.First(s => s.Id == idSurvey);
            foreach (IGrouping<int, Vote> grouping in Survey.Votes.GroupBy(v => v.Resto.Id))
            {
                int idRestaurant = grouping.Key;
                Resto resto = restaurants.First(r => r.Id == idRestaurant);
                int NumberOfVotes = grouping.Count();
                Results.Add(new Results { Name = resto.Name, Phone = resto.Phone, NumberOfVotes = NumberOfVotes });
            }
            return Results;
        }

        public void Dispose()
        {
            ListOfRestaurants = new List<Resto>();
            ListOfUsers = new List<User>();
            ListOfSurveys = new List<Survey>();
        }
    }
}