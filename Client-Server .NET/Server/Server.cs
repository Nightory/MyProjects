using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.IO;

namespace chat
{
    class Server
    {
        static TcpListener tcpListener;
        TcpClient client;
        string Path = "E:/user.txt";
        List<ClientInfo> clients = new List<ClientInfo>();
        string Username = "";
        string Password = "";

        protected internal void Listener()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 8005);
                tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");
                while (true)
                {
                    client = tcpListener.AcceptTcpClient();
                    Thread clientThread = new Thread(Receive);
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }
        protected internal void SaveToFile()
        {
            using (TextWriter tw = new StreamWriter(Path,true))
            {
                    tw.WriteLine(String.Format("{0} {1}", Username, Password));
            }
        }
        protected internal bool ReadToFile()
        {
            using (TextReader tr = new StreamReader(Path))
            {
                string line;
                string user_log;
                string user_pass;
                string message;
                while ((line = tr.ReadLine()) != null)
                {
                    string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    user_log = new string(words[0]);
                    user_pass = new string(words[1]);
                    if (user_log == Username && user_pass == Password)
                    {
                        ClientInfo clientObject = new ClientInfo(Username, Password, client);
                        clients.Add(clientObject);
                        message = Username + " вошел в чат ";
                        Console.WriteLine(message);
                        SendAllMessage(message);
                        return true;
                    }
                }
            }
            return false;
        }
        protected internal void Reg_Connect(string message)
        {
            char[] login;
            char[] pass;
            string codes;
            string[] words = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            codes = new string(words[0]);
            Username = new string(words[1]);
            Password = new string(words[2]);
            Console.WriteLine(codes + " " + Username + " " + Password);
            if (codes == "43792")       
            { 
                SaveToFile();
                message = new string("Регистрация прошла успешно");
                Console.WriteLine(message);
                SendOneMessage(message);
            }
            else if(codes=="63582")
            {
                bool search=ReadToFile();
                if (search == false)
                {
                    message = new string("97564 Такого пользователя нет, пройдите регистрацию");
                    Console.WriteLine(message);
                    SendOneMessage(message);
                }
            }
            }
        protected internal void SendOneMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            NetworkStream stream = client.GetStream();
            try
            {
                data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        protected internal void SendAllMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].SendMessage(message);
            }
        }
        
        protected internal void Receive()
        {
            try
            {
                int count = 0, first = 0;
                if (first == 0)
                {
                    string message = "";
                    byte[] data = new byte[256];
                    NetworkStream stream = client.GetStream();
                    while (stream.DataAvailable)
                    {
                        stream.Read(data, 0, data.Length);
                        message = Encoding.Unicode.GetString(data);
                    }
                    Reg_Connect(message);
                    first++;
                }
                while (true)
                {
                    string message="";
                    try
                    {
                        for (int i = 0; i < clients.Count; i++)
                        {
                            byte[] data = new byte[256];
                            NetworkStream stream = clients[i].client.GetStream();
                            while (stream.DataAvailable)
                            {
                                stream.Read(data, 0, data.Length);
                                message = Encoding.Unicode.GetString(data);
                            }

                            if (message != "")
                            {
                                string[] words = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                Console.WriteLine("Получено сообщение: " + message);
                                if (words[0] == "85345")
                                {
                                    message = clients[i].username + " покинул чат";
                                    clients.RemoveAt(i);
                                    SendAllMessage(message);
                                    message = "";
                                }
                                else
                                {
                                    SendAllMessage(message);
                                    message = "";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        protected internal void Disconnect()
        {
            tcpListener.Stop(); 

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); 
            }
            Environment.Exit(0); 
        }
    }
}
