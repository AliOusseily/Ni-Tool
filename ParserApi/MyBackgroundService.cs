using Microsoft.Extensions.Hosting;
using ParserApi.Services;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ParserApi
{
    public class MyBackgroundService : BackgroundService
    {
        private readonly ParsingService _parsingService;
        private readonly HttpRequestService _httpRequestService;
        public MyBackgroundService(ParsingService parsingService, HttpRequestService httpRequestService)
        {
            _parsingService = parsingService;
            _httpRequestService = httpRequestService;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Running in background process");

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = @"C:\Users\User\Desktop\Projects\Unparsed";
            watcher.NotifyFilter = NotifyFilters.Attributes
            | NotifyFilters.CreationTime
            | NotifyFilters.DirectoryName
            | NotifyFilters.FileName
            | NotifyFilters.LastAccess
            | NotifyFilters.LastWrite
            | NotifyFilters.Security
            | NotifyFilters.Size;
            watcher.Created += OnCreated;
            watcher.Changed += OnChanged;
            watcher.Filter = "*.txt";
            watcher.EnableRaisingEvents = true;
            return Task.CompletedTask;

        }
        private  void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);
            string parsedFilePath = _parsingService.Parse(e.FullPath);
            string jsonString = JsonSerializer.Serialize(new FilePathObjectRequest() { FilePath = parsedFilePath });
            var result = _httpRequestService.UpdateLoaderApi(jsonString).Result;
        }
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine($"Changed: {e.FullPath}");
        }

        public class FilePathObjectRequest
        {
            public string FilePath { get; set; }
        }
    }
}
