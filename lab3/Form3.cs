using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace lab3
{
    public partial class Form3 : Form
    {
        private bool isDrawing = false;
        private Point startPoint, endPoint;
        private Bitmap bitmap;
        public Form3()
        {
            InitializeComponent();
            bitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
        }
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            startPoint = e.Location;
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
            endPoint = e.Location;

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                if (radioButtonBresenham.Checked)
                {
                    DrawBresenhamLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                }
                else if (radioButtonWU.Checked)
                {
                    //DrawAALine(g, startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);
                    DrawWuLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y, g);
                }
            }

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(bitmap, 0, 0);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (bitmap != null)
            {
                Bitmap newBitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
                using (Graphics g = Graphics.FromImage(newBitmap))
                {
                    g.DrawImage(bitmap, 0, 0);
                }
                bitmap.Dispose();
                bitmap = newBitmap;
            }
            this.Invalidate();
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonBresenham.Checked)
            {
                radioButtonWU.Checked = false;
            }
            else if (radioButtonWU.Checked)
            {
                radioButtonBresenham.Checked = false;
            }
        }

        private void DrawBresenhamLine(int x0, int y0, int x1, int y1)
        {
            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);

            int dirx = x0 < x1 ? 1 : -1;
            int diry = y0 < y1 ? 1 : -1;

            int d;

            // Рисование линии с градиентом ≤ 1
            if (dx >= dy)
            {
                d = 2 * dy - dx;
                int y = y0;
                for (int x = x0; x != x1 + dirx; x += dirx)
                {
                    if (x >= 0 && x < bitmap.Width && y >= 0 && y < bitmap.Height)
                    {
                        bitmap.SetPixel(x, y, Color.Black);
                    }

                    if (d > 0)
                    {
                        y += diry;
                        d -= 2 * dx;
                    }
                    d += 2 * dy;
                }
            }
            // Рисование линии с градиентом > 1
            else
            {
                d = 2 * dx - dy;
                int x = x0;
                for (int y = y0; y != y1 + diry; y += diry)
                {
                    if (x >= 0 && x < bitmap.Width && y >= 0 && y < bitmap.Height)
                    {
                        bitmap.SetPixel(x, y, Color.Black);
                    }

                    if (d > 0)
                    {
                        x += dirx;
                        d -= 2 * dy;
                    }
                    d += 2 * dx;
                }
            }
        }

        private void DrawWuLine(int x0, int y0, int x1, int y1, Graphics g)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));

            float xIncrement = dx / (float)steps;
            float yIncrement = dy / (float)steps;
            float x = x0;
            float y = y0;

            for (int i = 0; i <= steps; i++)
            {
                float intensity = 1 - (x % 1);
                int xPixel = (int)x;
                int yPixel = (int)y;

                // Устанавливаем цвет для текущего пикселя
                if (xPixel >= 0 && xPixel < bitmap.Width && yPixel >= 0 && yPixel < bitmap.Height)
                {
                    Color color = Color.FromArgb((int)(intensity * 255), Color.Black);
                    bitmap.SetPixel(xPixel, yPixel, color);
                }

                // Устанавливаем цвет для следующего пикселя
                if (xPixel + 1 >= 0 && xPixel + 1 < bitmap.Width && yPixel >= 0 && yPixel < bitmap.Height)
                {
                    float nextIntensity = 1 - ((x + xIncrement) % 1);
                    Color nextColor = Color.FromArgb((int)(nextIntensity * 255), Color.Black);
                    bitmap.SetPixel(xPixel + 1, yPixel, nextColor);
                }
                x += xIncrement;
                y += yIncrement;
            }
        }
        /*
        private void DrawPixel(Graphics g, int x, int y, float brightness)
        {
            int c = (int)(255 * brightness);
            Color color = Color.FromArgb(c, c, c);
            bitmap.SetPixel(x, y, color);
        }

        private void DrawAALine(Graphics g, int x0, int y0, int x1, int y1)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);

            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            float dx = x1 - x0;
            float dy = y1 - y0;
            float gradient = dy / dx;
            if (dx == 0.0f) gradient = 1;

            float intersectY = y0;

            for (int x = x0; x <= x1; x++)
            {
                if (steep)
                {
                    DrawPixel(g, (int)intersectY, x, 1 - FractionalPart(intersectY));
                    DrawPixel(g, (int)intersectY + 1, x, FractionalPart(intersectY));
                }
                else
                {
                    DrawPixel(g, x, (int)intersectY, 1 - FractionalPart(intersectY));
                    DrawPixel(g, x, (int)intersectY + 1, FractionalPart(intersectY));
                }
                intersectY += gradient;
            }
        }

        private float FractionalPart(float x)
        {
            return x - (float)Math.Floor(x);
        }

        private void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
        */
    }
}