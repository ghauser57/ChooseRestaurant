using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChooseRestaurant.Models
{
    public interface IDal : IDisposable
    {
        void CreateRestaurant(string name, string phone/*, string email, string location, string details*/);
        void ModifyRestaurant(int id, string name, string phone/*, string email, string location, string details*/);
        List<Resto> GetAllRestaurants();
        bool RestaurantExist(string nom);

        int AddUser(string name, string password);
        User Authenticate(string nom, string motDePasse);
        User GetUser(int id);
        User GetUser(string idStr);

        int CreateSurvey();
        void AddVote(int idSurvey, int idResto, int idUser);
        List<Results> GetAllResults(int idSurvey);
        bool HasVoted(int idSurvey, string idStr);
    }
}