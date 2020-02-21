using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using System.Reflection;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Helper
{
    public class RoleAccessDataAttribute : ActionFilterAttribute
    {
        public RoleAccess _ra;


        /// <summary>
        /// Check User if Allowed to access the method.
        /// </summary>
        /// <param name="apiLink">URL</param>
        /// <param name="method">POST or PUT or DELETE or GET</param>
        /// <param name="map"> Data </param>
        /// <param name="TestExpression"> SystemName=Inventory, controlName=Names, Action=delete, isActive=1 </param>
        /// <param name="UserIDKey"> UserID Key Names from Request Header </param>
        /// <param name="TokenKey"> Token Key Name from Requests Header </param>
        public RoleAccessDataAttribute(string apiLink, string method, string map, string TestExpression, string UserIDKey, string TokenKey)
        {



            _ra = new RoleAccess();
            _ra.tokenKey = TokenKey;
            _ra.userIDKey = UserIDKey;
            _ra.apiLink = apiLink;
            _ra.method = method.ToUpper();
            _ra.map = map;
            if (TestExpression != null)
            {
                SetMatchExpression(TestExpression);
            }
        }

        public void SetMatchExpression(string TestExpression)
        {
            TestExpression = TestExpression.ToLower();
            _ra.TestMatchExpressionValue = new Dictionary<string, string>();
            foreach (string ex in TestExpression.Split(','))
            {
                string[] s = ex.Split('=');

                if (s.Length > 1)
                {
                    _ra.TestMatchExpressionValue.Add(s[0].Trim(), s[1].Trim());
                }
            }
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var request = actionContext.ControllerContext.Request.Headers;
            string ID = "9";
            string Token = "XUpGOIEZBZhrB1hg32CHXopNg";

            if (request.Contains(_ra.userIDKey))
            {
                ID = request.GetValues(_ra.userIDKey).First();
            }
            else if (request.Contains(_ra.tokenKey))
            {
                Token = request.GetValues(_ra.tokenKey).First();
            }

            SynchronousRequest sRequest = new SynchronousRequest(_ra.apiLink);
            sRequest.httpClient.DefaultRequestHeaders.Add("UserID", ID);
            sRequest.httpClient.DefaultRequestHeaders.Add("Token", Token);
            string ss = sRequest.HttpRequest("", _ra.method, null);
            JToken token = JsonConvert.DeserializeObject(ss) as JToken;


            JArray jArray = new JArray();
            if (token is JArray)
            {
                if (_ra.map == null || _ra.map == string.Empty)
                {
                    throw new Exception("Array detected, its not supported you may set map to null or empty.");
                }
                jArray = token as JArray;
            }
            else if (token is JObject)
            {
                JObject jObject = token as JObject;
                jArray = jObject[_ra.map] as JArray;
            }
            //SENT INVALID EMPTY
            if (jArray == null || jArray.Count <= 0) return;

            bool isValid = false;
            foreach (JObject j in jArray)
            {
                //Validation
                int validCount = _ra.TestMatchExpressionValue.Keys.Count;
                foreach (string s in _ra.TestMatchExpressionValue.Keys)
                {
                    //None Case Sensitive 
                    string jValue = j.GetValue(s, StringComparison.OrdinalIgnoreCase)?.Value<string>().ToLower();
                    if (jValue == _ra.TestMatchExpressionValue[s])
                    {
                        validCount--;
                    }
                }
                if (validCount == 0)
                {
                    isValid = true;
                    break;
                }
            }
            if (!isValid)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            //Console.Write(isValid);
            base.OnActionExecuting(actionContext);
        }
    }
    public class RoleAccess
    {
        public string apiLink { get; set; }
        public string map { get; set; }
        public string method { get; set; }
        public Dictionary<string, string> TestMatchExpressionValue { get; set; }
        public string userIDKey { get; set; }
        public string tokenKey { get; set; }

    }

}