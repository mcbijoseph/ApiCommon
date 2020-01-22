using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http.Formatting;


namespace BL.Filters
{
    public class ProduceResultAttribute : ActionFilterAttribute
    {
        string _contentType = "";
        public ProduceResultAttribute(string ContentType)
        {
            _contentType = ContentType;
        }
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var objectContent = actionExecutedContext.Response.Content as ObjectContent;
            if (objectContent != null && _contentType != "")
                actionExecutedContext.Response.Content = new StringContent(objectContent.Value.ToString(), System.Text.Encoding.UTF8, _contentType);

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}