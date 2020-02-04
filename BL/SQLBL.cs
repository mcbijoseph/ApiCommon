using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;

namespace BL
{
    public interface ISQLBL
    {
        System.Data.DataSet Execute(string procedureName, string json);
        object Result(int i);
        void SetJSONHeader(JObject json);
        void Dispose();
    }

    public class SQLBL : ISQLBL
    {
        SQL _sql;

        public SQLBL(string connectionString) {
            _sql = new SQL(connectionString);
        }

        private System.Data.DataSet _ds;
        private JObject JSONHeader;
        private SqlException _Err;
        public void SetJSONHeader(JObject json )
        {
            JSONHeader = json;
        }
        CommonParameter[] JSONtoCommonParameter(string json)
        {
            Newtonsoft.Json.Linq.JObject jo = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);

            //Additional JSON Data to be added
            if (JSONHeader != null)
            {
                jo.Merge(JSONHeader, new JsonMergeSettings() { MergeArrayHandling = MergeArrayHandling.Union });
            }

            IList<string> keys = jo.Properties().Select(x => x.Name).ToList();
            CommonParameter[] p = new CommonParameter[keys.Count];
            for (int i = 0; i < keys.Count; i++)
            {
                p[i] = new CommonParameter(keys[i], jo[keys[i]].ToString());
            }
            return p;
        }

       
        public System.Data.DataSet Execute(string procedureName, string json)
        {
            var retVal = _sql.Execute(procedureName, JSONtoCommonParameter(json));
            _ds =  retVal as System.Data.DataSet;
            if(_ds == null)
            {
                _Err = retVal as SqlException;
            }


            return _ds;
        }

        public object Result(int i)
        {
            if (_ds == null)
            {
                System.Data.SqlClient.SqlException se = _Err as System.Data.SqlClient.SqlException;
                return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(se.Errors));
            }
            return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(_ds.Tables[i], Formatting.None));
        }
        public void Dispose()
        {
            if (_ds != null)
                _ds.Dispose();
        }
        
    }
}
