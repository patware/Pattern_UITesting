using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        public const int Port = 50051;
        private Mocking.PingMock PingServiceMock;
        private Grpc.Core.Server Server;

        [TestInitialize]
        public void Init()
        {
            PingServiceMock = new Mocking.PingMock();
            
            Server = new Grpc.Core.Server
            {
                Services = { ClassLibrary1.Protos.Ping.BindService(PingServiceMock) },
                Ports = { new Grpc.Core.ServerPort("localhost", port: Port, Grpc.Core.ServerCredentials.Insecure) }
            };

            Server.Start();
        }
        [TestCleanup]
        public void Cleanup()
        {
            Server.ShutdownAsync().Wait();
        }
        [TestMethod]
        public void TestMethod1()
        {
            PingServiceMock.PingReturn = "Pong";

        }
    }
}
