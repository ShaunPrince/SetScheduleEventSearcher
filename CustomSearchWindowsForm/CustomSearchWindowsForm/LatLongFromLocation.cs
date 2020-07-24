using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomSearchWindowsForm
{
    class LatLongFromLocation
    {
        private string urlBase = "https://maps.googleapis.com/maps/api/geocode/json?";

        public Tuple<float, float> GetLatLong(string address)
        {
            string formatedText = address.Replace(" ", "+");
            formatedText = formatedText.Replace(",", "");   // get rid of commas
            string url = urlBase + "address=" + formatedText;

            var response = HTTPrequests.GetHTTPRequestWithGoogle(url);

            if (response == null)
            {
                return new Tuple<float, float>(-404, -404);
            }
            else
            {
                LatLongResults results = Newtonsoft.Json.JsonConvert.DeserializeObject<LatLongResults>(response);
                return new Tuple<float, float>(results.results[0].geometry.location.lat, results.results[0].geometry.location.lng);
            }
            
        }
    }
}
