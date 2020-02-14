using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Helper
{
    public abstract class ExtController : Controller
    {
        public Helper.SynchronousRequest SyncRequest;
        public abstract object TempObject { get; set; }
        public ExtController(string BaseUrl)
        {
            SyncRequest = new Helper.SynchronousRequest("http://192.168.1.100:90/api/");
        }

        public ActionResult Ok(object obj)
        {
            TempObject = obj;
            if (obj.GetType() == typeof(string))
            {
                return Content(obj.ToString());
            }

            return new JsonResult()
            {
                Data = TempObject,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}