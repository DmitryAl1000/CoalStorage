﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class CargoHistory
    {
        public DateTime DateTime { get; set; }
        public string AreaId { get; set; } = string.Empty;
        public double CargoOnArea { get; set; } 


        //public Area? Area { get; set; }
    }
}