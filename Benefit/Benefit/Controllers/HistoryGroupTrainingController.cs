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
    }
}
