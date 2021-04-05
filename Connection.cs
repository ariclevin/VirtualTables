using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using Microsoft.Xrm.Sdk.Data.Exceptions;
using Newtonsoft.Json;

namespace VirtualTable
{
    public static class Connection
    {
        #region Secured Values

        const string CONNECTION_STRING = "ENTER_CONNECTION_STRING_HERE";
        const string INITIAL_CATALOG = "ENTER_DATABASE_NAME_HERE";
        const string USER_ID= "ENTER_USERNAME_CREDENTIAL_HERE";
        const string PASSWORD = "ENTER_PASSWORD_CREDENTIAL_HERE";


        #endregion

        public static SqlConnection GetConnection()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = CONNECTION_STRING;
                builder.InitialCatalog = INITIAL_CATALOG;
                builder.UserID = USER_ID;
                builder.Password = PASSWORD;
                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                return connection;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
