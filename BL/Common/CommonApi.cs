using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BL.Common
{
    public abstract class CommonApi : ApiController
    {
        /// <summary>
        /// Providing CName will be automatically added to your [Stored Procedure] Parameter
        /// </summary>
        public string CName { get; set; }

        public BL.ISQLBL _sQLBL;

        /// <summary>
        /// Values sample
        /// </summary>
        /// <param name="sQLBL">Filter by Autofac based</param>
        public CommonApi(BL.ISQLBL sQLBL)
        {
            _sQLBL = sQLBL;
        }

        public abstract object Get();

        public abstract object Get(int id);

        //public abstract object Post([FromBody]object value);

       // public abstract object Put(int id, [FromBody]object value);

        public abstract object Delete(int id);
    }
}
