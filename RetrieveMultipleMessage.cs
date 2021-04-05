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
    public class RetrieveMultipleMessage : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.Get<IPluginExecutionContext>();
            EntityCollection collection = new EntityCollection();
            string cmdString = "SELECT ServiceRequestId, ServiceRequestNumber, Title, Description, " +
                               "CreatedOn, DueOn, Severity FROM ServiceRequest";
            SqlConnection connection = Connection.GetConnection();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = cmdString;
                connection.Open();
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Entity e = new Entity("bas_servicerequest");
                            e.Attributes.Add("bas_servicerequestid", reader.GetGuid(0));
                            e.Attributes.Add("bas_srnumber", reader.GetString(1));
                            e.Attributes.Add("bas_name", reader.GetString(2));
                            e.Attributes.Add("bas_description", reader.GetString(3));
                            e.Attributes.Add("bas_createdon", reader.GetDateTime(4));
                            e.Attributes.Add("bas_dueon", reader.GetDateTime(5));
                            e.Attributes.Add("bas_severity", reader.GetInt32(6));
                            collection.Entities.Add(e);
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
                context.OutputParameters["BusinessEntityCollection"] = collection;
            }
        }
    }
}
