using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BL.Filters
{
    public class ValidateDomainModelAttribute : ActionFilterAttribute
    {
        Type _type;
        string _value;
        /// <summary>
        /// Domain or Model Validator
        /// </summary>
        /// <param name="type">ClassName</param>
        public ValidateDomainModelAttribute(string value, Type type)
        {
            _value = value;
            _type = type;
        }
        public ValidateDomainModelAttribute()
        {

        }

        public void ActionWithParameter(HttpActionContext actionContext)
        {
            //Getting the request value
            string jsonValue = _value;
            if (jsonValue.First() == '{' && jsonValue.Last() == '}')
                jsonValue = actionContext.ActionArguments[jsonValue.Substring(1, jsonValue.Length - 2)].ToString();


            //Validate here
            object direct = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonValue, _type);

            if (direct != null)
            {

                var context = new System.ComponentModel.DataAnnotations.ValidationContext(direct, null, null);
                List<ValidationResult> result = new List<ValidationResult>();
                var isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(direct, context, result, true);

                if (!isValid)
                {
                    actionContext.Response = new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.BadRequest
                    };
                    JArray jo = new JArray();
                    //Newtonsoft.Json.Linq.JObject jo = new Newtonsoft.Json.Linq.JObject();
                    foreach (var str in result)
                    {
                        string keyname = str.MemberNames.FirstOrDefault();
                        string keyvalue = str.ToString();
                        JObject job = new JObject();
                        job.Add(new JProperty(keyname, keyvalue));
                        jo.Add(job);
                    }

                    string res = Newtonsoft.Json.JsonConvert.SerializeObject(jo);
                    actionContext.Response.Content = new StringContent(res, System.Text.Encoding.UTF8, "application/json");

                }

            }

        }
        public void ActionWithoutParameter(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var modelErrors = new StringBuilder();

                JArray jo = new JArray();

                foreach (var modelStateKey in actionContext.ModelState.Keys)
                {
                    var modelStateVal = actionContext.ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        var key = modelStateKey;

                        //sanitize key "value."
                        key = key.Length > 4 || key.Contains("value.") ? (key.Substring(0, 6) == "value." ? key.Substring(6) : key) : key;

                        var errorMessage = error.ErrorMessage;
                        var exception = error.Exception;
                        JObject job = new JObject();
                        job.Add(new JProperty(key, errorMessage));
                        jo.Add(job);
                    }
                }
                
                actionContext.Response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                };
                string ss = JsonConvert.SerializeObject(jo);
                actionContext.Response.Content = new StringContent(ss, System.Text.Encoding.UTF8, "application/json".ToString());
                //actionContext.Response.Headers.
            }
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if(_type == null)
            {
                ActionWithoutParameter(actionContext);
            }
            else
            {
                ActionWithParameter(actionContext);
            }

            base.OnActionExecuting(actionContext);
        }
    }
}
