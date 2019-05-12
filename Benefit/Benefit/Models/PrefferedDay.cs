using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class PrefferedDay
    {
            public int UserCode { get; set; }
            public string Token { get; set; }
            public string DayName { get; set; }
            public int NumOfTrainings { get; set; }



        public PrefferedDay(int _userCode, string _token, string _dayName, int _numOfTrainings)
        {
            UserCode = _userCode;
            Token = _token;
            DayName = _dayName;
            NumOfTrainings = _numOfTrainings;
        }

        public PrefferedDay()
        {

        }

    }
}