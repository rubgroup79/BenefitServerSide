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

    public class TraineeController : ApiController
    {
        public TraineeController()
        {

        }

        [HttpPost]
        [Route("api/InsertTrainee")]
        public int  SignIn([FromBody]Trainee t)
        {
           return t.SignInTrainee();
        }
        

        [HttpPost]
        [Route("api/JoinGroup")]
        public void JoinGroup(int UserCode, int GroupTrainingCode)
        {
            Trainee t = new Trainee();
            t.JoinGroup(UserCode, GroupTrainingCode);
        }

		


	}
}
