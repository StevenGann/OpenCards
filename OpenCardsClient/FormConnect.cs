using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenCardsClient
{
    public partial class FormConnect : Form
    {
        public IPAddress hostIP;
        public int hostPort = 0;
        public String username = "";


        public FormConnect()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] split = textBox2.Text.Split(new Char[] { ':' });

            try
            {
                hostIP = IPAddress.Parse(split[0]);

                try
                {
                    hostPort = Convert.ToInt32(split[1]);

                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Invalid Port");
                }
            }
            catch
            {
                MessageBox.Show("Invalid IP Address");
            }

            

            username = textBox1.Text;
        }
    }
}
