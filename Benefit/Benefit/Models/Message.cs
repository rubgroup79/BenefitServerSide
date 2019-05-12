using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class Message
    {
        public int MessageCode { get; set; }
        public int ChatCode { get; set; }
        public int SenderCode { get; set; }
        public DateTime SendingTime { get; set; }
        public string Content { get; set; }

        public Message(int _chatCode , int _senderCode, DateTime _sendingTime, string _content)
        {
            ChatCode = _chatCode;
            SenderCode = _senderCode;
            SendingTime = _sendingTime;
            Content = _content;
            
        }


    }
}