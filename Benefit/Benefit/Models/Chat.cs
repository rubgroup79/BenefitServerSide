using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class Chat
    {
        public int ChatCode { get; set; }
        public int UserCode1 { get; set; }
        public int UserCode2 { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LastMessage { get; set; }
        public string Picture { get; set; }

        public Chat(int _userCode1, int _userCode2, string _firstName, string _lastName, string _lastMessage, string _Picture)
        {
            UserCode1 = _userCode1;
            UserCode2 = _userCode2;
            FirstName = _firstName;
            LastName = _lastName;
            LastMessage = _lastMessage;
            Picture = _Picture;

        }

        public Chat()
        {

        }


        public int OpenChat(int UserCode1, int UserCode2)
        {
            DBservices dbs = new DBservices();
            return dbs.OpenChat(UserCode1, UserCode2);
        }

        public List<Chat> GetAllChats(int UserCode)
        {
            DBservices dbs = new DBservices();
            return dbs.GetAllChats(UserCode);
        }

        public List<Message> GetMessages(int ChatCode)
        {
            DBservices dbs = new DBservices();
            return dbs.GetMessages(ChatCode);
        }

        public void SendMessage(Message m)
        {
            DBservices dbs = new DBservices();
             dbs.SendMessage(m);
        }
        
            public int GetSuggestionCode(int UserCode1, int UserCode2)
        {
            DBservices dbs = new DBservices();
            return dbs.GetSuggestionCode(UserCode1, UserCode2);
        }


    }
}
