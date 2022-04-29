using System;
using Topshelf;

namespace WindowsServices.Topshelf
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<CustomService>();
                x.SetDisplayName("CustomService");
                x.SetDescription("Topshelf");

                x.EnableServiceRecovery(config =>
                {
                    config
                    .RestartService(TimeSpan.FromSeconds(1))
                    .RestartService(TimeSpan.FromSeconds(3))
                    .RestartService(TimeSpan.FromSeconds(5));

                    config.SetResetPeriod(5);
                });

                x.RunAsLocalSystem();
                x.StartAutomaticallyDelayed();
            });
        }
    }
}
