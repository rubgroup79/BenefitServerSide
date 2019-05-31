using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class AverageRateParameters
    {
        public RateParameter Parameter { get; set; }
        public float AverageRate { get; set; }

        public AverageRateParameters()
        {

        }
        public AverageRateParameters(RateParameter _parameter, float _averageRate)
        {
            Parameter = _parameter;
            AverageRate = _averageRate;
        }


    }
}