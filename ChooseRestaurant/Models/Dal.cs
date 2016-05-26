using ChooseRestaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ChooseRestaurant.Models
{
    public class Dal : IDal
    {
        private DataBaseContext db;

        public Dal()
        {
            db = new DataBaseContext();
        }

        public List<Resto> GetAllRestaurants()
        {
            return db.Restos.ToList();
        }

        public void CreateRestaurant(string name, string phone)
        {
            db.Restos.Add(new Resto { Name = name, Phone = phone });
            db.SaveChanges();
        }

        public void ModifyRestaurant(int id, string name, string phone)
        {
            Resto restoFound = db.Restos.FirstOrDefault(resto => resto.Id == id);
            if (restoFound != null)
            {
                restoFound.Name = name;
                restoFound.Phone = phone;
                db.SaveChanges();
            }
        }

        public bool RestaurantExist(string name)
        {
            return db.Restos.Any(resto => string.Compare(resto.Name, name, StringComparison.CurrentCultureIgnoreCase) == 0);
        }

        public int AddUser(string name, string password)
        {
            string passwordEncode = EncodeMD5(password);
            User user = new User { Login = name, Password = passwordEncode };
            db.Users.Add(user);
            db.SaveChanges();
            return user.Id;
        }

        public User Authenticate(string name, string password)
        {
            string passwordEncode = EncodeMD5(password);
            return db.Users.FirstOrDefault(u => u.Login == name && u.Password == passwordEncode);
        }

        public User GetUser(int id)
        {
            return db.Users.FirstOrDefault(u => u.Id == id);
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
            Survey survey = new Survey { Date = DateTime.Now };
            db.Surveys.Add(survey);
            db.SaveChanges();
            return survey.Id;
        }

        public void AddVote(int idSurvey, int idResto, int idUser)
        {
            Vote vote = new Vote
            {
                Resto = db.Restos.First(r => r.Id == idResto),
                User = db.Users.First(u => u.Id == idUser)
            };
            Survey Survey = db.Surveys.First(s => s.Id == idSurvey);
            if (Survey.Votes == null)
                Survey.Votes = new List<Vote>();
            Survey.Votes.Add(vote);
            db.SaveChanges();
        }

        public List<Results> GetAllResults(int idSurvey)
        {
            List<Resto> restaurants = GetAllRestaurants();
            List<Results> results = new List<Results>();
            Survey survey = db.Surveys.First(s => s.Id == idSurvey);
            foreach (IGrouping<int, Vote> grouping in survey.Votes.GroupBy(v => v.Resto.Id))
            {
                int idRestaurant = grouping.Key;
                Resto resto = restaurants.First(r => r.Id == idRestaurant);
                int NumberOfVotes = grouping.Count();
                results.Add(new Results { Name = resto.Name, Phone = resto.Phone, NumberOfVotes = NumberOfVotes });
            }
            return results;
        }

        public bool HasVoted(int idSurvey, string idStr)
        {
            int id;
            if (int.TryParse(idStr, out id))
            {
                Survey survey = db.Surveys.First(s => s.Id == idSurvey);
                if (survey.Votes == null)
                    return false;
                return survey.Votes.Any(v => v.User != null && v.User.Id == id);
            }
            return false;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        private string EncodeMD5(string password)
        {
            string passwordSel = "ChoixResto" + password + "ASP.NET MVC";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(passwordSel)));
        }
    }
}