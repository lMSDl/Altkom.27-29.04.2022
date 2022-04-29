using System;
using System.ServiceProcess;

namespace WindowsServices.Microsoft
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new CustomService();

            if(Environment.UserInteractive)
            {
                service.Start(args);

                Console.ReadLine();

                service.Stop();

            }
            else
            ServiceBase.Run(service);
        }
    }
}
