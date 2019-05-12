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


        public Chat(int _userCode1, int _userCode2)
        {
            UserCode1 = _userCode1;
            UserCode2 = _userCode2;
          
        }


    }
}