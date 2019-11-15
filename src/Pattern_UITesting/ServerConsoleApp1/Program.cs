using System;

namespace ServerConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting the service...");

            var server = new Grpc.Core.Server
            {
                Services = { ClassLibrary1.Protos.Ping.BindService(new PingImplementation()) },
                Ports = { new Grpc.Core.ServerPort("localhost", 50051, Grpc.Core.ServerCredentials.Insecure) }
            };

            server.Start();

            Console.WriteLine("Ping server listening on port 50051");
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
