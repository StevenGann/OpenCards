using OpenCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestingServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int ServerPort = -1;
            Network.Server MyServer;
            Deck deck = new Deck();

            //Debug
            ServerPort = 3000;//Port I'm listening to

            Console.WriteLine("OpenCards Testing Server\n");

            MyServer = new Network.Server(ServerPort);


            while (true)
            {
                //Console.ReadLine();

                Network.Message tempMessage = MyServer.GetNextMessage();
                if (tempMessage != null)
                {
                    Console.WriteLine(tempMessage.Sender);
                    if (tempMessage.PayloadType == typeof(Card))
                    {
                        deck.Add((Card)tempMessage.Payload);
                        Console.WriteLine(deck.ToString());
                    }
                    else if (tempMessage.PayloadType == typeof(Deck))
                    {
                        deck.Add((Deck)tempMessage.Payload);
                        Console.WriteLine(deck.ToString());
                    }
                    else if (tempMessage.PayloadType == typeof(BlackCard))
                    {
                        deck.Add((BlackCard)tempMessage.Payload);
                        Console.WriteLine(deck.ToString());
                    }
                    else if (tempMessage.PayloadType == typeof(GameStatus))
                    {
                    }
                    else if (tempMessage.PayloadType == typeof(Player))
                    {
                    }
                    else if (tempMessage.PayloadType == typeof(Response))
                    {
                        Response tempResponse = (Response)tempMessage.Payload;
                        Console.WriteLine(tempResponse.ToString());
                    }
                }

            }
        }
    }
}
