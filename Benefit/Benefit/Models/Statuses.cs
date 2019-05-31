using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class Statuses
    {
        public int StatusCode { get; set; }
        public string Description { get; set; }
 

        public Statuses(int _statusCode, string _description)
        {
            StatusCode = _statusCode;
            Description = _description;
        }


    }
}