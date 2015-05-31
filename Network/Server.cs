using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using OpenCards;

namespace Network
{
    public class Server
    {
        private TcpListener tcpListener;
        private Thread listenThread;
        private int port;

        private Queue<Message> messageQueue = new Queue<Message>();

        public Server(int serverport)
        {
            port = serverport; ;
            this.tcpListener = new TcpListener(IPAddress.Any, port);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
        }

        private void ListenForClients()
        {
            this.tcpListener.Start();
            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient client = this.tcpListener.AcceptTcpClient();
                
                //create a thread to handle communication 
                //with connected client
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
            }
        }
        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();
            
            byte[] message = new byte[4096];
            int bytesRead;
            String str = "";
            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                    for (int i = 0; i < message.Length; i++)
                    {
                        if (message[i]!=0)
                        {
                            str += Convert.ToChar(message[i]);
                        }
                    }

                }
                catch
                {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }

                

                //message has successfully been received
                ASCIIEncoding encoder = new ASCIIEncoding();
                String newMessage = encoder.GetString(message, 0, bytesRead);
                String sender = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();
                sender += ":" + ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Port.ToString();

                Message tempMessage = DeserializeMessageXML(newMessage, sender);
                messageQueue.Enqueue(tempMessage);

                if (tempMessage.Sender == "")
                {
                    Console.WriteLine("Got a bad message");
                    Console.WriteLine(newMessage+"\n\n");
                }
                else
                {
                    Console.WriteLine("Got a message");
                }
            }

            tcpClient.Close();
        }
        
        public Message GetNextMessage()
        {
            try
            {
                return messageQueue.Dequeue();
            }
            catch
            {
                return null;
            }
        }

        public int GetMessageCount()
        {
            return messageQueue.Count;
        }

        public static Message DeserializeMessageXML(String message, String sender)
        {
            
            Message result = new Message();

            try
            {
                var stringReader = new StringReader(message);
                var serializer = new XmlSerializer(typeof(Card));
                Card payload = serializer.Deserialize(stringReader) as Card;
                result.Payload = payload;
                result.PayloadType = typeof(Card);
                result.Sender = sender;
                return result;

            }
            catch { }

            try
            {
                var stringReader = new StringReader(message);
                var serializer = new XmlSerializer(typeof(BlackCard));
                BlackCard payload = serializer.Deserialize(stringReader) as BlackCard;
                result.Payload = payload;
                result.PayloadType = typeof(BlackCard);
                result.Sender = sender;
                return result;

            }
            catch { }

            try
            {
                var stringReader = new StringReader(message);
                var serializer = new XmlSerializer(typeof(Deck));
                Deck payload = serializer.Deserialize(stringReader) as Deck;
                result.Payload = payload;
                result.PayloadType = typeof(Deck);
                result.Sender = sender;
                return result;

            }
            catch { }

            try
            {
                var stringReader = new StringReader(message);
                var serializer = new XmlSerializer(typeof(Response));
                Response payload = serializer.Deserialize(stringReader) as Response;
                result.Payload = payload;
                result.PayloadType = typeof(Response);
                result.Sender = sender;
                return result;

            }
            catch { }

            try
            {
                var stringReader = new StringReader(message);
                var serializer = new XmlSerializer(typeof(Player));
                Player payload = serializer.Deserialize(stringReader) as Player;
                result.Payload = payload;
                result.PayloadType = typeof(Player);
                result.Sender = sender;
                return result;

            }
            catch { }

            try
            {
                var stringReader = new StringReader(message);
                var serializer = new XmlSerializer(typeof(GameStatus));
                GameStatus payload = serializer.Deserialize(stringReader) as GameStatus;
                result.Payload = payload;
                result.PayloadType = typeof(GameStatus);
                result.Sender = sender;
                return result;

            }
            catch { }

            return result;
        }
    }
}
