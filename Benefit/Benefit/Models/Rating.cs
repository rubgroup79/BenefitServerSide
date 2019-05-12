using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class Rating
    {
        public int RatingCode { get; set; }
        public int TraineeCode { get; set; }
        public int RatedCode { get; set; }
 

        public Rating(int _traineeCode, int _ratedCode)
        {
            TraineeCode = _traineeCode;
            RatedCode = _ratedCode;
        }


    }
}