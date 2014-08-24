using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace Network
{
    class Server
    {
        private TcpListener ConnectionListenter;
        private Thread ListenerThread;
        List<TcpClient> Clients = new List<TcpClient>();

        //Simple constructor that creates a listener then assigns that listener to a thread
        public Server(IPAddress Address, int Port)
        {
            this.ConnectionListenter = new TcpListener(Address, Port);
            this.ListenerThread = new Thread(new ThreadStart(ListenForClients));
            this.ListenerThread.Start();
        }
        
        private void ListenForClients()
        {
            this.ConnectionListenter.Start();

            while(true)
            {
                TcpClient Client = this.ConnectionListenter.AcceptTcpClient();

                Clients.Add(Client);
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleMessage));
            }
        }
        private void HandleMessage(object client)
        {
            TcpClient Client = (TcpClient)client;
            NetworkStream Stream = Client.GetStream();

            byte[] message = new byte[4096];
            int BytesRead;

            while(true)
            {
                BytesRead = 0;

                try
                {
                    BytesRead = Stream.Read(message, 0, 4096);
                }
                catch { break; } //Socket error

                if(BytesRead == 0) //Disconnect
                {
                    Clients.Remove(Client);
                    break;
                }

                //Deserialize message here
            }
        }

    }
}
