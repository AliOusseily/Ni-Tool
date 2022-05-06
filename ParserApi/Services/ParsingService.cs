using Microsoft.Extensions.Configuration;
using ParserApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ParserApi.Services
{
    public class ParsingService
    {
        public IConfiguration configuration { get; }
        public ParsingService(IConfiguration _configuration)
        {
            configuration = _configuration;

        }

        //private readonly ILogger _logger;
      
        public string Parse(string UnParsedFilePath)
        {
            NonParsedEntry nonParsedEntry = new NonParsedEntry();
            IList<ParsedEntry> parsedEntries = new List<ParsedEntry>();
            string[] y;
            string fileName = Path.GetFileNameWithoutExtension(UnParsedFilePath);
            string parsedFilePath = $@"C:\Users\User\Desktop\Projects\Parsed\{fileName}_Parsed.csv";
            var lines = File.ReadLines(UnParsedFilePath).Skip(1);
            if (File.Exists(UnParsedFilePath))
            {
                foreach (string line in lines)
                {
                    y = line.Split(',');

                    nonParsedEntry.NodeName = y[0];
                    nonParsedEntry.NeId = float.Parse(y[1]);
                    nonParsedEntry.Object = y[2];
                    nonParsedEntry.Time = y[3];
                    nonParsedEntry.Interval = int.Parse(y[4]);
                    nonParsedEntry.Direction = y[5];
                    nonParsedEntry.NeAlias = y[6];
                    nonParsedEntry.NeType = y[7];
                    nonParsedEntry.Position = y[8];
                    nonParsedEntry.RxLevelBelowTS1 = y[9];
                    nonParsedEntry.RxLevelBelowTS2 = y[10];
                    nonParsedEntry.MinRxLevel = y[11];
                    nonParsedEntry.MaxRxLevel = y[12];
                    nonParsedEntry.TxLevelAboveTS1 = y[13];
                    nonParsedEntry.MinTxLevel = y[14];
                    nonParsedEntry.MaxTxLevel = y[15];
                    nonParsedEntry.IdLogNum = int.Parse(y[16]);
                    nonParsedEntry.FailureDescription = y[17];

                    if (check(nonParsedEntry))
                    {
                        IList<string> slots = getSlot(nonParsedEntry.Object);
                        foreach (string tempslots in slots)
                        {
                            ParsedEntry parsedEntry = new ParsedEntry();
                            parsedEntry.NetworkSID = HashingFunction(nonParsedEntry.Object + nonParsedEntry.NeAlias);
                            parsedEntry.DATETIME_KEY = getFileDate(UnParsedFilePath);
                            parsedEntry.NeId = nonParsedEntry.NeId;
                            parsedEntry.Object = nonParsedEntry.Object;
                            parsedEntry.Time = nonParsedEntry.Time;
                            parsedEntry.Interval = nonParsedEntry.Interval;
                            parsedEntry.Direction = nonParsedEntry.Direction;
                            parsedEntry.NeAlias = nonParsedEntry.NeAlias;
                            parsedEntry.NeType = nonParsedEntry.NeType;
                            parsedEntry.RxLevelBelowTS1 = nonParsedEntry.RxLevelBelowTS1;
                            parsedEntry.RxLevelBelowTS2 = nonParsedEntry.RxLevelBelowTS2;
                            parsedEntry.MinRxLevel = nonParsedEntry.MinRxLevel;
                            parsedEntry.MaxRxLevel = nonParsedEntry.MaxRxLevel;
                            parsedEntry.TxLevelAboveTS1 = nonParsedEntry.TxLevelAboveTS1;
                            parsedEntry.MinTxLevel = nonParsedEntry.MinRxLevel;
                            parsedEntry.MaxTxLevel = nonParsedEntry.MaxTxLevel;
                            parsedEntry.FailureDescription = nonParsedEntry.FailureDescription;
                            parsedEntry.Link = getLink(nonParsedEntry.Object);
                            parsedEntry.Tid = getTID(nonParsedEntry.Object);
                            parsedEntry.FARENDTID = getFARENTID(nonParsedEntry.Object);
                            parsedEntry.SLOT = tempslots;
                            parsedEntry.PORT = getPort(nonParsedEntry.Object);
                            parsedEntries.Add(parsedEntry);

                        }
                    }
                }
                    //_logger.LogInformation("File has been parsed successfully");
            }
            sendEntryToResultCSV(parsedEntries, parsedFilePath);

            string connectionString = configuration.GetConnectionString("DefaultConnectionString");
            DateTime filedate = getFileDate(UnParsedFilePath);
            string query = $"INSERT INTO ParsedFiles(FileName,DateOfFile,DateOfParsing) VALUES ('{fileName}','{filedate}',NOW())";

            OdbcCommand command = new OdbcCommand(query);

            using (OdbcConnection conn = new OdbcConnection(connectionString))
            {
                command.Connection = conn;
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }

            //FilePathObjectRequest file = new FilePathObjectRequest() { FilePath = parsedFilePath };
            //string jsonString = JsonSerializer.Serialize(file);
            return parsedFilePath;
        }

        private bool check(NonParsedEntry nonParsedEntries)
        {
            if (nonParsedEntries.Object == "Unreachable Bulk FC" && nonParsedEntries.FailureDescription != "-")
            {
                return false;

            }
            return true;
        }
        private void sendEntryToResultCSV(IList<ParsedEntry> parsedEntries, string parsedfilepath)
        {

            using (StreamWriter writer = new StreamWriter(parsedfilepath, false))
            {
                string header = "NetworkSID,DATETIME_KEY,NeId,Object,Time,Interval,Direction,NeAlias,NeType,RxLevelBelowTS1,RxLevelBelowTS2,MinRxLevel,MaxRxLevel,TxLevelAboveTS1,MinTxLevel,MaxTxLevel,FailureDescription,Link,Tid,FARENDTID,SLOT,PORT";
                writer.WriteLine(header);
                foreach (var parsedEntry in parsedEntries)
                {
                    string line = $"{parsedEntry.NetworkSID} , {parsedEntry.DATETIME_KEY} , {parsedEntry.NeId} , {parsedEntry.Object} , {parsedEntry.Time} , {parsedEntry.Interval} , {parsedEntry.Direction} , {parsedEntry.NeAlias}, {parsedEntry.NeType} , {parsedEntry.RxLevelBelowTS1} , {parsedEntry.RxLevelBelowTS2} , {parsedEntry.MinRxLevel} , {parsedEntry.MaxRxLevel} , {parsedEntry.TxLevelAboveTS1} , {parsedEntry.MinTxLevel} , {parsedEntry.MaxTxLevel} , {parsedEntry.FailureDescription} , {parsedEntry.Link} , {parsedEntry.Tid} , {parsedEntry.FARENDTID} , {parsedEntry.SLOT} , {parsedEntry.PORT}";
                    writer.WriteLine(line);
                }
            }
        }

        private int HashingFunction(string source)
        {

            // Creates an instance of the default implementation of the MD5 hash algorithm.
            using (var md5Hash = MD5.Create())
            {
                // Byte array representation of source string
                var sourceBytes = Encoding.UTF8.GetBytes(source);

                // Generate hash value(Byte Array) for input data
                var hashBytes = md5Hash.ComputeHash(sourceBytes);

                // Convert hash byte array to int
                var hash = BitConverter.ToInt32(hashBytes);

                // Output the MD5 hash
                Console.WriteLine("The MD5 hash of " + source + " is: " + hash);

                return hash;
            }

        }

        private string getLink(string input)
        {
            string output = "";
            string slot = "";
            string port = "";
            string LinkOutput = "";
            int index = input.IndexOf("_");
            if (index >= 0)
                output = input.Substring(0, index);
            int indexOfDot = output.IndexOf(".");
            if (indexOfDot >= 0) // . exists in the middle
            {
                slot = output.Split(".")[0].Split("/")[1];
                port = output.Split(".")[1].Split("/")[0];

                LinkOutput = $"{slot}/{port}";
            }
            else // + 
            {
                slot = output.Split("/")[1];
                port = output.Split("/")[2];

                LinkOutput = $"{slot}/{port}";

            }
            return LinkOutput;
        }

        private string getTID(string input)
        {
            string TID = "";

            int index1 = (input.IndexOf("_") + 2);
            int index2 = (input.LastIndexOf("_") - 1);

            if (index1 > 0 && index2 > 0)
            {
                TID = input.Substring(index1, index2 - index1);
            }
            return TID;
        }

        private string getFARENTID(string input)
        {
            string Farentid = "";
            int index = (input.LastIndexOf("_"));

            if (index > 0)
            {

                Farentid = input.Substring(index + 1);
            }
            return Farentid;

        }

        private IList<string> getSlot(string input)
        {
            string output = "";
            IList<string> slots = new List<string>();
            int index = input.IndexOf("_");
            if (index >= 0)
                output = input.Substring(0, index);
            int indexOfDot = output.IndexOf(".");
            int indexOfPlus = output.IndexOf("+");
            string slot1 = "";
            if (indexOfDot >= 0) // . exists in the middle
            {
                slot1 = output.Split(".")[0].Split("/")[1];
                slots.Add(slot1);
            }
            else if (indexOfPlus >= 0) // + 
            {
                slot1 = output.Split("/")[1].Split("+")[0];
                string slot2 = output.Split("/")[1].Split("+")[1];
                slots.Add(slot1);
                slots.Add(slot2);

            }
            else
            {
                slot1 = output.Split("/")[1];
                slots.Add(slot1);
            }
            return slots;
        }
        private string getPort(string input)
        {
            string output = "";
            int index = input.IndexOf("_");
            if (index >= 0)
                output = input.Substring(0, index);
            int indexOfDot = output.IndexOf(".");
            int indexOfPlus = output.IndexOf("+");
            string port;
            if (indexOfDot >= 0) // . exists in the middle
            {
                port = output.Split(".")[1].Split("/")[0];
            }
            else
            {
                port = "1";
            }

            return port;
        }

        private DateTime getFileDate(string filepath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filepath);
            string dateOfFile = fileName.Substring(fileName.Length - 15).Replace("_", " ");
            dateOfFile = dateOfFile.Insert(4, "-");
            dateOfFile = dateOfFile.Insert(7, "-");
            dateOfFile = dateOfFile.Insert(13, ":");
            dateOfFile = dateOfFile.Insert(16, ":");
            DateTime timeofFile = DateTime.Parse(dateOfFile);
            return timeofFile;
        }
    }

    
}
