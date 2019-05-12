using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class CoupleTrainingSuggestion
    {
        public int SuggestionCode { get; set; }
        public int SenderCode { get; set; }
        public int ReceiverCode { get; set; }
        public int StatusCode { get; set; }
        public DateTime SendingTime { get; set; }

        public CoupleTrainingSuggestion(int _senderCode, int _receiverCode, int _statusCode, DateTime _sendingTime)
        {
            SenderCode = _senderCode;
            ReceiverCode = _receiverCode;
            StatusCode = _statusCode;
            SendingTime = _sendingTime;
        }

		public CoupleTrainingSuggestion()
		{

		}

		public string SendSuggestion(int SenderCode, int ReceiverCode)
		{
			DBservices dbs = new DBservices();
			return dbs.SendSuggestion(SenderCode, ReceiverCode);
		}

        public void ReplySuggestion(int SuggestionCode, bool reply)
        {
            DBservices dbs = new DBservices();
            dbs.ReplySuggestion(SuggestionCode, reply);
        }

        public List<SuggestionResult> GetSuggestions(int UserCode, bool IsApproved)
        {
            DBservices dbs = new DBservices();
           return dbs.GetSuggestions(UserCode,  IsApproved);
        }

        public List<Result> GetSuggestionDetails(int SuggestionCode)
        {
            DBservices dbs = new DBservices();
            return dbs.GetSuggestionDetails(SuggestionCode);
        }



        public string CheckActiveSuggestions(int SenderCode,int ReceiverCode)
        {
            DBservices dbs = new DBservices();
            return dbs.CheckActiveSuggestions(SenderCode,ReceiverCode);
        }

        public void CancelSuggestion(int SuggestionCode)
        {
            DBservices dbs = new DBservices();
            dbs.CancelSuggestion(SuggestionCode);
        }


    }
}