using System;
using System.Collections.Generic;
using System.Text;

namespace JsonTest
{
    //JSON parsing class
    class EventItem
    {
        public string name { get; set; }
        public string url { get; set; }
        public Dates dates { get; set; }
        public string info { get; set; }
        public EmbeddedInfo _embedded { get; set; }
    }
}
