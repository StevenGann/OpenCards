using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using OpenCards;

namespace TestingClient
{
    class Program
    {
        static void Main(string[] args)
        {
            int ServerPort = -1;
            int ClientPort = -1;
            IPAddress ServerIP = null;
            Network.Client MyClient;
            Network.Server MyServer;

            //Debug
            ServerIP = IPAddress.Parse("127.0.0.1");
            ServerPort = 3000;//Port I'm sending to
            ClientPort = 2001;//Port I'm listening
            
            Console.WriteLine("OpenCards Testing Client\n");

            while (ServerIP == null)
            {
                Console.Write("Please enter a the server IP: ");
                try
                {
                    ServerIP = IPAddress.Parse(Console.ReadLine());
                    break;
                }
                catch
                {
                    Console.WriteLine();
                    Console.WriteLine("ERROR. Failed to parse IP.");
                }
            }

            while (ServerPort <= 1 || ServerPort >= 49151)
            {
                Console.Write("Please enter the server's port number: ");
                try
                {
                    ServerPort = Convert.ToInt16(Console.ReadLine());
                    if (ServerPort > 1 && ServerPort < 49151) { break; }
                }
                catch { }

                Console.WriteLine();
                Console.WriteLine("ERROR. Invalid port number.");

            }

            while (ClientPort <= 1 || ClientPort >= 49151)
            {
                Console.Write("Please enter a forwarded port number: ");
                try
                {
                    ClientPort = Convert.ToInt16(Console.ReadLine());
                    if (ClientPort > 1 && ClientPort < 49151) { break; }
                }
                catch { }

                Console.WriteLine();
                Console.WriteLine("ERROR. Invalid port number.");

            }



            Console.Write("Connecting to server at ");
            Console.Write(ServerIP);
            Console.Write(":");
            Console.WriteLine(ServerPort + "\n");
            

            MyClient = new Network.Client(ServerPort);//Port I'm sending to
            MyClient.ServerIP = ServerIP.ToString();
            MyClient.ExternalPort = ClientPort;//Port I'm listening to
            MyServer = new Network.Server(ClientPort);//Port I'm listening to

            Console.WriteLine("Sending greeting\n");
            MyClient.SendMessage(MyClient);

            Console.WriteLine("Sending 2 cards\n");
            MyClient.SendMessage(new Card("Test card #1"));
            MyClient.SendMessage(new Card("Test card #2"));

            Console.WriteLine("Sending 2 black cards\n");
            MyClient.SendMessage(new BlackCard("Black ______ #1", 1));
            MyClient.SendMessage(new BlackCard("______ ______ #2", 2));

            Console.WriteLine("Sending a deck\n");
            Deck tempDeck = new Deck("TestingClient Deck");
            tempDeck.Add(new Card("Test deck card #1"));
            tempDeck.Add(new Card("Test deck card #2"));
            tempDeck.Add(new Card("Test deck card #3"));
            tempDeck.Add(new BlackCard("Testing a ______ card for _____.",2));
            MyClient.SendMessage(tempDeck);

            Console.WriteLine("Sending a response\n");
            Response tempResponse = new Response();
            tempResponse.Add(new Card("Response test card #1"));
            tempResponse.Add(new Card("Response test card #2"));
            MyClient.SendMessage(tempResponse);


            Console.ReadLine();
        }
    }
}
