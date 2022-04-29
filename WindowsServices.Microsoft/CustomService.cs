using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsServices.Microsoft
{
    public class CustomService : ServiceBase
    {
        private string _fileName = "c:\\CustomService\\MicrosoftService.txt";

        private void WriteMessage(string message)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_fileName));
            File.AppendAllText(_fileName, $"{DateTime.Now.ToShortTimeString()}: {message}\n");
        }

        private Timer _timer;

        public void Start(string[] args)
        {
            OnStart(args);
        }

        protected override void OnStart(string[] args)
        {
            WriteMessage("Starting...");

            _timer = new Timer(Work, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            base.OnStart(args);
        }

        protected void Work(object _)
        {
            WriteMessage("I am working hard...");
        }

        public void Stop()
        {
            OnStop();
        }

        protected override void OnStop()
        {
            WriteMessage("Stopping...");
            _timer.Dispose();
            base.OnStop();
        }

    }
}
