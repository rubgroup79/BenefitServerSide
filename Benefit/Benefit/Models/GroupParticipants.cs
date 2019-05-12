using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class GroupParticipants
    {
        public int UserCode { get; set; }
        public int GroupTrainingCode { get; set; }
        public int StatusCode { get; set; }
      

        public GroupParticipants(int _userCode, int _groupTrainingCode, int _statusCode=1)
        {
            UserCode = _userCode;
            GroupTrainingCode = _groupTrainingCode;
            StatusCode = _statusCode;
        }
    }
}