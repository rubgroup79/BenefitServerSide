using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class OnlineHistoryTrainee: OnlineHistory
    {
        public int WithTrainer { get; set; }
        public int WithPartner { get; set; }
        public int GroupWithTrainer { get; set; }
        public int GroupWithPartners { get; set; }

        public OnlineHistoryTrainee( int _userCode, float _latitude, float _longitude, string _startTime, string _endTime, int _withTrainer, int _withPartner, int _groupWithTrainer , int _groupWithPartners)
            : base( _userCode, _latitude,  _longitude,  _startTime,  _endTime)
        {
            WithTrainer = _withTrainer;
            WithPartner = _withPartner;
            GroupWithTrainer = _groupWithTrainer;
            GroupWithPartners = _groupWithPartners;
        }

        public List<Result> InsertOnlineTrainee(OnlineHistoryTrainee o)
        {
           DBservices dbs = new DBservices();
            return dbs.InsertOnlineTrainee(o);
        }

        public List<HistoryGroupTraining> SearchGroups(OnlineHistoryTrainee o)
        {
            DBservices dbs = new DBservices();
            return dbs.SearchGroups(o);
        }

    }
}