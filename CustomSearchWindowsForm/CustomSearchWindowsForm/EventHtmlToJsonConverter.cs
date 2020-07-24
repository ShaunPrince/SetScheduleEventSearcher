using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using JsonTest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace CustomSearchWindowsForm
{
    class EventHtmlToJsonConverter
    {
        public static AllEventsTopItem ConvertRawHtmlToJson(string rawText)
        {
            AllEventsTopItem allEvents = JsonConvert.DeserializeObject<AllEventsTopItem>(rawText);
            return allEvents;
        }
    }
}
