using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsService.Core
{
    public class Worker : BackgroundService
    {
        private string _fileName = "c:\\CustomService\\CoreService.txt";

        private void WriteMessage(string message)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_fileName));
            File.AppendAllText(_fileName, $"{DateTime.Now.ToShortTimeString()}: {message}\n");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            WriteMessage("Starting...");
            while (!stoppingToken.IsCancellationRequested)
            {
                WriteMessage("I am working hard...");
                await Task.Delay(5000, stoppingToken);
            }
            WriteMessage("Stopping...");
        }
    }
}
