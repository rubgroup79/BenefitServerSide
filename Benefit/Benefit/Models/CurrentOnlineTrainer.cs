using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class CurrentOnlineTrainer: OnlineHistoryTrainer
    {
        public CurrentOnlineTrainer(int _userCode, float _latitude, float _longitude, string _startTime, string _endTime)
            :base( _userCode ,  _latitude,  _longitude,  _startTime,  _endTime)
        {

        }


    }
}