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
    class Client
    {
        private List<Object> Messages = new List<Object>();
        private TcpClient Server;
        private Thread ListenerThread;
        private Boolean ConnectionState;
        private Boolean Pinging = false;

        // Constructor to start the message listener in its own thread
        public Client(IPAddress Address, int Port)
        {
            this.Server = new TcpClient(Address.ToString(), Port);
            this.ListenerThread = new Thread(new ThreadStart(MessageListener));
            this.ListenerThread.Start();
        }

        // Read the message from a connected client and adds it to the message list
        // Messages are collected with GetMessage()
        private void MessageListener()
        {
            NetworkStream NetStream = Server.GetStream();
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
                    if (Pinging)
                    {
                        ConnectionState = true;
                    }
                    else
                    {
                        Ping();
                    }
                }

                DeserializeMessage(Message);
            }
            ConnectionState = false;
            Server.Close();
        }

        // Send a message to the given client
        public void sendMessage(Object Message)
        {
            NetworkStream NetStream = new NetworkStream(Server.Client);

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

        // Deserialize a message
        private void DeserializeMessage(Byte[] Message)
        {
            int MessageLength = BitConverter.ToInt32(Message, 0);

            if (MessageLength == -1)
            {
                ConnectionState = true;
                return;
            }

            //Deserialize message
            MemoryStream MemStream = new MemoryStream();
            BinaryFormatter BinForm = new BinaryFormatter();
            MemStream.Write(Message, 4, MessageLength);
            Object MessageObj = (Object)BinForm.Deserialize(MemStream);
            Messages.Add(MessageObj);
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

        //Ping the server
        private Boolean Ping()
        {
            NetworkStream NetStream = new NetworkStream(Server.Client);
            Pinging = true;

            byte[] Ping = BitConverter.GetBytes(-1);
            NetStream.Write(Ping, 0, Ping.Length);
            Thread.Sleep(300);

            Pinging = false;
            return IsConnected();
        }

        public Boolean IsPinging()
        {
            return Pinging;
        }

        // Check connection status
        public Boolean IsConnected()
        {
            return ConnectionState;
        }
    }
}
