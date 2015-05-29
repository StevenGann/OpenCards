using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OpenCards
{
    [Serializable]
    public class BlackCard
    {
        public String Text = "";
        [XmlIgnore] public String Source = "Default";
        public int Blanks = 1; //The number of cards you can use to respond to this card.
                               //Even if the card has no blanks, this should be 1.
        public BlackCard()
        { }

        public BlackCard(String content, int blanks)
        {
            Text = content;
            Blanks = blanks;
        }

        public String ToString()
        {
            String result = "";

            result += "Black: \"";
            result += Text;
            result += "\", ";
            result += Convert.ToString(Blanks);
            result += " blanks\n";

            return result;
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

            DrawRoundedRectangle(g, new Rectangle(1, 1, width - 2, height - 2), (int)(30.0f*scalingFactor), Pens.White);


            //To Do: A lot of calculations being repeated. Clean this up!
            //       Replace these calculations with margins and H/W values that are only calculated once.

            //Text of the card
            rectf = new RectangleF((15.0f * scalingFactor), (20.0f * scalingFactor), width - (int)(30.0f * scalingFactor), height - (int)(40.0f * scalingFactor));
            g.DrawString(Text, new Font("Helvetica", fontSize), Brushes.White, rectf);

            //Source tag
            rectf = new RectangleF((15.0f * scalingFactor), height - (int)(20.0f * scalingFactor), width - (int)(30.0f * scalingFactor), height);

            try
            {
                g.DrawString(Source, new Font("Courier New", (int)((float)fontSize * 0.8)), Brushes.White, rectf);
            }
            catch { }

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

        public static void DrawRoundedRectangle(Graphics g, Rectangle r, int d, Pen p)
        {

            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();

            gp.AddArc(r.X, r.Y, d, d, 180, 90);
            gp.AddArc(r.X + r.Width - d, r.Y, d, d, 270, 90);
            gp.AddArc(r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 90);
            gp.AddArc(r.X, r.Y + r.Height - d, d, d, 90, 90);
            gp.AddLine(r.X, r.Y + r.Height - d, r.X, r.Y + d / 2);

            g.FillPath(Brushes.Black, gp);
            g.DrawPath(p, gp);
        }
    }
}
