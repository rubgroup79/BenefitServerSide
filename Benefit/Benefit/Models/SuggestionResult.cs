using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class SuggestionResult
    {
        public int SuggestionCode { get; set; }
		public int SenderCode { get; set; }
		public int ReceiverCode { get; set; }
		public int StatusCode { get; set; }
		public string SendingTime { get; set; }
		public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
		public string Picture { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
		public bool IsTrainer { get; set; }

		public SuggestionResult( int _suggestionCode, int _senderCode, int _receiverCode, int _statusCode, string _sendingTime, string _firstName, string _lastName, string _gender, int _age, string _picture, float _latitude, float _longitude, bool _isTrainer)
        {
			SuggestionCode = _suggestionCode;
			SenderCode = _senderCode;
			ReceiverCode = _receiverCode;
			StatusCode = _statusCode;
			SendingTime = _sendingTime;
			FirstName = _firstName;
			LastName = _lastName;
			Gender = _gender;
			Age = _age;
			Picture = _picture;
			Latitude = _latitude;
			Longitude = _longitude;
			IsTrainer = _isTrainer;
	}

        public SuggestionResult()
        {

        }
    }
}