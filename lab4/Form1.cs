using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4
{
    public partial class Form1 : Form
    {
        private List<List<Point>> polygons = new List<List<Point>>();
        private List<Point> currentPolygon = new List<Point>();
        private Bitmap _bitmap;
        private Graphics _graphics;
        private Pen _pen;

        public Form1()
        {
            InitializeComponent();
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _graphics = Graphics.FromImage(_bitmap);
            _graphics.Clear(Color.White);
            _pen = new Pen(Color.Black, 2);
            pictureBox1.Image = _bitmap;

            //pictureBox1.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);
            //pictureBox1.Paint += new PaintEventHandler(pictureBox1_Paint);
        } 

        private void ClearButton_Click(object sender, EventArgs e)
        {
            polygons.Clear();
            currentPolygon.Clear();
            _graphics.Clear(Color.White);
            pictureBox1.Invalidate();
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
            // Рисуем все полигоны
            foreach (var polygon in polygons)
            {
                if (polygon.Count == 1)
                {
                    e.Graphics.DrawEllipse(_pen, polygon[0].X, polygon[0].Y, 2, 2); // Рисуем точку
                }
                else if (polygon.Count >= 2)
                {
                    for (int i = 0; i < polygon.Count - 1; i++)
                    {
                        e.Graphics.DrawLine(_pen, polygon[i], polygon[i + 1]); // Рисуем ребро
                    }
                    if (polygon.Count > 2)
                    {
                        e.Graphics.DrawPolygon(_pen, polygon.ToArray()); // Рисуем полигон
                    }
                }
            }

            // Рисуем текущий полигон
            if (currentPolygon.Count == 1)
            {
                e.Graphics.DrawEllipse(_pen, currentPolygon[0].X, currentPolygon[0].Y, 2, 2); // Рисуем точку
            }
            else if (currentPolygon.Count >= 2)
            {
                for (int i = 0; i < currentPolygon.Count - 1; i++)
                {
                    e.Graphics.DrawLine(_pen, currentPolygon[i], currentPolygon[i + 1]); // Рисуем ребро
                }
                if (currentPolygon.Count > 2)
                {
                    e.Graphics.DrawPolygon(_pen, currentPolygon.ToArray()); // Рисуем полигон
                }
            }
        }
    }
}
