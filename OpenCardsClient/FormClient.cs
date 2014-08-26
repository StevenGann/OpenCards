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
using System.Net;

namespace OpenCardsClient
{
    public partial class FormClient : Form
    {
        //====================================================================================
        //Fields
        //------------------------------------------------------------------------------------
        public List<Card> Hand = new List<Card>();
        public List<Response> Responses = new List<Response>();
        public List<Card> SelectedCards = new List<Card>();
        public GameStatus status = new GameStatus();
        public bool IsCzar = false;

        public IPAddress serverIP;
        public int serverPort = 0;

        //Regulation playing cards are 56mm x 88mm
        //Keep to that ratio.
        int CardWidth = 168;
        int CardHeight = 264;

        //====================================================================================
        //Form Constructor
        //------------------------------------------------------------------------------------
        public FormClient()
        {
            InitializeComponent();

            toolStripComboBoxCardSize.SelectedIndex = 1;

            // Hack to fix the ugly selection lines on the splitters
            menuStrip1.Select();





            //Debug code to let me test the GUI without having all the network code done.
            //This is all dummy data for testing rendering.
            //This data will eventually come from the server.
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
            Hand.Add(new Card("Test Card #5. \nCard size is adjustable under Options. Please test on high and low DPI conditions."));
            Hand.Add(new Card("Test Card #6"));
            Hand.Add(new Card("Test Card #7 with abnormally long text to test the Card.Render() method. I hope this works because it seems unlikely that any card is going to be longer than this, but the program should be ready for this. This message is finally too long!"));

            Response newResponse = new Response();
            newResponse.Add(new Card("Giant Head"));
            newResponse.Add(new Card("Aspirin"));
            Responses.Add(newResponse);
            newResponse = new Response();
            newResponse.Add(new Card("Aunt"));
            newResponse.Add(new Card("Humility"));
            Responses.Add(newResponse);
            newResponse = new Response();
            newResponse.Add(new Card("Professor"));
            newResponse.Add(new Card("Acid"));
            Responses.Add(newResponse);
            newResponse = new Response();
            newResponse.Add(new Card("PC"));
            newResponse.Add(new Card("GabeN's Love"));
            Responses.Add(newResponse);

            RenderHand();
            RenderBlackCard();
            RenderResponses();
        }

        private void FormClient_Load(object sender, EventArgs e)
        {
            Application.EnableVisualStyles();
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
            RenderBlackCard();
            RenderResponses();
        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (this.splitContainer1.CanFocus)
            {
                this.splitContainer1.ActiveControl = this.buttonPlay;
            }

            RenderHand();
            RenderBlackCard();
            RenderResponses();
        }

        private void splitContainer3_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (this.splitContainer1.CanFocus)
            {
                this.splitContainer1.ActiveControl = this.buttonPlay;
            }

            RenderHand();
            RenderBlackCard();
            RenderResponses();
        }
        //------------------------------------------------------------------------------------

        private void FormClient_Resize(object sender, EventArgs e)
        {
            RenderHand();
            RenderBlackCard();
            RenderResponses();
        }

        //Play Cards button
        private void buttonPlay_Click(object sender, EventArgs e)
        {
            //Pack selected Cards into a new Response
            //Send Response to Server
            //Await updated GameStatus
        }

        //Main Timer Tick
        //This should function as a heartbeat, and to query the server for an update
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Temporary debug code:
            //RenderHand();
            //RenderResponses();
        }

        private void splitContainer3_Panel2_MouseWheel(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("Woop");
        }

        private void splitContainer3_Panel2_MouseEnter(object sender, EventArgs e)
        {
            //MessageBox.Show("Entered");
            if (!splitContainer3.Panel2.Focused)
            {
                splitContainer3.Panel2.Focus();
            }
        }

        private void splitContainer3_Panel2_MouseLeave(object sender, EventArgs e)
        {
            //MessageBox.Show("Left");
            if (splitContainer3.Panel2.Focused)
            {
                splitContainer3.Focus();
            }
        }

        private void splitContainer2_Panel2_MouseEnter(object sender, EventArgs e)
        {
            if (!splitContainer2.Panel2.Focused)
            {
                splitContainer2.Panel2.Focus();
            }
        }

        private void splitContainer2_Panel2_MouseLeave(object sender, EventArgs e)
        {
            if (splitContainer2.Panel2.Focused)
            {
                splitContainer2.Focus();
            }
        }

        private void card_Click(object sender, EventArgs e)
        {
            
            
            PictureBox senderPB = (PictureBox)sender;
            Card clickedCard = Hand[Convert.ToInt32(senderPB.Name)];
            //MessageBox.Show(Hand[Convert.ToInt32(senderPB.Name)].Text);

            if (clickedCard.Selection > 0)
            {
                clickedCard.Selection = 0;
                SelectedCards.Remove(clickedCard);

                //Iterate through SelectedCards and set their Selection to the appropriate value
                int index = 0;
                foreach (Card card in SelectedCards)
                {
                    card.Selection = index + 1;
                    index++;
                }
            }
            else
            {
                if (SelectedCards.Count < status.currentBlack.Blanks)
                {
                    SelectedCards.Add(clickedCard);
                    clickedCard.Selection = SelectedCards.Count;
                }
            }
            toolStripStatusLabel1.Text = "SelectedCards.Count = " + Convert.ToString(SelectedCards.Count);
            RenderHand();
            
        }

        private void connectToServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string rawIP = Microsoft.VisualBasic.Interaction.InputBox("Input the server IP", "Connect to Server", "");
            try
            {
                serverIP = IPAddress.Parse(rawIP);
                serverPort = int.Parse(Microsoft.VisualBasic.Interaction.InputBox("Input server port", "Connect to Server", serverPort.ToString()));
                MessageBox.Show("IP: " + serverIP.ToString() + ":" + serverPort.ToString());
            }
            catch 
            {
                if (rawIP == "")
                {
                    MessageBox.Show("ERROR: Empty IP");
                }
                else
                {
                    MessageBox.Show("ERROR: Failed to parse IP");
                }
            }
        }

        private void customToolStripMenuItem_Click(object sender, EventArgs e)
        {
            float scalar = 1.0f;
            try
            {
                scalar = float.Parse(Microsoft.VisualBasic.Interaction.InputBox("Input scale value", "Custom Card Scale", "1.0"));
            }
            catch
            {
                MessageBox.Show("ERROR: Failed to parse scalar value");
            }


            CardWidth = (int)(168.0f * scalar);
            CardHeight = (int)(264.0f * scalar);

            RenderHand();
            RenderBlackCard();
            RenderResponses();
        }

        private void toolStripComboBoxCardSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBoxCardSize.SelectedIndex == 0) //Small
            {
                CardWidth = (int)(168.0f / 1.5f); //this seems like the smallest you can make it and still read text.
                CardHeight = (int)(264.0f / 1.5f);
            }
            if (toolStripComboBoxCardSize.SelectedIndex == 1) //Medium (Default)
            {
                CardWidth = 168;
                CardHeight = 264;
            }
            if (toolStripComboBoxCardSize.SelectedIndex == 2) //Large
            {
                CardWidth = (int)(168.0f * 1.5f);
                CardHeight = (int)(264.0f * 1.5f);
            }

            RenderHand();
            RenderBlackCard();
            RenderResponses();
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
            

            int index = 0;

            foreach (Card card in Hand) //OMG It's like I'm writing plain English!
            {
                PictureBox pb = card.Render(CardWidth, CardHeight);
                handPictures.Add(pb);
            }

            splitContainer3.Panel2.Controls.Clear(); //Clear out the PictureBoxes from the last RenderHand() call.

            index = 0;
            
            foreach (PictureBox pb in handPictures)
            {
                splitContainer3.Panel2.Controls.Add(pb);                            //Put Picturebox in the Hand section of the GUI
                pb.Location = new System.Drawing.Point(newX, newY);                 //Move cards so they don't overlap.
                pb.Name = Convert.ToString(index);

                index++;

                newX += (CardWidth + marginH);

                if ((newX + CardWidth + marginH) >= splitContainer3.Panel2.Width)
                {
                    newX = marginH;
                    newY += (CardHeight + marginV);
                }

                pb.Click += new System.EventHandler(this.card_Click);
            }
        }

        private void RenderBlackCard()
        {
            int marginH = 4;    //ToDo: Replace these magic numbers with something adjustable.
            int marginV = 4;
            int newX = marginH;
            int newY = marginV;

            //Attempted to make cards dynamically scale with the size of the panel they're in.
            //It looks awful and doesn't work.
            /*
            float magicRatio = (float)53 / (float)88;
            float panelRatio = (float)splitContainer2.Panel1.Width / (float)splitContainer2.Panel1.Height;

            if (panelRatio >= magicRatio)//Panel is WIDER than BlackCard, snap to Height
            {
                height = splitContainer2.Panel1.Height;

                //mR = w/h
                //w = mR*h
                width = (int)(magicRatio * (float)height) - 50;
            }

            if (panelRatio < magicRatio)//Panel is TALLER thatn BlackCard, snap to Width
            {
                width = splitContainer2.Panel1.Width;

                //mR = w/h
                //h = w/mR
                height = (int)((float)width / magicRatio) - 50;
            }*/

            PictureBox pb = status.currentBlack.Render(CardWidth, CardHeight);
            
            splitContainer2.Panel1.Controls.Clear();

            splitContainer2.Panel1.Controls.Add(pb);

            pb.Location = new System.Drawing.Point(newX, newY);
        }

        private void RenderResponses()
        {
            List<Panel> responsePanels = new List<Panel>();

            int marginH = 8;    //ToDo: Replace these magic numbers with something adjustable.
            int marginV = 4;
            int newX = marginH;
            int newY = marginV;

            foreach (Response response in Responses)
            {
                responsePanels.Add(response.Render(CardWidth, CardHeight));
            }

            splitContainer2.Panel2.Controls.Clear();

            foreach (Panel panel in responsePanels)
            {
                splitContainer2.Panel2.Controls.Add(panel);

                panel.Location = new System.Drawing.Point(newX, newY);                 //Move cards so they don't overlap.

                newX += (panel.Width + marginH);

                if ((newX + panel.Width + marginH) >= splitContainer2.Panel2.Width)
                {
                    newX = marginH;
                    newY += (panel.Height + marginV);
                }
            }

        }

        

        
        

        

        

        

        

        

        

        

        //------------------------------------------------------------------------------------
    }
}
