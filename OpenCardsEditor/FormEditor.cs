using OpenCards;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        //============================================================================================

        public static void SaveDeckXML(Deck deck, String filename, String path) //To Do: Move this to the OpenCards DLL
        {
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Deck));

            System.IO.StreamWriter file = new System.IO.StreamWriter(path + filename);
            writer.Serialize(file, deck);
            file.Close();
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

        




        //============================================================================================
    }
}
