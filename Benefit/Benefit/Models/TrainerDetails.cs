using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class TrainerDetails
    {
        public int TrainerCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public string Picture { get; set; }
        public float Rate { get; set; }
        public int SearchRadius { get; set; }
        public int PersonalTrainingPrice { get; set; }
        public List<SportCategory> SportCategories { get; set; }



        public TrainerDetails()
        {

        }

        public TrainerDetails(int _traineeCode, string _firstName, string _lastName, string _email, string _password, int _age, string _picture, float _rate, int _searchRadius, int _personalTrainingPrice, List<SportCategory> _sportCategories)
        {
            TrainerCode = _traineeCode;
            FirstName = _firstName;
            LastName = _lastName;
            Email = _email;
            Password = _password;
            Age = _age;
            Picture = _picture;
            Rate = _rate;
            SearchRadius = _searchRadius;
            PersonalTrainingPrice = _personalTrainingPrice;
            SportCategories = _sportCategories;

        }


    }
}
