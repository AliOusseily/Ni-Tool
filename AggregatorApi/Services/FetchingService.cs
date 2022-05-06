using AggregatorApi.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;

namespace AggregatorApi.Services
{
    public class FetchingService
    {
        public IConfiguration configuration { get; }

        public FetchingService(IConfiguration _configuration)
        {
                configuration = _configuration;
        }

        public IList<AggregatedField> GetHourlyData(/*DateTime From, DateTime To*/)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");
            string query = "select Time,Link,Max(MaxRxLevel),Max(MaxTxLevel),Max(RSL_DEVIATION)\n" +
                "from TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER_hourly\n" +
                "where NeAlias = ' TN-ALT13.1'\n" +
                "group by 1,2";
            IList<AggregatedField> Fields = new List<AggregatedField>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                OdbcCommand command = new OdbcCommand(query, connection);

                connection.Open();

                // Execute the DataReader and access the data.
                OdbcDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    AggregatedField field = new AggregatedField();
                    //field.NetworkSID = (int)(long)reader["NetworkSID"];
                    field.Time = (DateTime)reader["Time"];
                    field.Link = (string)reader["Link"];
                    //field.SLOT = (int)(long)reader["SLOT"];
                    field.MaxRxLevel = (double)reader[2];
                    field.MaxTxLevel = (double)reader[3];
                    field.RSL_DEVIATION = (double)reader[4];
                    Fields.Add(field);
                }

                // Call Close when done reading.
                reader.Close();
            }
            return Fields;
        }public IList<AggregatedField> GetDailyData(/*DateTime From,DateTime To*/)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");
            string query = "select Time,Link,Max(MaxRxLevel),Max(MaxTxLevel),Max(RSL_DEVIATION)\n" +
                      "from TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER_Daily\n" +
                     $"where NeAlias = ' TN-ALT13.1'\n" +
                      "group by 1,2";
            IList<AggregatedField> Fields = new List<AggregatedField>();

            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                OdbcCommand command = new OdbcCommand(query, connection);

                connection.Open();

                // Execute the DataReader and access the data.
                OdbcDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    AggregatedField field = new AggregatedField();
                    //field.NetworkSID = (int)(long)reader["NetworkSID"];
                    field.Time = (DateTime)reader["Time"];
                    field.Link = (string)reader["Link"];
                    //field.SLOT = (int)(long)reader["SLOT"];
                    field.MaxRxLevel = (double)reader[2];
                    field.MaxTxLevel = (double)reader[3];
                    field.RSL_DEVIATION = (double)reader[4];
                    Fields.Add(field);
                }

                // Call Close when done reading.
                reader.Close();
            }
            return Fields;
        }

    }
}
