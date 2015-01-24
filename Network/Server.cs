using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

namespace Network
{
    public class Server
    {
        private TcpListener ConnectionListenter;
        private Thread ListenerThread;
        private List<Object> Messages = new List<Object>();
        private List<TcpClient> Clients = new List<TcpClient>();
        private List<TcpClient> LiveClients = new List<TcpClient>();

        //Simple constructor that creates a listener then assigns that listener to a thread
        public Server(IPAddress Address, int Port)
        {
            this.ConnectionListenter = new TcpListener(Address, Port);
            this.ListenerThread = new Thread(new ThreadStart(ListenForClients));
            this.ListenerThread.Start();
        }
        
        // Start listening for clients
        // Is invoked in a thread by the constructor
        private void ListenForClients()
        {
            this.ConnectionListenter.Start();

            while(true)
            {
                TcpClient Client = this.ConnectionListenter.AcceptTcpClient();
                Clients.Add(Client);
                Thread ClientThread = new Thread(new ParameterizedThreadStart(ClientListener));
            }
        }

        // Read the message from a connected client and adds it to the message list
        // Messages are collected with GetMessage()
        private void ClientListener(object client)
        {
            TcpClient Client = (TcpClient)client;
            NetworkStream NetStream = Client.GetStream();
            int BytesRead;
            byte[] Message = new byte[2048];

            while (true)
            {
                BytesRead = 0;

                try
                {
                    BytesRead = NetStream.Read(Message, 0, 2048);
                }
                catch
                {
                    break; // Socket error
                }

                if (BytesRead == 0)
                {
                    break; // Client disconnection
                }
            }

            Client.Close();
        }

        // Send a message to the given client
        public void sendMessage(Object Message, TcpClient Client)
        {
        }

        // Get the most recent message
        public Object GetMessage()
        {
            if (Messages.Count != 0)
            {
                Object Message = Messages[0];
                Messages.RemoveAt(0);
                return Message;
            }
            else
            {
                return null;
            }
        }

        // Get the most recent connection
        // Should be invoked whenever a player connection is found
        public TcpClient GetConnectedPlayer()
        {
            if (Clients.Count != 0)
            {
                TcpClient Client = Clients[0];
                Clients.RemoveAt(0);
                return Client;
            }
            else
            {
                return null;
            }
        }

        // Deserialize a message
        private void DeserializeMessage(Byte[] Message)
        {
        }
    }
}
