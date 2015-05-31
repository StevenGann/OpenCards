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

            //Actual game stuff
            Deck deck = new Deck();//Current deck to deal from
            List<Player> Players = new List<Player>();//List of players
            int round = 0;
            bool dealt = false; //Have I dealt for this round?
            int responsesReceived = 0; //Responses received this round.

            //Debug
            ServerPort = 3000;//Port I'm listening to

            Console.WriteLine("OpenCards Testing Server\n");

            MyServer = new Network.Server(ServerPort);


            while (true)//Main game loop
            {
                #region //Check for and deal with messages
                Network.Message tempMessage = MyServer.GetNextMessage();
                if (tempMessage != null)
                {
                    //Console.WriteLine(tempMessage.Sender);
                    if (tempMessage.PayloadType == typeof(Deck))
                    {
                        Deck tempDeck = (Deck)tempMessage.Payload;
                        deck.Add(tempDeck);
                        Console.WriteLine("Got a deck from " + tempDeck.Sender.Name + " at " + tempDeck.Sender.IP);
                    }
                    else if (tempMessage.PayloadType == typeof(Player))
                    {
                        Player tempPlayer = (Player)tempMessage.Payload;
                        Players.Add(tempPlayer);
                        //Console.WriteLine(tempPlayer.ToString());
                    }
                    else if (tempMessage.PayloadType == typeof(Response))
                    {
                        Response tempResponse = (Response)tempMessage.Payload;
                        Console.WriteLine(tempResponse.ToString());
                    }
                }
                #endregion

                //If haven't dealt, deal

                //If all responses have come in, send responses to all players

                //If last Response came from Czar and all responses have been received, notify all players of the round winner, send Gamestate to all players
            }




            //End of Main
        }


        //End of class
    }
}


/*
//Console.WriteLine("Sending a response\n");
            Response tempResponse = new Response();
            tempResponse.Add(new Card("Response test card #1"));
            tempResponse.Add(new Card("Response test card #2"));
            //MyClient.SendMessage(tempResponse);
            Response tempResponse2 = new Response();
            tempResponse2.Add(new Card("Response test card A"));
            tempResponse2.Add(new Card("Response test card B"));

            Player tempPlayer1 = new Player("Skylark");
            tempPlayer1.Score = 10;
            tempPlayer1.Status = "Czar";

            Player tempPlayer2 = new Player("Cad'ika");
            tempPlayer2.Score = 8;

            Player tempPlayer3 = new Player("Jacen");
            tempPlayer3.Score = 13;

            GameStatus tempStatus = new GameStatus();
            tempStatus.currentBlack = new BlackCard("Black ______ #1", 1);
            tempStatus.Responses.Add(tempResponse);
            tempStatus.Responses.Add(tempResponse2);
            tempStatus.Players.Add(tempPlayer1);
            tempStatus.Players.Add(tempPlayer2);
            tempStatus.Players.Add(tempPlayer3);

            MyClient.SendMessage(tempStatus);
*/