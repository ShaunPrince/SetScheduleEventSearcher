using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace CustomSearchWindowsForm
{
    class HTTPrequests
    {
        private static readonly HttpClient client = new HttpClient();
        private static string googleApikey = "AIzaSyDtNGLim_7x3NXiQxzHdTWPoKFwIFyYL68";
        private static string ticketMasterApiKey = "KRJXKVPALETfipN1T7GeMGbMO4pC7Pr8";

        public static string GetHTTPRequestWithGoogle(string url)
        {
            try
            {
                url += "&key=" + googleApikey;
                Uri uri = new Uri(url);
                var responseString = client.GetStringAsync(uri).Result;
                return responseString;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex + " for url: " + url);
                return null;
            }
        }

        public static string GetHTTPRequestWithTicketMaster(string url)
        {
            try
            {
                url += "&apikey=" + ticketMasterApiKey;
                Uri uri = new Uri(url);
                var responseString = client.GetStringAsync(uri).Result;
                return responseString;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex + " for url: " + url);
                return null;
            }
        }
    }
}
