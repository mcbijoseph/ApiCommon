using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;

namespace ApiHelper
{
   public interface IApiRequest
    {
        object HttpRequest(string Url, Controller controller, string format, string body);

    }

    public class ApiRequest
    {
        Controller _controller;
        private HttpContent httpContent { get; set; }
        private HttpClient httpClient;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Url">HTTP LOCATION</param>
        /// <param name="controller">Controller</param>
        /// <param name="format">SAMPLE: "application/json"</param>
        public ApiRequest(string Url, Controller controller, string format, string body)
        {
            _controller = controller;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(Url);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            //ApiClient.
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(format));            
            httpContent = new System.Net.Http.StringContent(body, System.Text.Encoding.UTF8, format);
        }
        public object HttpRequest(string Url, Controller controller, string format, string body)
        {
            httpClient.BaseAddress = new Uri(Url);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(format));
            httpContent = new StringContent(body, Encoding.UTF8, format);
            return HttpRequest("");
        }
        public object HttpRequest(string CName)
        {
            //ApiHelper ap = new ApiHelper();

            HttpResponseMessage response = null;
            if (_controller.Request.HttpMethod == "POST")
            {

                response = httpClient.PostAsync(CName, httpContent).Result;
            }
            else if (_controller.Request.HttpMethod == "PUT")
            {
                response = httpClient.PutAsync(CName, httpContent).Result;
            }
            else if (_controller.Request.HttpMethod == "GET")
            {
                response = httpClient.GetAsync(CName).Result;
            }
            else
            {
                throw new Exception("Unsupported Method.");
            }
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
            httpClient.Dispose();

            return responseString;
        }
    }
}
