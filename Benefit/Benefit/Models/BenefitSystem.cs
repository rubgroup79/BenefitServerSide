﻿using System;
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

        public void UpdateTrainingsStatus()
        {

            DBservices dbs = new DBservices();
            dbs.UpdateTrainingsStatus();
        }

        //---------------------------------------------------------------------------------
        // Login
        //---------------------------------------------------------------------------------

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

        public void UpdateToken(string Token, int UserCode)
        {
            DBservices dbs = new DBservices();
            dbs.UpdateToken(Token, UserCode);
        }
        //---------------------------------------------------------------------------------
        // User Learning
        //---------------------------------------------------------------------------------

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
        
         //---------------------------------------------------------------------------------
        // Profile
        //---------------------------------------------------------------------------------
        
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

        public List<SportCategory> GetSportCategories()
        {
            DBservices dbs = new DBservices();
            return dbs.GetSportCategories();
        }

        public TraineeDetails GetTraineeProfileDetails(int UserCode)
        {
            DBservices dbs = new DBservices();
            return dbs.GetTraineeProfileDetails(UserCode);
        }

        public TrainerDetails GetTrainerProfileDetails(int UserCode)
        {
            DBservices dbs = new DBservices();
            return dbs.GetTrainerProfileDetails(UserCode);
        }

        public void UpdateTraineeDetails(TraineeDetails td)
        {
            DBservices dbs = new DBservices();
            dbs.UpdateTraineeDetails(td);
        }

        public void UpdateTrainerDetails(TrainerDetails td)
        {
            DBservices dbs = new DBservices();
            dbs.UpdateTrainerDetails(td);
        }

        //---------------------------------------------------------------------------------
        // Home - Online\Offline
        //---------------------------------------------------------------------------------

        public void GoOffline(int UserCode, int IsTrainer)
        {
            DBservices dbs = new DBservices();
            dbs.GoOffline(UserCode, IsTrainer);
        }

        public OnlineHistoryTrainee CheckIfTraineeOnline(int UserCode)
        {
            DBservices dbs = new DBservices();
            return dbs.CheckIfTraineeOnline(UserCode);
        }

        public OnlineHistoryTrainer CheckIfTrainerOnline(int UserCode)
        {
            DBservices dbs = new DBservices();
            return dbs.CheckIfTrainerOnline(UserCode);
        }

        //---------------------------------------------------------------------------------
        // Rates
        //---------------------------------------------------------------------------------
        
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

        public List<AverageRateParameters> GetAvarageParametersRate(int UserCode)
        {
            DBservices dbs = new DBservices();
            return dbs.GetAvarageParametersRate(UserCode);
        }

        //---------------------------------------------------------------------------------
        // Trainings
        //---------------------------------------------------------------------------------
       
        public bool checkForCloseTrainings(int UserCode, string TrainingTime)
        {
            DBservices dbs = new DBservices();
            return dbs.checkForCloseTrainings(UserCode, TrainingTime);
        }

        //---------------------------------------------------------------------------------
        // Chats
        //---------------------------------------------------------------------------------

        public int OpenChat(int UserCode1, int UserCode2)
        {
            DBservices dbs = new DBservices();
            return dbs.OpenChat(UserCode1, UserCode2);
        }

        public List<RateParameter> GetRateParameters()
        {
            DBservices dbs = new DBservices();
            return dbs.GetRateParameters();
        }

    }
}