using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Benefit.Models;

namespace Benefit.Controllers
{

    public class CoupleTrainingController : ApiController
    {
        public CoupleTrainingController()
        {

        }
        [HttpGet]
        [Route("api/SendSuggestion")]
        public string SendSuggestion(int SenderCode, int ReceiverCode)
        {
            CoupleTrainingSuggestion s = new CoupleTrainingSuggestion();
            return s.SendSuggestion(SenderCode, ReceiverCode);
        }

        [HttpPost]
        [Route("api/ReplySuggestion")]
        public void ReplySuggestion(int SuggestionCode, bool reply)
        {
            CoupleTrainingSuggestion s = new CoupleTrainingSuggestion();
            s.ReplySuggestion(SuggestionCode, reply);
        }

        [HttpGet]
        [Route("api/GetSuggestions")]
        public IEnumerable<SuggestionResult> GetSuggestions(int UserCode,  bool IsApproved)
        {
            CoupleTrainingSuggestion s = new CoupleTrainingSuggestion();
            return s.GetSuggestions(UserCode,  IsApproved);
        }


        [HttpGet]
        [Route("api/CheckActiveSuggestions")]
        public string  CheckActiveSuggestions(int SenderCode, int ReceiverCode)
        {
            CoupleTrainingSuggestion s = new CoupleTrainingSuggestion();
            return s.CheckActiveSuggestions(SenderCode,ReceiverCode);
        }

        [HttpPost]
        [Route("api/CancelSuggestion")]
        public void CancelSuggestion(  int SuggestionCode)
        {
            CoupleTrainingSuggestion s = new CoupleTrainingSuggestion();
            s.CancelSuggestion(SuggestionCode);
        }

        [HttpGet]
        [Route("api/GetFutureCoupleTrainings")]
        public IEnumerable<CoupleTraining> GetFutureCoupleTrainings(int UserCode)
        {
            CoupleTraining c = new CoupleTraining();
            return c.GetFutureCoupleTrainings(UserCode);
        }

        [HttpPost]
        [Route("api/CancelCoupleTraining")]
        public int CancelCoupleTraining(int CoupleTrainingCode, int UserCode)
        {
            CoupleTraining c = new CoupleTraining();
            return c.CancelCoupleTraining(CoupleTrainingCode, UserCode);
        }

        [HttpGet]
        [Route("api/GetPastCoupleTrainings")]
        public IEnumerable<CoupleTraining> GetPastCoupleTrainings(int UserCode)
        {
            CoupleTraining ct = new CoupleTraining();
            return ct.GetPastCoupleTrainings(UserCode);
        }


    }
}
