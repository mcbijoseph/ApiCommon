using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AddingConnection
    {
            /*
            AddingConnectionConfig();
            System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~"); //; System.Web.Configuration.WebConfigurationManager.ConnectionStrings["sample"].ConnectionString;// System.Configuration.ConfigurationManager.ConnectionStrings[0].ConnectionString;
            string s2 =  config.ConnectionStrings.ConnectionStrings["sample"].ConnectionString;
            */
            public void AddingConnectionConfig()
            {
                //USE ONLY ON WEB
                //System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");

                //USE ONLY ON EXE
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("sample", "server=sample;db=sample;pass=sample;user=sample", "MySQL"));
                config.ConnectionStrings.ConnectionStrings.Remove("sample");
                config.Save(ConfigurationSaveMode.Modified);

            }
        
    }
}
