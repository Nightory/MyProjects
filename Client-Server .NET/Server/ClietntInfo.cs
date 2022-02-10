using System;
using System.Net.Sockets;
using System.Text;

namespace chat
{
    class ClientInfo
    {
        protected internal string username { get; set; }
        protected internal string password { get; set; }
        protected internal TcpClient client { get; set; }
        public ClientInfo(string username, string password, TcpClient client)
        {
            this.username = username;
            this.password = password;
            this.client = client;
        }
        public void SendMessage(string message)
        {
            byte[] data = new byte[256];
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
        
        protected internal void Close()
        {
            if (client != null) client.Close();
        }
    }
}
