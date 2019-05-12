using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Benefit.Models
{
    public class PushNotification
    {
        public string To { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Badge { get; set; }

        public PushNotification(string _to, string _title, string _body, int _badge)
        {
            To = _to;
            Title = _title;
            Body = _body;
            Badge = _badge;
        }

        public PushNotification()
        {

        }


    }



}
