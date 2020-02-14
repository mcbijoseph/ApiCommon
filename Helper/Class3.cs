using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Helper
{
    public class ProducesJsonAttribute: ActionFilterAttribute
    {


        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ExtController ext = filterContext.Controller as ExtController;
            object ob = filterContext.RouteData.Values["action"];

            var resbase = filterContext.Result as JsonResult;
            if (resbase != null)
            {
                resbase.ContentType = "application/json";
            }
            else
            {
                filterContext.HttpContext.Response.ContentType = "application/json";
            }

            base.OnActionExecuted(filterContext);

        }
    }
}