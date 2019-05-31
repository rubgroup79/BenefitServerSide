using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class TraineeDetails
    {
        public int TraineeCode { get; set; }
        public string Description { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public string Picture { get; set; }
        public float Rate { get; set; }
        public int SearchRadius { get; set; }
        public int MaxBudget { get; set; }
        public string PartnerGender { get; set; }
        public string TrainerGender { get; set; }
        public int MinPartnerAge { get; set; }
        public int MaxPartnerAge { get; set; }
        public List<SportCategory> SportCategories { get; set; }



        public TraineeDetails()
        {

        }

        public TraineeDetails(int _traineeCode, string _description, string _firstName, string _lastName, string _email, string _password, int _age, string _picture, float _rate, int _searchRadius, int _maxBudget, string _partnerGender, string _trainerGender, int _minPartnerAge, int _maxPartnerAge, List<SportCategory> _sportCategories)
        {
            TraineeCode = _traineeCode;
            Description = _description;
            FirstName = _firstName;
            LastName = _lastName;
            Email = _email;
            Password = _password;
            Age = _age;
            Picture = _picture;
            Rate = _rate;
            SearchRadius = _searchRadius;
            MaxBudget = _maxBudget;
            TrainerGender = _trainerGender;
            MinPartnerAge = _minPartnerAge;
            MaxPartnerAge = _maxPartnerAge;
            SportCategories = _sportCategories;

        }


    }
}
