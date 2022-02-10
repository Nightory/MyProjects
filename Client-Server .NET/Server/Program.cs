﻿using System;
using System.Threading;

namespace chat
{
    class Program
    {
        static Server server; 
        static Thread listenThread;
        static void Main(string[] args)
        {
            try
            {
                server = new Server();
                listenThread = new Thread(new ThreadStart(server.Listener));
                listenThread.Start(); 
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
