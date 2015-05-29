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

            //Debug
            ServerPort = 3000;//Port I'm listening to

            Console.WriteLine("OpenCards Testing Server\n");

            MyServer = new Network.Server(ServerPort);


            while (true)
            {
                Console.ReadLine();//Something is screwy.

                Network.Message tempMessage = MyServer.GetNextMessage();
                if (tempMessage != null)
                {
                    Console.WriteLine(tempMessage.Sender);
                }

            }
        }
    }
}
