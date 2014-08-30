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
        List<Object> Messages = new List<Object>();

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
                Thread ClientThread = new Thread(new ParameterizedThreadStart(GetMessage));
            }
        }

        private void GetMessage(object client)
        {
            TcpClient Client = (TcpClient)client;
            NetworkStream NetStream = Client.GetStream();
            int DataRead = 0;

            // Read four byte int of the size of the full message
            byte[] IncMessLength = new byte[4];
            NetStream.Read(IncMessLength, 0, 4); 

            // Read the message
            int MsgLength = BitConverter.ToInt32(IncMessLength, 0);
            Byte[] IncMessage = new byte[MsgLength];
            // This reads until it gets the whole message if it arrives in pieces
            do {
                DataRead += NetStream.Read(IncMessage, 0, MsgLength - DataRead);
            }while(DataRead < MsgLength);
            
            //Deserialize message
            MemoryStream MemStream = new MemoryStream();
            BinaryFormatter BinForm = new BinaryFormatter();
            MemStream.Write(IncMessage, 0, IncMessage.Length);
            Object Message = (Object) BinForm.Deserialize(MemStream);
            Messages.Add(Message);
            
        }

    }
}
