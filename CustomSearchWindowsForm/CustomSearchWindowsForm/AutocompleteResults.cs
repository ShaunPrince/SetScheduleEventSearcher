using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomSearchWindowsForm
{
    //JSON parsing class
    class AutocompleteResults
    {
        public List<Predictions> predictions { get; set; }

        public string status { get; set; }
    }
}
