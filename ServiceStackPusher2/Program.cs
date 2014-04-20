using System;
using ServiceStack.Logging;

namespace ServiceStackPusher2
{
    class Program
    {
        static void Main(string[] args)
        {
            var listeningOn = args.Length == 0 ? "http://*:1337/" : args[0];

            LogManager.LogFactory = new ConsoleLogFactory();
 
            var appHost = new AppHost();
            appHost.Init();
            appHost.Start(listeningOn);

            Console.WriteLine("AppHost Created at {0}, listening on {1}", DateTime.Now, listeningOn);

            Console.ReadKey();
        }
    }
}
