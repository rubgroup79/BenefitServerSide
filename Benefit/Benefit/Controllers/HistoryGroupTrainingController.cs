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

    public class HistoryGroupTrainingController : ApiController
    {
        public HistoryGroupTrainingController()
        {

        }

        [HttpPost]
        [Route("api/InsertGroupTraining")]
        public void InsertGroupTraining([FromBody]HistoryGroupTraining h)
        {
            h.InsertGroupTraining(h);
        }

        [HttpGet]
        [Route("api/GetFutureGroupTrainings")]
        public IEnumerable<HistoryGroupTraining> GetFutureGroupTrainings(int UserCode)
        {
            HistoryGroupTraining hgt = new HistoryGroupTraining();
            return hgt.GetFutureGroupTrainings(UserCode);
        }
        
        [HttpPost]
        [Route("api/CancelGroupParticipant")]
        public void CancelGroupParticipant(int GroupTrainingCode, int UserCode)
        {
            HistoryGroupTraining hgt = new HistoryGroupTraining();
            hgt.CancelGroupParticipant(GroupTrainingCode, UserCode);
        }

        
             [HttpGet]
        [Route("api/GetPastGroupTrainings")]
        public IEnumerable<HistoryGroupTraining> GetPastGroupTrainings(int UserCode)
        {
            HistoryGroupTraining hgt = new HistoryGroupTraining();
            return hgt.GetPastGroupTrainings(UserCode);
        }

        [HttpGet]
        [Route("api/GetGroupDetails")]
        public HistoryGroupTraining GetGroupDetails(int GroupCode)
        {
            HistoryGroupTraining hgt = new HistoryGroupTraining();
            return hgt.GetGroupDetails(GroupCode);
        }

        [HttpGet]
        [Route("api/GetGroupParticipants")]
        public IEnumerable<User> GetGroupParticipants(int GroupCode)
        {
            HistoryGroupTraining hgt = new HistoryGroupTraining();
            return hgt.GetGroupParticipants(GroupCode);
        }

        [HttpGet]
        [Route("api/CancelGroupTraining")]
        public List<User> CancelGroupTraining(int GroupCode)
        {
            HistoryGroupTraining hgt = new HistoryGroupTraining();
            return hgt.CancelGroupTraining(GroupCode);
        }


    }
}
