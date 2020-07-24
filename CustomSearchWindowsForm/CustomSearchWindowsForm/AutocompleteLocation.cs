using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Json.Net;
using Newtonsoft.Json.Linq;

namespace CustomSearchWindowsForm
{
    class AutocompleteLocation
    {
        private string urlBase = "https://maps.googleapis.com/maps/api/place/autocomplete/json?";

        public string[] GetAutocomplete(string enteredText)
        {
            string formatedText = enteredText.Replace(" ", "+");
            string url = urlBase + "input=" + formatedText;

            var response = HTTPrequests.GetHTTPRequestWithGoogle(url);

            //JObject json = JObject.Parse(response.ToString());
            //Console.WriteLine(json);
            AutocompleteResults results = Newtonsoft.Json.JsonConvert.DeserializeObject<AutocompleteResults>(response);

            //Console.WriteLine(response.ToString());

            int numResults = results.predictions.Count();
            string[] returnResults = new string[numResults];
            for(int i = 0; i < numResults; i++)
            {
                returnResults[i] = results.predictions[i].description;
            }

            return returnResults;
        }
    }
}
