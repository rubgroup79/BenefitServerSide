using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Benefit.Models
{
    public class SportCategory
    {
        public int CategoryCode { get; set; }
        public string Description { get; set; }

        public SportCategory()
        {

        }

        public SportCategory(int _categoryCode, string _description)
        {
            CategoryCode = _categoryCode;
            Description = _description;
        }


    }
}