using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChooseRestaurant.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ChooseRestaurant.Tests
{

    [TestClass]
    public class DalTests
    {
        private IDal dal;

        [TestInitialize]
        public void Init_AvantChaqueTest()
        {
            IDatabaseInitializer<DataBaseContext> init = new DropCreateDatabaseAlways<DataBaseContext>();
            Database.SetInitializer(init);
            init.InitializeDatabase(new DataBaseContext());

            dal = new Dal();
        }

        [TestCleanup]
        public void ApresChaqueTest()
        {
            dal.Dispose();
        }

        [TestMethod]
        public void CreateRestaurant_AvecUnNouveauRestaurant_GetAllRestaurantsRenvoitBienLeRestaurant()
        {
            dal.CreateRestaurant("La bonne fourchette", "0102030405");
            List<Resto> restos = dal.GetAllRestaurants();

            Assert.IsNotNull(restos);
            Assert.AreEqual(1, restos.Count);
            Assert.AreEqual("La bonne fourchette", restos[0].Name);
            Assert.AreEqual("0102030405", restos[0].Phone);
        }

        [TestMethod]
        public void ModifyRestaurant_CreationDUnNouveauRestaurantEtChangementNameEtPhone_LaModificationEstCorrecteApresRechargement()
        {
            dal.CreateRestaurant("La bonne fourchette", "0102030405");
            dal.ModifyRestaurant(1, "La bonne cuillère", null);

            List<Resto> restos = dal.GetAllRestaurants();
            Assert.IsNotNull(restos);
            Assert.AreEqual(1, restos.Count);
            Assert.AreEqual("La bonne cuillère", restos[0].Name);
            Assert.IsNull(restos[0].Phone);
        }

        [TestMethod]
        public void RestaurantExist_AvecCreationDunRestauraunt_RenvoiQuilExist()
        {
            dal.CreateRestaurant("La bonne fourchette", "0102030405");

            bool exist = dal.RestaurantExist("La bonne fourchette");

            Assert.IsTrue(exist);
        }

        [TestMethod]
        public void RestaurantExist_AvecRestaurauntInexistant_RenvoiQuilExist()
        {
            bool exist = dal.RestaurantExist("La bonne fourchette");

            Assert.IsFalse(exist);
        }

        [TestMethod]
        public void GetUser_UserInexistant_RetourneNull()
        {
            User user = dal.GetUser(1);
            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUser_IdNonNumerique_RetourneNull()
        {
            User user = dal.GetUser("abc");
            Assert.IsNull(user);
        }

        [TestMethod]
        public void AddUser_NewUserEtRecuperation_LUserEstBienRecupere()
        {
            dal.AddUser("New User", "12345");

            User user = dal.GetUser(1);

            Assert.IsNotNull(user);
            Assert.AreEqual("New User", user.Login);

            user = dal.GetUser("1");

            Assert.IsNotNull(user);
            Assert.AreEqual("New User", user.Login);
        }

        [TestMethod]
        public void Authenticate_LoginMdpOk_AuthentificationOK()
        {
            dal.AddUser("New User", "12345");

            User user = dal.Authenticate("New User", "12345");

            Assert.IsNotNull(user);
            Assert.AreEqual("New User", user.Login);
        }

        [TestMethod]
        public void Authenticate_LoginOkMdpKo_AuthentificationKO()
        {
            dal.AddUser("New User", "12345");
            User user = dal.Authenticate("New User", "0");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void Authenticate_LoginKoMdpOk_AuthentificationKO()
        {
            dal.AddUser("New User", "12345");
            User user = dal.Authenticate("New", "12345");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void Authenticate_LoginMdpKo_AuthentificationKO()
        {
            User user = dal.Authenticate("New User", "12345");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void HasVoted_AvecIdNonNumerique_RetourneFalse()
        {
            bool pasVote = dal.HasVoted(1, "abc");

            Assert.IsFalse(pasVote);
        }

        [TestMethod]
        public void HasVoted_UserNAPasVote_RetourneFalse()
        {
            int idSurvey = dal.CreateSurvey();
            int idUser = dal.AddUser("New User", "12345");

            bool pasVote = dal.HasVoted(idSurvey, idUser.ToString());

            Assert.IsFalse(pasVote);
        }

        [TestMethod]
        public void HasVoted_UserAVote_RetourneTrue()
        {
            int idSurvey = dal.CreateSurvey();
            int idUser = dal.AddUser("New User", "12345");
            dal.CreateRestaurant("La bonne fourchette", "0102030405");
            dal.AddVote(idSurvey, 1, idUser);

            bool aVote = dal.HasVoted(idSurvey, idUser.ToString());

            Assert.IsTrue(aVote);
        }

        [TestMethod]
        public void GetAllResults_AvecQuelquesChoix_RetourneBienLesResults()
        {
            int idSurvey = dal.CreateSurvey();
            int idUser1 = dal.AddUser("User1", "12345");
            int idUser2 = dal.AddUser("User2", "12345");
            int idUser3 = dal.AddUser("User3", "12345");

            dal.CreateRestaurant("Resto pinière", "0102030405");
            dal.CreateRestaurant("Resto pinambour", "0102030405");
            dal.CreateRestaurant("Resto mate", "0102030405");
            dal.CreateRestaurant("Resto ride", "0102030405");

            dal.AddVote(idSurvey, 1, idUser1);
            dal.AddVote(idSurvey, 3, idUser1);
            dal.AddVote(idSurvey, 4, idUser1);
            dal.AddVote(idSurvey, 1, idUser2);
            dal.AddVote(idSurvey, 1, idUser3);
            dal.AddVote(idSurvey, 3, idUser3);

            List<Results> results = dal.GetAllResults(idSurvey);

            Assert.AreEqual(3, results[0].NumberOfVotes);
            Assert.AreEqual("Resto pinière", results[0].Name);
            Assert.AreEqual("0102030405", results[0].Phone);
            Assert.AreEqual(2, results[1].NumberOfVotes);
            Assert.AreEqual("Resto mate", results[1].Name);
            Assert.AreEqual("0102030405", results[1].Phone);
            Assert.AreEqual(1, results[2].NumberOfVotes);
            Assert.AreEqual("Resto ride", results[2].Name);
            Assert.AreEqual("0102030405", results[2].Phone);
        }

        [TestMethod]
        public void GetAllResults_AvecDeuxSurveys_RetourneBienLesBonsResults()
        {
            int idSurvey1 = dal.CreateSurvey();
            int idUser1 = dal.AddUser("User1", "12345");
            int idUser2 = dal.AddUser("User2", "12345");
            int idUser3 = dal.AddUser("User3", "12345");
            dal.CreateRestaurant("Resto pinière", "0102030405");
            dal.CreateRestaurant("Resto pinambour", "0102030405");
            dal.CreateRestaurant("Resto mate", "0102030405");
            dal.CreateRestaurant("Resto ride", "0102030405");
            dal.AddVote(idSurvey1, 1, idUser1);
            dal.AddVote(idSurvey1, 3, idUser1);
            dal.AddVote(idSurvey1, 4, idUser1);
            dal.AddVote(idSurvey1, 1, idUser2);
            dal.AddVote(idSurvey1, 1, idUser3);
            dal.AddVote(idSurvey1, 3, idUser3);

            int idSurvey2 = dal.CreateSurvey();
            dal.AddVote(idSurvey2, 2, idUser1);
            dal.AddVote(idSurvey2, 3, idUser1);
            dal.AddVote(idSurvey2, 1, idUser2);
            dal.AddVote(idSurvey2, 4, idUser3);
            dal.AddVote(idSurvey2, 3, idUser3);

            List<Results> results1 = dal.GetAllResults(idSurvey1);
            List<Results> results2 = dal.GetAllResults(idSurvey2);

            Assert.AreEqual(3, results1[0].NumberOfVotes);
            Assert.AreEqual("Resto pinière", results1[0].Name);
            Assert.AreEqual("0102030405", results1[0].Phone);
            Assert.AreEqual(2, results1[1].NumberOfVotes);
            Assert.AreEqual("Resto mate", results1[1].Name);
            Assert.AreEqual("0102030405", results1[1].Phone);
            Assert.AreEqual(1, results1[2].NumberOfVotes);
            Assert.AreEqual("Resto ride", results1[2].Name);
            Assert.AreEqual("0102030405", results1[2].Phone);

            Assert.AreEqual(1, results2[0].NumberOfVotes);
            Assert.AreEqual("Resto pinambour", results2[0].Name);
            Assert.AreEqual("0102030405", results2[0].Phone);
            Assert.AreEqual(2, results2[1].NumberOfVotes);
            Assert.AreEqual("Resto mate", results2[1].Name);
            Assert.AreEqual("0102030405", results2[1].Phone);
            Assert.AreEqual(1, results2[2].NumberOfVotes);
            Assert.AreEqual("Resto pinière", results2[2].Name);
            Assert.AreEqual("0102030405", results2[2].Phone);
        }
    }
}
