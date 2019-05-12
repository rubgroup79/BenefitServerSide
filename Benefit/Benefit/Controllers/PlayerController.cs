using Newtonsoft.Json;
//using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Benefit.Models;
using System.Net;
//using OpenQA.Selenium;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;


namespace Benefit.Controllers
{
    public class PlayerController : ApiController
    {


        [HttpGet]
        [Route("api/start")]
        public void StartTimer()
        {
            WebApiApplication.StartTimer();
        }
        [HttpGet]
        [Route("api/stop")]
        public void StopTimer()
        {
            WebApiApplication.EndTimer();
        }

      
    }
}