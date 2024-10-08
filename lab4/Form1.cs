﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace lab4
{
    public partial class Form1 : Form
    {
        private List<List<Point>> polygons = new List<List<Point>>();
        private List<Point> currentPolygon = new List<Point>();
        private Bitmap _bitmap;
        private Graphics _graphics;
        private Pen _pen;
        private int intFlag = 0;
        private List<Point> tempEdge = new List<Point>();
        private Color polygonColor = Color.Black;
        private Color selectedPolygonColor = Color.Red;
        Point tempPoint; 
        public Form1()
        {
            InitializeComponent();
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _graphics = Graphics.FromImage(_bitmap);
            _graphics.Clear(Color.White);
            _pen = new Pen(Color.Black, 2);
            tempPoint = new Point(); // Потому что больше негде
            pictureBox1.Image = _bitmap;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton1.Checked)
            {
                StopDrawPolygon();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                intFlag = 0;
                tempEdge.Clear();
                outputTextBox.Text = "Выберите 4 точки для проверки пересечения.";
            }
            else if (!radioButton2.Checked)
            {                
                intFlag = 0;
                tempEdge.Clear();
                _graphics.Clear(Color.White);
                pictureBox1.Invalidate();
            }
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                intFlag = 0;
                outputTextBox.Text = "Нарисуйте отрезок и точку с любой стороны от него.";
            }  
            else if (!radioButton4.Checked)
            {
                if (polygons.Count > 0 && intFlag >= 2)
                {
                   polygons.RemoveAt(polygons.Count - 1);
                }
                intFlag = 0;
                currentPolygon.Clear();
                pictureBox1.Invalidate();
            }          
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            polygons.Clear();
            currentPolygon.Clear();
            _graphics.Clear(Color.White);
            pictureBox1.Invalidate();
            intFlag = 0;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (radioButton1.Checked)
            {
                if (e.Button == MouseButtons.Left)
                {
                    DrawPolygon(e);
                }
                else if (e.Button == MouseButtons.Right)
                {
                    StopDrawPolygon();
                }
            }
            else if (radioButton2.Checked)
            {
                HandleIntersectionMode(e);
            }
            else if (radioButton3.Checked)
            {
               
            } 
            else if(radioButton4.Checked)
            {
               CheckSidePoint(e);
            }
        }

        private void CheckSidePoint(MouseEventArgs e)
        {
            if (intFlag == 0)
            {
                intFlag++;
                DrawPolygon(e);
            }
            else if (intFlag == 1)
            {
                intFlag++;
                DrawPolygon(e);
                StopDrawPolygon();
            }
            else if (intFlag == 2)
            {
                intFlag++;
                DrawPolygon(e);
                Point point = currentPolygon[0];
                List<Point> edge = polygons[polygons.Count - 1];
                Point b = new Point(point.X - edge[1].X, point.Y - edge[1].Y);
                Point a = new Point(edge[0].X - edge[1].X, edge[0].Y - edge[1].Y);
                double result = b.Y * a.X - b.X * a.Y;
                if (result > 0)
                {
                    outputTextBox.Text = $"Точка ({point.X},{point.Y}) находится слева от ребра.";
                }
                else if (result < 0)
                {
                    outputTextBox.Text = $"Точка ({point.X},{point.Y}) находится справа от ребра.";
                }
                else
                {
                    outputTextBox.Text = $"Точка ({point.X},{point.Y}) находится на ребре.";
                }
                outputTextBox.Text += " Нажмите на экран, чтобы продолжить.";
            } 
            else if (intFlag == 3 && polygons.Count > 0)
            {
                polygons.RemoveAt(polygons.Count - 1);
                intFlag = 0;
                currentPolygon.Clear();
                outputTextBox.Text = "Нарисуйте отрезок и точку с любой стороны от него.";
            }
            pictureBox1.Invalidate();
        }
        private void StopDrawPolygon()
        {
            if (currentPolygon.Count > 0)
            {
                polygons.Add(new List<Point>(currentPolygon));
                currentPolygon.Clear();
                pictureBox1.Invalidate();
            }
        }

        private void DrawPolygon(MouseEventArgs e)
        {
            currentPolygon.Add(e.Location);
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var polygon in polygons)
            {
                if (polygon.Count == 1)
                {
                    e.Graphics.DrawEllipse(_pen, polygon[0].X, polygon[0].Y, 2, 2);
                }
                else if (polygon.Count >= 2)
                {
                    for (int i = 0; i < polygon.Count - 1; i++)
                    {
                        e.Graphics.DrawLine(_pen, polygon[i], polygon[i + 1]);
                    }
                    if (polygon.Count > 2)
                    {
                        e.Graphics.DrawPolygon(_pen, polygon.ToArray());
                    }
                }
            }

            if (currentPolygon.Count == 1)
            {
                e.Graphics.DrawEllipse(_pen, currentPolygon[0].X, currentPolygon[0].Y, 2, 2);
            }
            else if (currentPolygon.Count >= 2)
            {
                for (int i = 0; i < currentPolygon.Count - 1; i++)
                {
                    e.Graphics.DrawLine(_pen, currentPolygon[i], currentPolygon[i + 1]);
                }
                if (currentPolygon.Count > 2)
                {
                    e.Graphics.DrawPolygon(_pen, currentPolygon.ToArray());
                }
            }
        }
        private void HandleIntersectionMode(MouseEventArgs e)
        {
            if (intFlag < 4)
            {
                tempEdge.Add(e.Location);
                _bitmap.SetPixel(e.Location.X, e.Location.Y, polygonColor);
                intFlag++;

                if (tempEdge.Count % 2 == 0)
                {
                    _graphics.DrawLine(_pen, tempEdge[tempEdge.Count - 2], tempEdge[tempEdge.Count - 1]);
                }

                if (intFlag == 4)
                {
                    CheckIntersection();
                }
            }
            else if (intFlag == 5)
            {
                _graphics.Clear(Color.White);
                tempEdge.Clear();
                intFlag = 0;
                outputTextBox.Text = "Выберите 4 точки для нового пересечения.";
            }
            pictureBox1.Invalidate();
        }
        private Point? FindIntersection(Point a, Point b, Point c, Point d, out string message)
        {
            Point n = new Point(-(d.Y - c.Y), d.X - c.X);

            float denominator = n.X * (b.X - a.X) + n.Y * (b.Y - a.Y);
            if (denominator == 0)
            {
                message = "Прямые параллельны.";
                return null;
            }

            float t = -(float)(n.X * (a.X - c.X) + n.Y * (a.Y - c.Y)) / denominator;

            Point res = new Point(a.X + (int)(t * (b.X - a.X)), a.Y + (int)(t * (b.Y - a.Y)));

            if (res.X >= Math.Min(a.X, b.X) && res.X <= Math.Max(a.X, b.X) &&
                res.Y >= Math.Min(a.Y, b.Y) && res.Y <= Math.Max(a.Y, b.Y) &&
                res.X >= Math.Min(c.X, d.X) && res.X <= Math.Max(c.X, d.X) &&
                res.Y >= Math.Min(c.Y, d.Y) && res.Y <= Math.Max(c.Y, d.Y))
            {
                message = $"Точка пересечения - ({res.X}, {res.Y}).";
                return res;
            }
            else
            {
                message = "Рёбра не пересекаются.";
                return null;
            }
        }

        private void CheckIntersection()
        {
            Point a = tempEdge[0];
            Point b = tempEdge[1];
            Point c = tempEdge[2];
            Point d = tempEdge[3];

            string message;
            Point? intersection = FindIntersection(a, b, c, d, out message);

            if (intersection.HasValue)
            {
                _graphics.DrawRectangle(new Pen(selectedPolygonColor, 2), intersection.Value.X, intersection.Value.Y, 2, 2);
            }

            outputTextBox.Text = message + " Нажмите на экран, чтобы продолжить.";
            intFlag = 5; // Переход в следующий режим

            pictureBox1.Invalidate();
        }
    }
}