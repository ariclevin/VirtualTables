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
    public class UpdateMessage : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.Get<IPluginExecutionContext>();
            Guid id = Guid.Empty;
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];
                //change the table name below to the source table name you have created  
                string cmdString = "UPDATE ServiceRequest SET {0} WHERE ServiceRequestId=@ServiceRequestId";
                SqlConnection connection = Connection.GetConnection();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.Parameters.AddWithValue("@ServiceRequestId", entity["bas_servicerequestid"]);
                    List<string> setList = new List<string>();
                    if (entity.Attributes.Contains("bas_name"))
                    {
                        command.Parameters.AddWithValue("@Title", entity["bas_name"]);
                        setList.Add("Title=@Title");
                    }
                    if (entity.Attributes.Contains("bas_description"))
                    {
                        command.Parameters.AddWithValue("@Description", entity["bas_description"]);
                        setList.Add("Description=@Description");
                    }

                    if (entity.Attributes.Contains("bas_severity"))
                    {
                        command.Parameters.AddWithValue("@Severity", entity["bas_severity"]);
                        setList.Add("Severity=@Severity");
                    }
                    command.CommandText = string.Format(cmdString, string.Join(",", setList)); connection.Open();
                    try
                    {
                        var numRecords = command.ExecuteNonQuery();
                        Console.WriteLine("updated {0} records", numRecords);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    // other codes. 
                }
            }
        }
    }
}
