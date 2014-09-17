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
    class Server
    {
        private TcpListener ConnectionListenter;
        private Thread ListenerThread;
        private List<Object> Messages = new List<Object>();
        private List<TcpClient> Clients = new List<TcpClient>();
        private List<TcpClient> LiveClients = new List<TcpClient>();
        private Boolean Pinging = false;

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
                else if (BytesRead == 4)
                {
                    if(Pinging)
                    {
                        LiveClients.Add(Client);
                    }
                    else
                    {
                        Ping((TcpClient)client);
                    }
                }

                DeserializeMessage(Message);
            }

            Client.Close();
        }

        // Send a message to the given client
        public void sendMessage(Object Message, TcpClient Client)
        {
            NetworkStream NetStream = new NetworkStream(Client.Client);
            
            BinaryFormatter BinForm = new BinaryFormatter();
            MemoryStream MemStream = new MemoryStream();
            BinForm.Serialize(MemStream, Message);
            byte[] OutMessageObj = MemStream.ToArray();
            byte[] OutMessageLength = BitConverter.GetBytes(OutMessageObj.Length);
            byte[] OutMessage = new byte[OutMessageObj.Length + OutMessageLength.Length];
            OutMessageLength.CopyTo(OutMessage, 0);
            OutMessageObj.CopyTo(OutMessage, OutMessageLength.Length);
            NetStream.Write(OutMessage, 0, OutMessage.Length);
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

        // Check if all players are still connected and remove them if not
        public List<TcpClient> IsConnected(List<TcpClient> Clients)
        {
            Pinging = true;
            for (int i = 0; i <= Clients.Count; i++)
            {
                Ping(Clients[i]);
            }
            Thread.Sleep(350);
            Pinging = false;
            return LiveClients;
        }

        //Ping a client
        private void Ping(TcpClient Client)
        {
            NetworkStream NetStream = new NetworkStream(Client.Client);

            byte[] Ping = BitConverter.GetBytes(-1);
            NetStream.Write(Ping, 0, Ping.Length);
        }

        public Boolean IsPinging()
        {
            return Pinging;
        }

        // Deserialize a message
        private void DeserializeMessage(Byte[] Message)
        {
            int MessageLength = BitConverter.ToInt32(Message, 0);

            if (MessageLength == -1)
            {
                return;
            }
            else // This shouldn't happen and is just error catching
            {
                LiveClients.RemoveAt(0);
            }

            //Deserialize message
            MemoryStream MemStream = new MemoryStream();
            BinaryFormatter BinForm = new BinaryFormatter();
            MemStream.Write(Message, 4, MessageLength);
            Object MessageObj = (Object)BinForm.Deserialize(MemStream);
            Messages.Add(MessageObj);
        }
    }
}
