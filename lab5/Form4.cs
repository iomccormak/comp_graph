using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab5
{
    public partial class Form4 : Form
    {
        private Graphics g;
        private Bitmap bm;
        private List<Point> points = new List<Point>();
        private int selectedPointIndex = -1;
        private int movingPointIndex = -1;
        private bool isDragging = false;
        private const float pointRadius = 5f;

        public Form4()
        {
            InitializeComponent();
            bm = new Bitmap(850, 600);
            pictureBox1.Image = bm;
            g = Graphics.FromImage(pictureBox1.Image);
        }

        private void AddPoint(Point point)
        {
            points.Add(point);
            Redraw();
        }

        private void RemovePoint(Point location)
        {
            selectedPointIndex = FindPoint(location);
            if (selectedPointIndex != -1)   
            {
                points.RemoveAt(selectedPointIndex);
                Redraw();
            }
        }

        private int FindPoint(Point location)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (Math.Abs(points[i].X - location.X) < pointRadius && Math.Abs(points[i].Y - location.Y) < pointRadius)
                {
                    return i;
                }
            }
            return -1;
        }

        private void Redraw()
        {
            g.Clear(Color.White);
            DrawBezierCurve();
            DrawPoints();
            pictureBox1.Image = bm;
        }


        private void DrawPoints()
        {
            for (int i = 0; i < points.Count; i++)
            {
                g.FillRectangle(Brushes.Red, points[i].X - pointRadius, points[i].Y - pointRadius, pointRadius * 2, pointRadius * 2);
            }
        }

        private void DrawBezierCurve()
        {
            if (points.Count < 4) return;
            List<Point> result = new List<Point>();
            float step = 0.01f;

            if (points.Count < 6)
            {
                for (int i = 0; i <= points.Count - 3; i += 4)
                {
                    for (float t = 0; t <= 1; t += step)
                    {
                        float x = (float)(Math.Pow(1 - t, 3) * points[i].X +
                                           3 * Math.Pow(1 - t, 2) * t * points[i+1].X +
                                           3 * (1 - t) * Math.Pow(t, 2) * points[i+2].X +
                                           Math.Pow(t, 3) * points[i+3].X);

                        float y = (float)(Math.Pow(1 - t, 3) * points[i].Y +
                                           3 * Math.Pow(1 - t, 2) * t * points[i+1].Y +
                                           3 * (1 - t) * Math.Pow(t, 2) * points[i+2].Y +
                                           Math.Pow(t, 3) * points[i+3].Y);

                        result.Add(new Point((int)x, (int)y));
                    }
                }
            } 
            else
            {
                Point temp2 = new Point((points[2].X + points[3].X) / 2, (points[2].Y + points[3].Y) / 2);
                Point temp;

                for (float t = 0; t <= 1; t += step)
                {
                    float x = (float)(Math.Pow(1 - t, 3) * points[0].X +
                                       3 * Math.Pow(1 - t, 2) * t * points[1].X +
                                       3 * (1 - t) * Math.Pow(t, 2) * points[2].X +
                                       Math.Pow(t, 3) * temp2.X);

                    float y = (float)(Math.Pow(1 - t, 3) * points[0].Y +
                                       3 * Math.Pow(1 - t, 2) * t * points[1].Y +
                                       3 * (1 - t) * Math.Pow(t, 2) * points[2].Y +
                                       Math.Pow(t, 3) * temp2.Y);

                    result.Add(new Point((int)x, (int)y));
                }

                for (int i = 3; i <= points.Count - 5; i += 2)
                {
                    temp = new Point((points[i-1].X + points[i].X) / 2, (points[i-1].Y + points[i].Y) / 2);
                    temp2 = new Point((points[i + 1].X + points[i+2].X) / 2, (points[i + 1].Y + points[i+2].Y) / 2);

                    for (float t = 0; t <= 1; t += step)
                    {
                        float x = (float)(Math.Pow(1 - t, 3) * temp.X +
                                           3 * Math.Pow(1 - t, 2) * t * points[i].X +
                                           3 * (1 - t) * Math.Pow(t, 2) * points[i+1].X +
                                           Math.Pow(t, 3) * temp2.X);

                        float y = (float)(Math.Pow(1 - t, 3) * temp.Y +
                                           3 * Math.Pow(1 - t, 2) * t * points[i].Y +
                                           3 * (1 - t) * Math.Pow(t, 2) * points[i+1].Y +
                                           Math.Pow(t, 3) * temp2.Y);

                        result.Add(new Point((int)x, (int)y));
                    }
                }

                if (points.Count % 2 == 0)
                {
                    temp = new Point((points[points.Count - 4].X + points[points.Count - 3].X) / 2, 
                        (points[points.Count - 4].Y + points[points.Count - 3].Y) / 2);

                    for (float t = 0; t <= 1; t += step)
                    {
                        float x = (float)(Math.Pow(1 - t, 3) * temp.X +
                                           3 * Math.Pow(1 - t, 2) * t * points[points.Count - 3].X +
                                           3 * (1 - t) * Math.Pow(t, 2) * points[points.Count - 2].X +
                                           Math.Pow(t, 3) * points[points.Count - 1].X);

                        float y = (float)(Math.Pow(1 - t, 3) * temp.Y +
                                           3 * Math.Pow(1 - t, 2) * t * points[points.Count - 3].Y +
                                           3 * (1 - t) * Math.Pow(t, 2) * points[points.Count - 2].Y +
                                           Math.Pow(t, 3) * points[points.Count - 1].Y);

                        result.Add(new Point((int)x, (int)y));
                    }
                }
                else
                {
                    temp = new Point((points[points.Count - 5].X + points[points.Count - 4].X) / 2,
                        (points[points.Count - 5].Y + points[points.Count - 4].Y) / 2);

                    for (float t = 0; t <= 1; t += step)
                    {
                        float x = (float)(Math.Pow(1 - t, 3) * temp.X +
                                           3 * Math.Pow(1 - t, 2) * t * points[points.Count - 4].X +
                                           3 * (1 - t) * Math.Pow(t, 2) * points[points.Count - 3].X +
                                           Math.Pow(t, 3) * points[points.Count - 2].X);

                        float y = (float)(Math.Pow(1 - t, 3) * temp.Y +
                                           3 * Math.Pow(1 - t, 2) * t * points[points.Count - 4].Y +
                                           3 * (1 - t) * Math.Pow(t, 2) * points[points.Count - 3].Y +
                                           Math.Pow(t, 3) * points[points.Count - 2].Y);

                        result.Add(new Point((int)x, (int)y));
                    }
                }
            }
            


            if (result.Count > 1)
            {
                g.DrawLines(new Pen(Color.Blue), result.ToArray());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            points.Clear();
            g.Clear(pictureBox1.BackColor);
            bm = new Bitmap(850, 600);
            pictureBox1.Image = bm;
            g = Graphics.FromImage(pictureBox1.Image);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            selectedPointIndex = FindPoint(e.Location);
            if (selectedPointIndex != -1)
            {
                if (e.Button == MouseButtons.Left && !isDragging)
                {
                    isDragging = true;
                    movingPointIndex = selectedPointIndex;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    RemovePoint(e.Location);
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                AddPoint(e.Location);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && movingPointIndex != -1)
            {
                points[selectedPointIndex] = e.Location;
                Redraw();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            selectedPointIndex = -1;
            movingPointIndex = -1;
        }

    }
}
