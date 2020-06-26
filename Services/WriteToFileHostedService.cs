using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MoviesAPI.Services
{
    public class WriteToFileHostedService: IHostedService
    {
        private IWebHostEnvironment _env;
        private readonly string _filename = "File1.txt";

        public WriteToFileHostedService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            WriteToFile("Process started!");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            WriteToFile("Process stopped!");
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            WriteToFile("Process on going... " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
        }

        private void WriteToFile(string message)
        {
            var path = $@"{_env.ContentRootPath}\wwwroot\{_filename}";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
            }
        }
    }
}