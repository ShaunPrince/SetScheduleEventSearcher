using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomSearchWindowsForm
{
    //JSON parsing class
    class LatLongResults
    {
        public List<LatLongResultItem> results { get; set; }
        public string status { get; set; }
    }
}
