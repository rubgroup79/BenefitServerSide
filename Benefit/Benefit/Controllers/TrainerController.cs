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

    public class TrainerController : ApiController
    {
        public TrainerController()
        {

        }

        [HttpPost]
        [Route("api/InsertTrainer")]
        public int SignInTrainer([FromBody]Trainer t)
        {
            return t.SignInTrainer();
        }
      


    }
}
