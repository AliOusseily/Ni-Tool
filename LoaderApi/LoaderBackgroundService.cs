using LoaderApi.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LoaderApi
{
    public class LoaderBackgroundService : BackgroundService
    {
        private readonly LoaderService _loaderService;
        public LoaderBackgroundService(LoaderService loaderService)
        {
            _loaderService = loaderService;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Running in background process");

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = @"C:\Users\User\Desktop\Projects\Parsed";
            watcher.NotifyFilter = NotifyFilters.Attributes
            | NotifyFilters.CreationTime
            | NotifyFilters.DirectoryName
            | NotifyFilters.FileName
            | NotifyFilters.LastAccess
            | NotifyFilters.LastWrite
            | NotifyFilters.Security
            | NotifyFilters.Size;
            watcher.Created += OnCreated;
            //watcher.Changed += OnChanged;
            watcher.Filter = "*.*";
            watcher.EnableRaisingEvents = true;
            return Task.CompletedTask;


        }


        private  void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);
            //Thread.Sleep(50000);
            //_loaderService.Load(e.FullPath);


        }
        //private void OnChanged(object sender, FileSystemEventArgs e)
        //{
        //    if (e.ChangeType != WatcherChangeTypes.Changed)
        //    {
        //        return;
        //    }
        //    Console.WriteLine($"Changed: {e.FullPath}");
        //}


    }
}
