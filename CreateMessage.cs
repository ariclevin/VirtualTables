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
    public class CreateMessage : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.Get<IPluginExecutionContext>();
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];
                Guid id = Guid.NewGuid();
                string cmdString = "INSERT INTO ServiceRequest (ServiceRequestId, ServiceRequestNumber,Title,Description,CreatedOn,DueOn,Severity) VALUES " +
                                   "(@ServiceRequestId, @ServiceRequestNumber, @Title, @Description, @CreatedOn, @DueOn, @Severity)";
                SqlConnection connection = Connection.GetConnection();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = cmdString;
                    command.Parameters.AddWithValue("@ServiceRequestId", id);
                    command.Parameters.AddWithValue("@ServiceRequestNumber", entity["bas_srnumber"]);
                    command.Parameters.AddWithValue("@Title", entity["bas_name"]);
                    command.Parameters.AddWithValue("@Description", entity["bas_description"]);
                    command.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                    command.Parameters.AddWithValue("@DueOn", DateTime.Now.AddDays(2));
                    command.Parameters.AddWithValue("@Severity", entity["bas_severity"]);

                    connection.Open();
                    try
                    {
                        var numRecords = command.ExecuteNonQuery();
                        Console.WriteLine("inserted {0} records", numRecords);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    // other codes. 
                }
                context.OutputParameters["id"] = id;
            }
        }
    }
}
