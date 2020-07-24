using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomSearchWindowsForm
{
    class EventSearchUrlBuilder
    {
        public static string BuildSearchUrl(string keyword, string lati, string longi, string radius)
        {
            return "https://app.ticketmaster.com/discovery/v2/events.json?countryCode=US&size=50" + "&keyword=" + keyword + "&latlong=" + lati + "," + longi + "&radius=" + radius;
        }
    }
}
