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
        //Testing data
        Deck currentDeck = new Deck("");
        
        

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
            SaveDeckXML(currentDeck, "test.dek", "C:\\");
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            UpdateWhiteDeck();
        }

        private void loadDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Deck files (*.dek)|*.dek|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    currentDeck = LoadDeckXML(openFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        //============================================================================================

        public static void SaveDeckXML(Deck deck, String filename, String path) //To Do: Move this to the OpenCards DLL
        {
            XmlSerializer writer = new XmlSerializer(typeof(Deck));

            StreamWriter file = new StreamWriter(path + filename);
            writer.Serialize(file, deck);
            file.Close();
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
            currentDeck = new Deck("");

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    string value = row.Cells[0].Value.ToString();
                    Card newCard = new Card(value);
                    currentDeck.Add(newCard);
                }
            }

            
        }

        public void UpdateWhiteGrid()
        {
            //Wipe the whole grid
            //Add a row for every Card in currentDeck
        }

        




        //============================================================================================
    }
}
