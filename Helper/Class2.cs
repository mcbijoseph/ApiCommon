using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
namespace Helper
{
    /**
     * //USAGE SAMPLE
     * Helper.SynchronousRequest sr = new Helper.SynchronousRequest("http://124.105.198.3:94/api/Projects");
     * sr.HttpRequest();
     * 
     */
    public class SynchronousRequest
    {
        //
        public HttpClient httpClient;

        /// <summary>
        ///
        /// </summary>
        /// <param name="Url">HTTP LOCATION</param>
        /// <param name="controller">Controller</param>
        /// <param name="format">SAMPLE: "application/json"</param>
        public SynchronousRequest(string Url)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(Url);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            //ApiClient.
        }
        public string HttpRequest()
        {
            return HttpRequest("");
        }


        public string HttpRequest(string url, string Method = "Get", HttpContent content = null)
        {
            //ApiHelper ap = new ApiHelper();
            HttpResponseMessage response = null;



            if (Method == "POST")
                response = httpClient.PostAsync(url, content).Result;
            else if (Method == "PUT")
                response = httpClient.PutAsync(url, content).Result;
            else
                response = httpClient.GetAsync(url).Result;

            string responseString = "[]";
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;
                // by calling .Result you are synchronously reading the result
                responseString = responseContent.ReadAsStringAsync().Result;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var responseContent = response.Content;
                responseString = responseContent.ReadAsStringAsync().Result;
            }
            return responseString;
        }
        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}