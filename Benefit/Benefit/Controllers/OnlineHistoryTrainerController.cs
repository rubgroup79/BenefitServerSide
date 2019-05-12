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

    public class OnlineHistoryTrainerController : ApiController
    {
        public OnlineHistoryTrainerController()
        {

        }

        [HttpPost]
        [Route("api/InsertOnlineTrainer")]
        public void  InsertOnlineTrainer([FromBody]OnlineHistoryTrainer o)
        {
            o.InsertOnlineTrainer(o);
        }

        

    }
}
