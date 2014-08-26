using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OpenCards
{
    [Serializable]
    public class Card
    {
        public String Text = "";
        [XmlIgnore] public int Selection = 0;
        [XmlIgnore] public String Source = "Default"; //For cards from boosters, the name of the booster should be here.

        public Card()
        { }

        public Card(String content)
        {
            Text = content;
        }

        //Render Card into a bitmap for displaying in the GUI
        public PictureBox Render(int width, int height)
        {
            Bitmap resultBmp = new Bitmap(width, height);
            PictureBox result = new PictureBox();
            RectangleF rectf = new RectangleF();

            int fontSize = 12;
            int sizeFactor = 168; //The default width
            float scalingFactor = ((float)width / (float)sizeFactor);
            fontSize = (int)((float)fontSize * scalingFactor);

            //Rendering Code
            Graphics g = Graphics.FromImage(resultBmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            DrawRoundedRectangle(g, new Rectangle(1, 1, width-2, height-2), 30, Pens.Black);

            
            //To Do: A lot of calculations being repeated. Clean this up!
            //       Replace these calculations with margins and H/W values that are only calculated once.

            //Text of the card
            rectf = new RectangleF((15.0f*scalingFactor), (20.0f*scalingFactor), width-(int)(30.0f*scalingFactor), height-(int)(40.0f*scalingFactor));
            g.DrawString(Text, new Font("Helvetica", fontSize), Brushes.Black, rectf);

            //Source tag
            rectf = new RectangleF((15.0f * scalingFactor), height - (int)(20.0f * scalingFactor), width - (int)(30.0f * scalingFactor), height);
            try
            {
                g.DrawString(Source, new Font("Courier New", (int)((float)fontSize * 0.8)), Brushes.Black, rectf);
            }
            catch { }

            //If selected, draw extra colored outline and selection number
            if (Selection > 0)
            {
                Color selectionColor = Color.Blue;

                if (Selection == 2) { selectionColor = Color.Red; }
                if (Selection == 3) { selectionColor = Color.Green; }
                if (Selection == 4) { selectionColor = Color.Purple; }
                if (Selection == 5) { selectionColor = Color.Orange; }


                Pen selectionPen = new Pen(selectionColor, 5.0f);
                SolidBrush selectionBrush = new SolidBrush(selectionColor);

                DrawRoundedRectangle(g, new Rectangle(1, 1, width - 2, height - 2), 30, selectionPen);

                rectf = new RectangleF((3 * width) / 4, height - (int)(50.0f * scalingFactor), width - (int)(30.0f * scalingFactor), height - (int)(40.0f * scalingFactor));
                g.DrawString(Convert.ToString(Selection), new Font("Helvetica", (int)((float)fontSize * 2.0f)), selectionBrush, rectf);
            }

            g.Flush();

            //Put Bitmap in the PictureBox
            result.Size = new System.Drawing.Size(resultBmp.Width, resultBmp.Height);
            result.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            result.Image = resultBmp;

            return result;
        }

        //Render card with default dimensions. I hate having methods/constructors that HAVE to have arguments. Defaults can save time.
        public PictureBox Render()
        {
            //Regulation playing cards are 56mm x 88mm
            //I'll make the default dimensions 224px x 352px. This'll make the card downsampled in most cases.
            PictureBox result = Render(224, 352);
            return result;
        }

        private static void DrawRoundedRectangle(Graphics g, Rectangle r, int d, Pen p)
        {

            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();

            gp.AddArc(r.X, r.Y, d, d, 180, 90);
            gp.AddArc(r.X + r.Width - d, r.Y, d, d, 270, 90);
            gp.AddArc(r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 90);
            gp.AddArc(r.X, r.Y + r.Height - d, d, d, 90, 90);
            gp.AddLine(r.X, r.Y + r.Height - d, r.X, r.Y + d / 2);

            g.FillPath(Brushes.White, gp);
            g.DrawPath(p, gp);
        }
    }
}
