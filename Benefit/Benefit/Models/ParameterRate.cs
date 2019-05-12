using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class ParameterRate
    {
        public int RatingCode { get; set; }
        public int ParameterCode { get; set; }
        public float Rate { get; set; }
 

        public ParameterRate(int _ratingCode, int _parameterCode, float _rate)
        {
            RatingCode = _ratingCode;
            ParameterCode = _parameterCode;
            Rate = _rate;
        }


    }
}