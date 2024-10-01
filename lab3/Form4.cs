using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3
{
    public partial class Form4 : Form
    {
        Graphics _graphics;
        Bitmap bitmap;
        Point[] p = new Point[3];
        Color[] colors = new Color[3];
        int index = 0;
        Pen pen = new Pen(Color.Red, 1);
        Bitmap paletteBitmap;

        public Form4()
        {
            InitializeComponent();
            bitmap = new Bitmap(pic.Width, pic.Height);
            _graphics = Graphics.FromImage(bitmap);
            _graphics.Clear(Color.White);
            pic.Image = bitmap;
            paletteBitmap = new Bitmap(palettePic.Image);
        }

        private void pic_MouseClick(object sender, MouseEventArgs e)
        {
            if (index < 2)
            {
                p[index] = e.Location;
                colors[index] = pen.Color;
                index++;
                _graphics.DrawRectangle(pen, e.Location.X, e.Location.Y, 1, 1);
                pic.Refresh();
            }
            else
            {
                colors[2] = pen.Color;
                p[2] = e.Location;
                _graphics.DrawRectangle(pen, e.Location.X, e.Location.Y, 1, 1);
                DrawTriangle();
                pic.Refresh();
                pen.Color = colorLabel.BackColor;
                index = 0;
            }
        }

        private void palettePic_MouseClick(object sender, MouseEventArgs e)
        {
            if (paletteBitmap != null)
            {
                int paletteWidth = palettePic.Image.Width;
                int paletteHeight = palettePic.Image.Height;
                if (palettePic.SizeMode == PictureBoxSizeMode.StretchImage || palettePic.SizeMode == PictureBoxSizeMode.Zoom)
                {
                    float scaleX = (float)paletteWidth / palettePic.Width;
                    float scaleY = (float)paletteHeight / palettePic.Height;
                    int realX = (int)(e.X * scaleX);
                    int realY = (int)(e.Y * scaleY);

                    if (realX >= 0 && realX < paletteBitmap.Width && realY >= 0 && realY < paletteBitmap.Height)
                    {
                        Color selectedColor = paletteBitmap.GetPixel(realX, realY);
                        pen.Color = selectedColor;
                        colorLabel.BackColor = selectedColor;
                    }
                }
                else
                {
                    if (e.X >= 0 && e.X < paletteBitmap.Width && e.Y >= 0 && e.Y < paletteBitmap.Height)
                    {
                        Color selectedColor = paletteBitmap.GetPixel(e.X, e.Y);
                        pen.Color = selectedColor;
                        colorLabel.BackColor = selectedColor;
                    }
                }
            }
        }

        private void DrawTriangle()
        {
            if (p[1].Y < p[0].Y) Swap(0, 1);
            if (p[2].Y < p[0].Y) Swap(0, 2);
            if (p[2].Y < p[1].Y) Swap(1, 2);

            int top_y = p[0].Y;

            while (top_y < p[1].Y)
            {
                float Xleft, Xright;
                Xleft = InterpolateX(p[0], p[1], p[2], top_y, true, false);
                Xright = InterpolateX(p[0], p[1], p[2], top_y, false, false);
                Color Cleft = InterpolateColor(p[0], p[1], colors[0], colors[1], top_y);
                Color Cright = InterpolateColor(p[0], p[2], colors[0], colors[2], top_y);

                if (Xleft > Xright)
                {
                    (Xleft, Xright) = (Xright, Xleft);
                    (Cleft, Cright) = (Cright, Cleft);
                }

                for (float x = Xleft; x <= Xright; x++)
                {
                    float t = (x - Xleft) / (Xright - Xleft);
                    Color interpolatedColor = InterpolateColor(Cleft, Cright, t);
                    pen.Color = interpolatedColor;
                    _graphics.DrawRectangle(pen, x, top_y, 1, 1);
                }
                top_y++;
            }

            while (top_y < p[2].Y)
            {
                float Xleft, Xright;
                Xleft = InterpolateX(p[0], p[1], p[2], top_y, true, true);
                Xright = InterpolateX(p[0], p[1], p[2], top_y, false, true);
                Color Cleft = InterpolateColor(p[1], p[2], colors[1], colors[2], top_y);
                Color Cright = InterpolateColor(p[0], p[2], colors[0], colors[2], top_y);

                if (Xleft > Xright)
                {
                    (Xleft, Xright) = (Xright, Xleft);
                    (Cleft, Cright) = (Cright, Cleft);
                }

                for (float x = Xleft; x <= Xright; x++)
                {
                    float t = (x - Xleft) / (Xright - Xleft);
                    Color interpolatedColor = InterpolateColor(Cleft, Cright, t);
                    pen.Color = interpolatedColor;
                    _graphics.DrawRectangle(pen, x, top_y, 1, 1);
                }
                top_y++;
            }
        }


        private float InterpolateX(Point p0, Point p1, Point p2, float y, bool isLeft, bool afterMid)
        {
            if (!afterMid)
            {
                if (isLeft)
                {
                    if (p0.Y == p1.Y) 
                        return p0.X;

                    return p0.X + (p1.X - p0.X) * (y - p0.Y) / (p1.Y - p0.Y);
                }
                else
                {
                    if (p0.Y == p2.Y) 
                        return p0.X;

                    return p0.X + (p2.X - p0.X) * (y - p0.Y) / (p2.Y - p0.Y);
                }
            }
            else
            {
                if (isLeft)
                {
                    if (p1.Y == p2.Y) 
                        return p1.X;

                    return p1.X + (p2.X - p1.X) * (y - p1.Y) / (p2.Y - p1.Y);
                }
                else
                {
                    if (p0.Y == p2.Y) 
                        return p0.X;

                    return p0.X + (p2.X - p0.X) * (y - p0.Y) / (p2.Y - p0.Y);
                }
            }
        }

        private Color InterpolateColor(Point p0, Point p1, Color c0, Color c1, float y)
        {
            if (p0.Y == p1.Y) 
                return c0;

            float t = (y - p0.Y) / (p1.Y - p0.Y);
            return InterpolateColor(c0, c1, t);
        }

        private Color InterpolateColor(Color c1, Color c2, float t)
        {
            int r = (int)Math.Round(c1.R + (c2.R - c1.R) * t);
            int g = (int)Math.Round(c1.G + (c2.G - c1.G) * t);
            int b = (int)Math.Round(c1.B + (c2.B - c1.B) * t);

            r = Clamp(r, 0, 255);
            g = Clamp(g, 0, 255);
            b = Clamp(b, 0, 255);

            return Color.FromArgb(r, g, b);
        }

        private int Clamp(int value, int min, int max)
        {
            if (value < min) 
                return min;
            if (value > max) 
                return max;

            return value;
        }

        private void Swap(int i, int j)
        {
            Point tempPoint = p[i];
            p[i] = p[j];
            p[j] = tempPoint;

            Color tempColor = colors[i];
            colors[i] = colors[j];
            colors[j] = tempColor;
        }

        private void Erase_Click(object sender, EventArgs e)
        {
            _graphics.Clear(Color.White);
            index = 0;
            pic.Refresh();
        }
    }
}
