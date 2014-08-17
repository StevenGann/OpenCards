using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCards;

namespace OpenCardsClient
{
    public partial class FormClient : Form
    {
        //====================================================================================
        //Fields
        //------------------------------------------------------------------------------------
        public List<Card> Hand = new List<Card>();
        public List<Response> Responses = new List<Response>();
        public GameStatus status = new GameStatus();
        public bool IsCzar = false;

        //====================================================================================
        //Form Constructor
        //------------------------------------------------------------------------------------
        public FormClient()
        {
            InitializeComponent();

            menuStrip1.Select();

            //Debug code to let me test the GUI without having all the network code done.
            Random RNG = new Random();
            /*
            public List<Player> Players = new List<Player>();
            public List<Response> Responses = new List<Response>();
            public bool GameOver = false;
            */
            status.currentBlack = new BlackCard("My _____ needs a heavy dose of _____.", 2);

            dataGridPlayerList.Rows.Add("Cadika", RNG.Next(15), "CZAR");
            dataGridPlayerList.Rows.Add("Skylark", RNG.Next(15), "");
            dataGridPlayerList.Rows.Add("Conrad", RNG.Next(15), "");
            dataGridPlayerList.Rows.Add("Damen", RNG.Next(15), "");
            dataGridPlayerList.Rows.Add("Jacen", RNG.Next(15), "");
            
            Hand.Add(new Card("Test Card #1"));
            Hand.Add(new Card("Test Card #2. \nCard #1 had little text."));
            Hand.Add(new Card("Test Card #3. \nThese cards need to test several different lengths of cards."));
            Hand.Add(new Card("Test Card #4. \nSupport for scaling cards with window size is still in the works."));
            Hand.Add(new Card("Test Card #5. \nFor now, card size is hardcoded, so it must suck to have a high-DPI display."));
            Hand.Add(new Card("Test Card #6"));
            Hand.Add(new Card("Test Card #7 with abnormally long text to test the Card.Render() method. I hope this works because it seems unlikely that any card is going to be longer than this, but the program should be ready for this. This message is finally too long!"));

            RenderHand();
            RenderBlackCard();
        }

        //====================================================================================
        //Event Handlers
        //------------------------------------------------------------------------------------
        // Hack to fix the ugly selection lines on the splitters
        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (this.splitContainer1.CanFocus)
            {
                this.splitContainer1.ActiveControl = this.buttonPlay;
            }

            RenderHand();
        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (this.splitContainer1.CanFocus)
            {
                this.splitContainer1.ActiveControl = this.buttonPlay;
            }

            RenderHand();
        }

        private void splitContainer3_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (this.splitContainer1.CanFocus)
            {
                this.splitContainer1.ActiveControl = this.buttonPlay;
            }

            RenderHand();
        }
        //------------------------------------------------------------------------------------

        //Play Cards button
        private void buttonPlay_Click(object sender, EventArgs e)
        {
            //Pack selected Cards into a new Response
            //Send Response to Server
            //Await updated GameStatus
        }

        //Main Timer Tick
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Temporary debug code:
            //RenderHand();
        }

        //====================================================================================
        //GUI Logic Methods
        //------------------------------------------------------------------------------------

        private void RenderHand()
        {
            List<PictureBox> handPictures = new List<PictureBox>();

            int marginH = 4;    //ToDo: Replace these magic numbers with something adjustable.
            int marginV = 4;
            int newX = marginH;
            int newY = marginV;
            int width = 168;
            int height = 264;

            foreach (Card card in Hand)
            {
                PictureBox pb = card.Render(width, height);
                
                handPictures.Add(pb);
                //Not sure if I really need to set these?
                //pb.Name = "";
                //pb.TabIndex = 0;
                //pb.TabStop = false;
            }

            splitContainer3.Panel2.Controls.Clear(); //Clear out the PictureBoxes from the last RenderHand() call.

            foreach (PictureBox pb in handPictures)
            {
                splitContainer3.Panel2.Controls.Add(pb);                            //Put Picturebox in the Hand section of the GUI
                pb.Location = new System.Drawing.Point(newX, newY);                 //Move cards so they don't overlap.

                newX += (width + marginH);

                if ((newX + width + marginH) >= splitContainer3.Panel2.Width)
                {
                    newX = marginH;
                    newY += (height + marginV);
                }

                
            }
        }

        private void RenderBlackCard()
        {
            int marginH = 4;    //ToDo: Replace these magic numbers with something adjustable.
            int marginV = 4;
            int newX = marginH;
            int newY = marginV;
            int width = 168;
            int height = 264;

            PictureBox pb = status.currentBlack.Render(width, height);
            
            splitContainer2.Panel1.Controls.Clear();

            splitContainer2.Panel1.Controls.Add(pb);

            pb.Location = new System.Drawing.Point(newX, newY);
        }

        //------------------------------------------------------------------------------------
    }
}
