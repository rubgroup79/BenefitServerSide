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



        public HistoryGroupTraining(string _trainingTime, float _latitude, float _longitude, int _withTrainer, int _statusCode, int _price, int _creatorCode, int _minParticipants, int _maxParticipants, string _sportCategory,  int _currentParticipants=0)
            :base(_trainingTime,  _latitude,  _longitude, _withTrainer, _statusCode, _price)
        {
            CreatorCode = _creatorCode;
            MinParticipants = _minParticipants;
            MaxParticipants = _maxParticipants;
            CurrentParticipants = _currentParticipants;
            SportCategory = _sportCategory;



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
        
        public void CancelGroupParticipant(int GroupTrainingCode, int UserCode)
        {
            DBservices dbs = new DBservices();
            dbs.CancelGroupParticipant(GroupTrainingCode,UserCode);
        }
        
        public List <HistoryGroupTraining> GetPastGroupTrainings(int UserCode)
        {
            DBservices dbs = new DBservices();
            return dbs.GetPastGroupTrainings(UserCode);
        }
    }
}