using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class Trainee: User
    {
        public int MinBudget { get; set; }
        public int MaxBudget { get; set; }
        public string PartnerGender { get; set; }
        public string TrainerGender { get; set; }
        public int MinPartnerAge { get; set; }
        public int MaxPartnerAge { get; set; } 

        public Trainee(string _email, string _firstName, string _lastName, string _password, string _gender, string _dateOfBirth, string _picture,  int _searchRadius, int _isTrainer, int[] _sportCategories,  int _minBudget, int _maxBudget, string _partnerGender , string _trainerGender , int _minPartnerAge, int _maxPartnerAge,string _token, float _rate=0 )
            : base(_email, _firstName, _lastName, _password, _gender, _dateOfBirth, _picture, _searchRadius, _isTrainer, _sportCategories, _token, _rate)
        {
            MinBudget = _minBudget;
            MaxBudget = _maxBudget;
            PartnerGender = _partnerGender;
            TrainerGender = _trainerGender;
            MinPartnerAge = _minPartnerAge;
            MaxPartnerAge = _maxPartnerAge;
        }
        public Trainee()
        {

        }

        public int SignInTrainee()
        {
            DBservices dbs = new DBservices();
            int UserCode = dbs.SignInTrainee(this);
            return UserCode;
        }

        public void JoinGroup(int UserCode, int GroupTrainingCode)
        {
            DBservices dbs = new DBservices();
            dbs.JoinGroup( UserCode, GroupTrainingCode,0);
        }

    }
}