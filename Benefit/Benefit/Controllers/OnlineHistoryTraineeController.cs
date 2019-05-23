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

    public class OnlineHistoryTraineeController : ApiController
    {
        public OnlineHistoryTraineeController()
        {

        }

        [HttpPost]
        [Route("api/InsertOnlineTrainee")]
        public void InsertOnlineTrainee([FromBody]OnlineHistoryTrainee o)
        {
           o.InsertOnlineTrainee(o);
        }


        [HttpPost]
        [Route("api/SearchCoupleTraining")]
        public IEnumerable<Result> SearchCoupleTraining([FromBody]OnlineHistoryTrainee o)
        {
            return o.SearchCoupleTraining(o);
        }


        [HttpPost]
        [Route("api/SearchGroups")]
        public IEnumerable<HistoryGroupTraining> SearchGroups([FromBody]OnlineHistoryTrainee o)
        {
            return o.SearchGroups(o);
        }
        
    }
}
