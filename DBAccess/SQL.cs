using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DBAccess
{
    public class SQL
    {
        CommonParameter[] _CParameters;
        String _SPName = "";
        string _connectionString = "";

        public SQL(string connectionString) { _connectionString = connectionString; }

        public object Execute(string StoredProc, CommonParameter[] commonParameters)
        {
            _CParameters = commonParameters;
            _SPName = StoredProc;
            return Execute();
        }

        public object Execute()
        {
            DataSet dat = new DataSet();

            //ADDING DICTIONARY CONNECTION BASED ON PREFIX
            string ss = _connectionString;// Properties.Settings.Default.connectionString;

            SqlDataAdapter adap = new SqlDataAdapter(_SPName, ss);
            adap.SelectCommand.CommandType = CommandType.StoredProcedure;

            //ADDING PARAMETERS
            if (_CParameters != null) { foreach (CommonParameter p in _CParameters) adap.SelectCommand.Parameters.AddWithValue("@" + p.Name, p.Value); }

            adap.Fill(dat);
            return dat;
        }

    }
}
