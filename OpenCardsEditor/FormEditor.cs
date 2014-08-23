using OpenCards;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OpenCardsEditor
{
    public partial class FormEditor : Form
    {
        Deck currentDeck = new Deck("untitled");
        String savePath = "";
        

        public FormEditor()
        {
            InitializeComponent();
        }

        private void FormEditor_Load(object sender, EventArgs e)
        {
            Card newCard = new Card("Test deck card #1.");
            currentDeck.Add(newCard);
            
        }

        private void saveDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SaveDeckXML(currentDeck, currentDeck.Title + ".dek", "C:\\");

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Deck files (*.dek)|*.dek|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SaveDeckXML(currentDeck, saveFileDialog1.FileName);
                savePath = saveFileDialog1.FileName;
            }
        }

        private void FormEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)       // Ctrl-S Save
            {
                // Bring up save dialog if file hasn't been saved before.
                // Otherwise save to previous location

                if (savePath == "")
                {
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                    saveFileDialog1.Filter = "Deck files (*.dek)|*.dek|All files (*.*)|*.*";
                    saveFileDialog1.FilterIndex = 1;
                    saveFileDialog1.RestoreDirectory = true;

                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        SaveDeckXML(currentDeck, saveFileDialog1.FileName);
                        savePath = saveFileDialog1.FileName;
                    }
                }
                else 
                {
                    SaveDeckXML(currentDeck, savePath);
                }

                e.SuppressKeyPress = true;
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            currentDeck = new Deck(textBoxTitle.Text);
            UpdateWhiteDeck(); //To Do: move this work to a BackgroundWorker
            UpdateBlackDeck();
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            currentDeck = new Deck(textBoxTitle.Text);
            UpdateBlackDeck(); //To Do: move this work to a BackgroundWorker
            UpdateWhiteDeck();
        }

        private void loadDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "C:\\";
            openFileDialog1.Filter = "Deck files (*.dek)|*.dek|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    currentDeck = LoadDeckXML(openFileDialog1.FileName);
                    UpdateWhiteGrid();
                    UpdateBlackGrid();
                    textBoxTitle.Text = currentDeck.Title;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void newDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentDeck = new Deck();
            UpdateWhiteGrid();
            UpdateBlackGrid();
            textBoxTitle.Text = "untitled";
        }

        private void textBoxTitle_TextChanged(object sender, EventArgs e)
        {
            currentDeck.Title = textBoxTitle.Text;
        }

        
        
        //============================================================================================

        public static void SaveDeckXML(Deck deck, String path) //To Do: Move this to the OpenCards DLL
        {
            XmlSerializer writer = new XmlSerializer(typeof(Deck));

            try
            {
                StreamWriter file = new StreamWriter(path);
                writer.Serialize(file, deck);
                file.Close();
            }
            catch
            {
                MessageBox.Show("File write error!");
            }
        }

        public static Deck LoadDeckXML(String filename) //To Do: Move this to the OpenCards DLL
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(Deck));

            FileStream myFileStream = new FileStream(filename, FileMode.Open);

            Deck loadedDeck = (Deck)mySerializer.Deserialize(myFileStream);

            return loadedDeck;
        }

        //Update the white cards whenever the UI is edited.
        public void UpdateWhiteDeck()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    try
                    {
                        string value = row.Cells[0].Value.ToString();
                        Card newCard = new Card(value);
                        currentDeck.Add(newCard);
                    }
                    catch { }
                }
            }

            textBoxNumWhite.Text = Convert.ToString(currentDeck.WhiteCards.Count);
        }

        //Update UI when the deck is edited programmatically
        public void UpdateWhiteGrid()
        {
            //Wipe the whole grid
            dataGridView1.Rows.Clear();

            //Add a row for every Card in currentDeck
            foreach (Card card in currentDeck.WhiteCards)
            {
                string[] row = { card.Text };
                dataGridView1.Rows.Add(row);
            }

            textBoxNumWhite.Text = Convert.ToString(currentDeck.WhiteCards.Count);
        }

        //Update the black cards whenever the UI is edited.
        public void UpdateBlackDeck()
        {
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (!row.IsNewRow)
                {
                    try
                    {
                        string content = row.Cells[0].Value.ToString();
                        int blanks = Convert.ToInt32(row.Cells[1].Value);

                        //Enforce a minimum of 1 blank
                        if (blanks < 1)
                        {
                            blanks = 1;
                            row.Cells[1].Value = blanks;
                        }

                        BlackCard newCard = new BlackCard(content, blanks);
                        currentDeck.Add(newCard);
                    }
                    catch { }
                }
            }

            textBoxNumBlack.Text = Convert.ToString(currentDeck.BlackCards.Count);
        }

        //Update UI when the deck is edited programmatically
        public void UpdateBlackGrid()
        {
            //Wipe the whole grid
            dataGridView2.Rows.Clear();

            //Add a row for every Card in currentDeck
            foreach (BlackCard card in currentDeck.BlackCards)
            {
                string[] row = { card.Text, Convert.ToString(card.Blanks) };
                dataGridView2.Rows.Add(row);
            }

            textBoxNumBlack.Text = Convert.ToString(currentDeck.BlackCards.Count);
        }




        //============================================================================================
    }
}
