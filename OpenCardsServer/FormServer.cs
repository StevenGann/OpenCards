using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Network;
using OpenCards;
using System.Net;

namespace OpenCardsServer
{
    public partial class FormServer : Form
    {
        public Server server;
        public IPAddress IP;
        public int Port = 0;

        public List<Player> ConnectedPlayers;
        public List<Player> ActivePlayers;
        public int MaxRounds = 10;
        public int Round = 0;

        public FormServer()
        {
            InitializeComponent();

            IP = IPAddress.Parse("localhost");
            server = new Server(Port);

            GameLoop();
        }

        public void GameLoop()
        {
            while (true)
            {
                //Start Game
                //Load decks

                while (Round <= MaxRounds)
                {
                    //Start Round
                    //Check for connected players

                    //Add all ConnectedPlayers to ActivePlayers

                    //Send every player a status

                    //Deal for hand of white cards

                    //Wait for all Responses or timer

                    //Update Status with Responses

                    //Wait for Czar selection

                    //Update Status with new scores

                    //End Round
                    Round += 1;
                    //Start new Round
                }
                Round = 0;
            }

            //End game
            return;
        }
        
    }
}

/*
Server:
Run constructor(IPAddress Address, int port)
loop:
 GetCommectedPlayer() //For the most recently connected player
 GetMessage() //For the most recently received message
 //Both return null if there are none of either
EndLoop
 * 
Every Now and Then:
 IsConnected(List<TcpClient>) //Ping everyone. 
 //Returns a list of connected players after ~350ms

Client:
Constructor(IPAddress, Int port)
Loop:
GetMessage()
/Loop
 * 
Run in this order:
Ping() // Pings the server
if(IsPinging()) //Active ping state
IsConnected() //Checks the result of the most recently
 //completed Ping()

IsPinging() basically just tracks whether it is currently 
 * pinging or not. To track if it is responding or not.
*/