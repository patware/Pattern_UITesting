using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PingButton_Click(object sender, RoutedEventArgs e)
        {
            var uri = "127.0.0.1:50051";

            var channel = new Grpc.Core.Channel(uri, Grpc.Core.ChannelCredentials.Insecure);

            var client = new ClassLibrary1.Protos.Ping.PingClient(channel);

            try
            {
                var pingResult = client.Ping(new Google.Protobuf.WellKnownTypes.Empty());
                PingLabel.Content = pingResult.Result;
            }
            catch(Exception ex)
            {
                PingLabel.Content = ex.Message;
            }
            

            channel.ShutdownAsync().Wait();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true});
            e.Handled = true;
        }
    }
}
