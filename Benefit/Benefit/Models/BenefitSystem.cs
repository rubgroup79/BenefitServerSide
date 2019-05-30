using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class BenefitSystem
    {
        
        public BenefitSystem()
        {
          
        }

        public bool CheckIfEmailExists(string UserEmail)
        {
            DBservices dbs = new DBservices();
            return dbs.CheckIfEmailExists(UserEmail);
        }

        public Trainee CheckIfPasswordMatches(string UserEmail, string Password)
        {
            DBservices dbs = new DBservices();
            return dbs.CheckIfPasswordMatches(UserEmail, Password);
        }

        // this function returns all trainees that didnt participate in a training for over a week//
        public List<Trainee> GetLazyTrainees()
        {
            DBservices dbs = new DBservices();
            return dbs.GetLazyTrainees();
        }

        public List<Trainer> GetLazyTrainers()
        {
            DBservices dbs = new DBservices();
            return dbs.GetLazyTrainers();
        }


        public List<PrefferedDay> GetPrefferedTrainingDay()
        {
            DBservices dbs = new DBservices();
            return dbs.GetPrefferedTrainingDay();
        }



        public void UpdateToken(string Token, int UserCode)
        {
            DBservices dbs = new DBservices();
            dbs.UpdateToken(Token, UserCode);
        }

        //public List<User> SearchPartners(CurrentOnlineTrainee o)
        //{
        //    DBservices dbs = new DBservices();
        //    return dbs.SearchPartners(o);
        //}

        public User ShowProfile(int UserCode)
        {
            DBservices dbs = new DBservices();
            return dbs.ShowProfile(UserCode);
        }

        public void GoOffline(int UserCode, int IsTrainer)
        {
            DBservices dbs = new DBservices();
            dbs.GoOffline(UserCode, IsTrainer);
        }
        public bool CheckIfUserOnline(int UserCode, int IsTrainer)
        {
            DBservices dbs = new DBservices();
            return dbs.CheckIfUserOnline(UserCode, IsTrainer);
        }

        public void UpdateTrainingsStatus()
        {

            DBservices dbs = new DBservices();
            dbs.UpdateTrainingsStatus();
        }

        public int InsertNewRating(Rating r)
        {
            DBservices dbs = new DBservices();
            return dbs.InsertNewRating(r);
        }

        public void InsertParametersRate(ParameterRate r)
        {
            DBservices dbs = new DBservices();
            dbs.InsertParametersRate(r);
        }

        public Rating CheckIfRateExists(int UserCode, int RatedUserCode)
        {
            DBservices dbs = new DBservices();
            return dbs.CheckIfRateExists(UserCode, RatedUserCode);
        }

        public void UpdateExistingParametersRate(ParameterRate pr)
        {
            DBservices dbs = new DBservices();
            dbs.UpdateExistingParametersRate(pr);
        }

        //public void UpdateUserRate(float NewRate, int RatedCode)
        //{
        //    DBservices dbs = new DBservices();
        //    dbs.UpdateUserRate(NewRate, RatedCode);
        //}

        public void UpdateExistingAvarageRate(Rating r)
        {
            DBservices dbs = new DBservices();
            dbs.UpdateExistingAvarageRate(r);
        }

    }
}