using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Benefit.Models;
using Benefit.Controllers;

namespace Benefit.Controllers
{

    public class ChatController : ApiController
    {
        public ChatController()
        {

        }
        
        [HttpGet]
        [Route("api/GetAllChats")]
        public IEnumerable<Chat> GetAllChats(int UserCode)
        {
            Chat c = new Chat();
            return c.GetAllChats(UserCode);
        }

        [HttpGet]
        [Route("api/GetMessages")]
        public IEnumerable<Message> GetMessages(int ChatCode)
        {
            Chat c = new Chat();
            return c.GetMessages(ChatCode);
        }
        

        [HttpPost]
        [Route("api/SendMessage")]
        public void SendMessage(Message m)
        {
            Chat c = new Chat();
            c.SendMessage(m);
        }

       
             [HttpGet]
        [Route("api/GetSuggestionCode")]
        public int GetSuggestionCode(int UserCode1, int UserCode2)
        {
            Chat c = new Chat();
            return c.GetSuggestionCode(UserCode1, UserCode2);
        }
    }



}

