﻿using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace ChatAppNet
{
    internal class Server
    {
        List<TcpClient> clients = new List<TcpClient>();
        List<string> messages = new List<string>();
        TcpListener _listener;
        Server()
        {
            _listener = new TcpListener(IPAddress.Any, 5000);
        }

        async void Start()
        {
            _listener.Start();
            Console.WriteLine("Сервер запущен на порту 5000");
            while (true)
            {
                clients.Add(_listener.AcceptTcpClient());
                Console.WriteLine("Клиент подключился");
                await HandlerSocket(clients.Last());
                //client.Close();
                foreach (var i in clients) {
                    byte[] res = Encoding.UTF8.GetBytes(messages.Last());
                    (i.GetStream()).Write(res, 0, res.Length);
                }
            }
        }
        async public Task HandlerSocket(object socket)
        {
            var client = (TcpClient)socket;
                var stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    dynamic responseData = Newtonsoft.Json.JsonConvert.DeserializeObject(message);
                    Console.WriteLine($"Сообщение от пользователя {responseData.user} получено: {responseData.message}");
                    messages.Add($"{responseData.user}: {responseData.message}\n");
                    break;
                }
            }

        void TwoMess(object socket) {
            using (var client = (TcpClient)socket) {
                var stream2 = client.GetStream();
                byte[] res = Encoding.UTF8.GetBytes(messages.Last());
                stream2.Write(res, 0, res.Length);
            }
        }

        static void Main(string[] args)
        {
            var server = new Server();
            server.Start();
        }
    }
}
