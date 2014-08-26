using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenCards
{
    [Serializable]
    public class Response
    {
        public int Size = 0;
        public List<Card> Cards = new List<Card>();
        public bool IsFromCzar = false;

        public Response()
        { }

        public void Add(Card newCard)
        {
            Cards.Add(newCard);
            Size = Cards.Count;
        }

        public Panel Render(int width, int height)
        {
            Panel result = new Panel();

            int marginH = 0;    //ToDo: Replace these magic numbers with something adjustable.
            int marginV = 0;
            int newX = marginH;
            int newY = marginV;
            List<PictureBox> pictures = new List<PictureBox>();


            result.AutoSize = true;
            result.BackColor = SystemColors.ControlDarkDark;
            result.BorderStyle = BorderStyle.Fixed3D;
            //result.Width = ((width + marginH) * Cards.Count) + marginH;
            //result.Height = height + (marginV * 2);

            foreach (Card card in Cards)
            {
                PictureBox pb = card.Render(width, height);
                pictures.Add(pb);
            }

            foreach (PictureBox pb in pictures)
            {
                result.Controls.Add(pb);
                pb.Location = new System.Drawing.Point(newX, newY);                 //Move cards so they don't overlap.

                newX += (width + marginH);
            }

            return result;
        }
    }
}
