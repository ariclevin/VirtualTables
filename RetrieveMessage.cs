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
    public class RetrieveMessage : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.Get<IPluginExecutionContext>();
            Guid id = Guid.Empty;
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is EntityReference)
            {
                EntityReference entityRef = (EntityReference)context.InputParameters["Target"];
                Entity e = new Entity("bas_servicerequest");
                //change the table name below to the source table name you have created  
                string cmdString = "SELECT ServiceRequestId, ServiceRequestNumber, Title, Description, CreatedOn, DueOn, Severity " +
                                   "FROM ServiceRequest WHERE ServiceRequestId=@ServiceRequestId";
                SqlConnection connection = Connection.GetConnection();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = cmdString;
                    command.Parameters.AddWithValue("@ServiceRequestId", entityRef.Id);
                    connection.Open();
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                e.Attributes.Add("bas_servicerequestid", reader.GetGuid(0));
                                e.Attributes.Add("bas_srnumber", reader.GetString(1));
                                e.Attributes.Add("bas_name", reader.GetString(2));
                                e.Attributes.Add("bas_description", reader.GetString(3));
                                e.Attributes.Add("bas_createdon", reader.GetDateTime(4));
                                e.Attributes.Add("bas_dueon", reader.GetDateTime(5));
                                e.Attributes.Add("bas_severity", reader.GetInt32(6));
                            }
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                    // other codes. 
                }
                context.OutputParameters["BusinessEntity"] = e;
            }
        }
    }
}
