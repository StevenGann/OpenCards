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
    public class Client
    {
        private List<Object> Messages = new List<Object>();
        private TcpClient Server;
        private Thread ListenerThread;

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
            }
            Server.Close();
        }

        // Send a message to the given client
        public void sendMessage(Object Message)
        {
        }

        // Deserialize a message
        private void DeserializeMessage(Byte[] Message)
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
    }
}
