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
    }
}