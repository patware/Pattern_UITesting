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
            System.Diagnostics.Debug.WriteLine($"Starting the PingMock service at Port {Port}");
            PingServiceMock = new Mocking.PingMock();
            
            Server = new Grpc.Core.Server
            {
                Services = { ClassLibrary1.Protos.Ping.BindService(PingServiceMock) },
                Ports = { new Grpc.Core.ServerPort("localhost", port: Port, Grpc.Core.ServerCredentials.Insecure) }
            };

            Server.Start();
            System.Diagnostics.Debug.WriteLine($"Server Started.");
        }
        [TestCleanup]
        public void Cleanup()
        {
            System.Diagnostics.Debug.WriteLine($"Stopping Service...");
            Server.ShutdownAsync().Wait();
            System.Diagnostics.Debug.WriteLine($"We're done.");
        }
        [TestMethod]
        public void TestMethod1()
        {
            System.Diagnostics.Debug.WriteLine($"Specifying the text that will be returned by the PingServiceMock to validate that the WpfClient calls this Mock and not another service.");
            PingServiceMock.PingReturn = "Pong From Mock";

            System.Diagnostics.Debug.WriteLine($"TestMethod1: Setting up Appium");
            var options = new OpenQA.Selenium.Appium.AppiumOptions();
            var exeLocation = System.IO.Path.GetFullPath(@"..\..\..\..\WpfApp1\bin\Debug\netcoreapp3.0\WpfApp1.exe");
            options.AddAdditionalCapability("app",exeLocation);
            
            var wpfSession = new OpenQA.Selenium.Appium.Windows.WindowsDriver<OpenQA.Selenium.Appium.Windows.WindowsElement>(
                new System.Uri("http://127.0.0.1:4723"),
                options, 
                new System.TimeSpan(1,0,0));

            var pingButton = wpfSession.FindElementByAccessibilityId("PingButton");
            pingButton.Click();

            var pingLabel = wpfSession.FindElementByAccessibilityId("PingLabel");
            Assert.AreEqual("Pong From Mock", pingLabel.Text);

        }
    }
}
