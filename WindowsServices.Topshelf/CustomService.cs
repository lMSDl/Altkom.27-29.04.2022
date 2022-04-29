using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;

namespace WindowsServices.Topshelf
{
    public class CustomService : ServiceControl
    {
        private string _fileName = "c:\\CustomService\\TopshelfService.txt";

        private void WriteMessage(string message)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_fileName));
            File.AppendAllText(_fileName, $"{DateTime.Now.ToShortTimeString()}: {message}\n");
        }

        private Timer _timer;

        public bool Start(HostControl hostControl)
        {
            WriteMessage("Starting...");

            _timer = new Timer(Work, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return true;
        }
        protected void Work(object _)
        {
            WriteMessage("I am working hard...");
        }

        public bool Stop(HostControl hostControl)
        {
            _timer.Dispose();

            return true;
        }
    }
}
