using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class HistoryGroupTraining : Training
    {
        public int CreatorCode { get; set; }
        public int MinParticipants { get; set; }
        public int MaxParticipants { get; set; }
        public int CurrentParticipants { get; set; }
        public string SportCategory { get; set; }
        public int SportCategoryCode { get; set; }
        public int Price { get; set; }


        public HistoryGroupTraining(string _trainingTime, float _latitude, float _longitude, int _withTrainer, int _statusCode, int _creatorCode, int _minParticipants, int _maxParticipants, int _price, string _sportCategory,  int _currentParticipants=0)
            :base(_trainingTime,  _latitude,  _longitude, _withTrainer, _statusCode)
        {
            CreatorCode = _creatorCode;
            MinParticipants = _minParticipants;
            MaxParticipants = _maxParticipants;
            if (_withTrainer == 0) CurrentParticipants = 1;
            else CurrentParticipants = 0;
            SportCategory = _sportCategory;
            Price = _price;



        }

        public HistoryGroupTraining()
        {

        }

        public void InsertGroupTraining(HistoryGroupTraining h)
        {
            DBservices dbs = new DBservices();
            dbs.InsertGroupTraining(this);
        }

        public List<HistoryGroupTraining> GetFutureGroupTrainings(int UserCode)
        {
            DBservices dbs = new DBservices();
            return dbs.GetFutureGroupTrainings(UserCode);
        }




    }
}