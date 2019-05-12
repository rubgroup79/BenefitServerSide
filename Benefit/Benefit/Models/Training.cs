using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public abstract class Training
    {
        public int TrainingCode { get; set; }
        public string TrainingTime { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int WithTrainer { get; set; }
        public int StatusCode { get; set; }

        public Training(string _trainingTime, float _latitude, float _longitude, int _withTrainer, int _statusCode)
        {
            TrainingTime = _trainingTime;
            Latitude = _latitude;
            Longitude = _longitude;
            WithTrainer = _withTrainer;
            StatusCode = _statusCode;
        }

        public Training()
        {

        }


    }
}