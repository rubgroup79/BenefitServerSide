using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class RateParameter
    {
        public int ParameterCode { get; set; }
        public string Description { get; set; }
 

        public RateParameter(int _parameterCode, string _description)
        {
            ParameterCode = _parameterCode;
            Description = _description;
        }


    }
}