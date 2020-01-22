using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Newtonsoft.Json.Linq;


namespace BL.Filters
{
    public class RequestHeaderAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Common.CommonApi com = actionContext.ControllerContext.Controller as Common.CommonApi;
            JObject jsonheader = new JObject();
            if (com.CName != null)
            {
                if (com.CName.Trim() != "")
                    jsonheader.Add(new JProperty("CName", com.CName));
            }
            //Getting the CName
            //Token from header
            //TokenID from header

            com._sQLBL.SetJSONHeader(jsonheader);

            base.OnActionExecuting(actionContext);
        }
    }
}

