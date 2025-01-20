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
using System.Windows.Threading;

namespace ChatClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class Recieve {
        public string text;
        public NetworkStream stream;

        public Recieve(string text, NetworkStream stream)
        {
            this.text = text;
            this.stream = stream;
        }
    }
    public partial class MainWindow : Window
    {
        NetworkStream stream;
        public MainWindow()
        {
            InitializeComponent();
            TcpClient client = new TcpClient("127.0.0.1", 5000);
            stream = client.GetStream();
            Recieve r = new Recieve("", stream);
            Thread thread = new Thread(RecieveMessage);
            thread.Start(r);
            
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
                
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public void RecieveMessage(object obj) {
            var stream = obj as Recieve;
            byte[] buffer = new byte[1024];
            while (true) {
                try
                {
                    int byteRead = stream.stream.Read(buffer, 0, buffer.Length);
                    string message = Encoding.UTF8.GetString(buffer, 0, byteRead);
                    stream.text = message;
                    Dispatcher.Invoke(() => ChatBlock.Text = ChatBlock.Text + message);
                    
                }
                catch (Exception)
                {

                }
            }
        }

    }
}