using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace BL.Filters
{
    public class JsonResultAttribute : ActionFilterAttribute
    {
        Type _getType = null;
        public JsonResultAttribute(Type getType)
        {
            _getType = getType;
        }
        public JsonResultAttribute()
        {
        }
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            

            var objectContent = actionExecutedContext.Response.Content as ObjectContent;
            if (objectContent != null)
            {
                if (objectContent.Value.GetType() == typeof(string))
                    actionExecutedContext.Response.Content = new StringContent(objectContent.Value.ToString(), System.Text.Encoding.UTF8, "application/json");
                else if(_getType == null)
                    actionExecutedContext.Response.Content = new ObjectContent(objectContent.Value.GetType(), objectContent.Value, System.Web.Http.GlobalConfiguration.Configuration.Formatters.JsonFormatter, new MediaTypeHeaderValue("application/json"));// 
                else
                {
                   
                        actionExecutedContext.Response.Content = new ObjectContent(_getType, objectContent.Value, System.Web.Http.GlobalConfiguration.Configuration.Formatters.JsonFormatter, new MediaTypeHeaderValue("application/json"));
                }
            }
            base.OnActionExecuted(actionExecutedContext);
        }

    }
    public class MyFormatter : MediaTypeFormatter
    {
        public MyFormatter(string format)
        {

            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue(format));
        }
        
        public override bool CanReadType(Type type)
        {
            //throw new NotImplementedException();
           return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
            //throw new NotImplementedException();
        }
    }
}