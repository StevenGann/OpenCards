using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OpenCards
{
    [Serializable]
    public class Card
    {
        public String Text = "";
        public int Selection = 0;
        public String Source = "Default"; //For cards from boosters, the name of the booster should be here.

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

            //Rendering Code
            Graphics g = Graphics.FromImage(resultBmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            DrawRoundedRectangle(g, new Rectangle(1, 1, width-2, height-2), 30, Pens.Black);

            RectangleF rectf = new RectangleF(15, 20, width-30, height-40);//"Formatting Rectangle" - Not sure what this does...
            g.DrawString(Text, new Font("Helvetica", 12), Brushes.Black, rectf);

            rectf = new RectangleF(15, height - 20, width - 30, height);
            g.DrawString(Source, new Font("Helvetica", 8), Brushes.Black, rectf);

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
