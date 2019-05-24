using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{


    public class CoupleTraining : Training
    {
        public int PartnerUserCode { get; set; }
        public string PartnerFirstName { get; set; }
        public string PartnerLastName { get; set; }
        public int PartnerAge { get; set; }
        public string PartnerPicture { get; set; }
       public int SuggestionCode { get; set; }


        public CoupleTraining(string _trainingTime, float _latitude, float _longitude, int _withTrainer, int _statusCode, int _price
            , int _partnerUserCode, string _partnerFirstName, string _partnerLastName, int _partnerAge, string _partnerPicture, int _suggestionCode)
            :base(_trainingTime,  _latitude,  _longitude, _withTrainer, _statusCode, _price)
        {
            PartnerUserCode = _partnerUserCode;
            PartnerFirstName = _partnerFirstName;
            PartnerLastName = _partnerLastName;
            PartnerAge = _partnerAge;
            PartnerPicture = _partnerPicture;
            SuggestionCode = _suggestionCode;
    }

        public CoupleTraining() { }
        public List<CoupleTraining> GetFutureCoupleTrainings(int UserCode)
        {
            DBservices dbs = new DBservices();
            return dbs.GetFutureCoupleTrainings(UserCode);
        }

        public int CancelCoupleTraining(int CoupleTrainingCode, int UserCode)
        {
            DBservices dbs = new DBservices();
            return dbs.CancelCoupleTraining(CoupleTrainingCode, UserCode);
        }
        
     public List<CoupleTraining> GetPastCoupleTrainings(int UserCode)
        {
            DBservices dbs = new DBservices();
            return dbs.GetPastCoupleTrainings(UserCode);
        }

        public void InsertCoupleTraining(CoupleTraining ct)
        {

            DBservices dbs = new DBservices();
           dbs.InsertCoupleTraining(this);
        }
    }
}