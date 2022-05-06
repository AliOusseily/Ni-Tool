using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;

namespace AggregatorApi.Services
{
    public class AggregatingService
    {
        public IConfiguration configuration { get; }

        //private readonly ILogger _logger;
        public AggregatingService(IConfiguration _configuration/*, ILogger logger*/)
        {
            configuration = _configuration;
            //_logger = logger;
        }

        public void Aggregate()
        {
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");

            string HourlyAggregateScript = "INSERT INTO TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER_hourly(Time,Link,NeAlias,NeType,SLOT,MaxRxLevel,MaxTxLevel , RSL_DEVIATION)\n" +
                "select date_trunc('hour', Time),Link,NeAlias, NeType,SLOT,MaxRxLevel,MaxTxLevel,abs(MaxRxLevel - MaxTxLevel) as RSL_DEVIATION\n" +
                "from TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER \n" +
                "where date_trunc('hour', Time) \n" +
                "NOT IN(select date_trunc('hour',Time)from TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER_hourly)\n " +
                "group by 1,2,3,4,5,6,7";


            string DailyAggregateScript = "INSERT INTO TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER_Daily(Time,Link,NeAlias,NeType,SLOT,MaxRxLevel,MaxTxLevel , RSL_DEVIATION)\n " +
                "select date_trunc('day', Time),Link,NeAlias, NeType,SLOT,MaxRxLevel,MaxTxLevel,abs(MaxRxLevel - MaxTxLevel) as RSL_DEVIATION \n" +
                "from TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER_hourly\n " +
                " where date_trunc('day', Time)\n " +
                "NOT IN(select date_trunc('day',Time)from TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER_Daily) \n " +
                " group by 1, 2, 3, 4,5,6,7";


            OdbcCommand HourlyAggregateData = new OdbcCommand(HourlyAggregateScript);
            OdbcCommand DailyAggregateData = new OdbcCommand(DailyAggregateScript);

            using (OdbcConnection conn = new OdbcConnection(connectionString))
            {
                HourlyAggregateData.Connection = conn;
                conn.Open();
                HourlyAggregateData.ExecuteNonQuery();
                DailyAggregateData.Connection = conn;
                DailyAggregateData.ExecuteNonQuery();
                conn.Close();
            }


        }
    }
}
