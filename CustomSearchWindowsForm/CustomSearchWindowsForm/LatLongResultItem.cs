﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomSearchWindowsForm
{
    //JSON parsing class
    class LatLongResultItem
    {
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
    }
}
