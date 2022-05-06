
using System.Data.Odbc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IO;

namespace LoaderApi.Services
{
    public class LoaderService

    {
        public IConfiguration configuration { get; }

        //private readonly ILogger _logger;
        public LoaderService(IConfiguration _configuration)
        {
            configuration = _configuration;
            //_logger = logger;
        }



        public void Load(string filepath)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnectionString");
            Console.WriteLine(filepath);
            Console.WriteLine("------------------------------------------------------------------------");
            string filename = Path.GetFileNameWithoutExtension(filepath);
            Console.WriteLine(filename);
            Console.WriteLine("------------------------------------------------------------------------");
            string queryString = $"select FileName from LoadedFiles where FileName = '{filename}'";
            IList<string> result = new List<string>();
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                OdbcCommand command2 = new OdbcCommand(queryString, connection);

                connection.Open();

                // Execute the DataReader and access the data.
                OdbcDataReader reader = command2.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(reader["FileName"].ToString());

                }

                // Call Close when done reading.
                reader.Close();
            }
            //var value = selecrt filename from loadedfiles where filename { filename }
            //if value is null


            if (result.Count == 0)
            {

                string query = $"COPY TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER FROM LOCAL '{filepath}' with DELIMITER as ',' skip 1 ;" +
                    $"INSERT INTO LoadedFiles(FileName,DateOfFile) VALUES ('{filename}',NOW())";

                OdbcCommand command = new OdbcCommand(query);

                using (OdbcConnection conn = new OdbcConnection(connectionString))
                {
                    command.Connection = conn;
                    conn.Open();
                    command.ExecuteNonQuery();
                    conn.Close();
                }
            }

        }
    }
}
