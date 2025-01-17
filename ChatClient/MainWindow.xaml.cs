using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NetworkStream stream;
        public MainWindow()
        {
            InitializeComponent();
            TcpClient client = new TcpClient("127.0.0.1", 5000);
            stream = client.GetStream();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var requestData = new
            {
                message = MessageBox.Text,
                user = NameBox.Text,
            };
            var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
            try
            {
                    
                    byte[] data = Encoding.UTF8.GetBytes(jsonRequest);
                    stream.Write(data, 0, data.Length);
                    byte[] buffer = new byte[1024];
                    int bytesRead;
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        ChatBlock.Text = ChatBlock.Text + message;
                        break;
                    }
                    
                
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

    }
}