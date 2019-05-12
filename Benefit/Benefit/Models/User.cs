using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class User
    {
        public int UserCode { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string Picture { get; set; }
        public float Rate { get; set; }
        public int SearchRadius { get; set; }
        public int IsTrainer { get; set; }
        public int[] SportCategories { get; set; }
        public string Token { get; set; }

        public User(string _email, string _firstName, string _lastName, string _password, string _gender, string _dateOfBirth, string _picture, int _searchRadius, int _isTrainer, int[] _sportCategories ,string _token, float _rate = 0)
        {
            Email = _email;
            FirstName = _firstName;
            LastName = _lastName;
            Password = _password;
            Gender = _gender;
            DateOfBirth = _dateOfBirth;
            Picture = _picture;
            Rate = _rate;
            SearchRadius = _searchRadius;
            IsTrainer = _isTrainer;
            SportCategories = _sportCategories;
            Token = _token;
        }

        public User()
        {
            
        }

        
		public string GetToken(int UserCode)
		{
			DBservices dbs = new DBservices();
			return dbs.GetToken(UserCode);
		}



	}
}