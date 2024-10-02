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

        public Form1()
        {
            InitializeComponent();
            this.MouseClick += new MouseEventHandler(OnMouseClick);
            this.Paint += new PaintEventHandler(OnPaint);
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            // Добавляем точку в текущий полигон
            currentPolygon.Add(new Point(e.X, e.Y));

            // Перерисовываем форму после каждого клика
            this.Invalidate();
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Black, 2);

            // Рисуем все полигоны
            foreach (var polygon in polygons)
            {
                if (polygon.Count == 1)
                {
                    g.DrawEllipse(pen, polygon[0].X - 2, polygon[0].Y - 2, 4, 4); // Рисуем точку
                }
                else if (polygon.Count >= 2)
                {
                    for (int i = 0; i < polygon.Count - 1; i++)
                    {
                        g.DrawLine(pen, polygon[i], polygon[i + 1]); // Рисуем ребро
                    }
                    if (polygon.Count > 2)
                    {
                        g.DrawPolygon(pen, polygon.ToArray()); // Рисуем полигон
                    }
                }
            }

            // Рисуем текущий полигон
            if (currentPolygon.Count == 1)
            {
                g.DrawEllipse(pen, currentPolygon[0].X - 2, currentPolygon[0].Y - 2, 4, 4); // Рисуем точку
            }
            else if (currentPolygon.Count >= 2)
            {
                for (int i = 0; i < currentPolygon.Count - 1; i++)
                {
                    g.DrawLine(pen, currentPolygon[i], currentPolygon[i + 1]); // Рисуем ребро
                }
                if (currentPolygon.Count > 2)
                {
                    g.DrawPolygon(pen, currentPolygon.ToArray()); // Рисуем полигон
                }
            }

            pen.Dispose();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            polygons.Clear();
            currentPolygon.Clear();
            this.Invalidate();
        }

        private void NewPolygonButton_Click(object sender, EventArgs e)
        {
            // Если текущий полигон не пуст, добавляем его в список полигонов
            if (currentPolygon.Count > 0)
            {
                polygons.Add(new List<Point>(currentPolygon)); // Сохраняем текущий полигон
                currentPolygon.Clear(); // Очищаем для нового полигона
                this.Invalidate(); // Перерисовываем форму
            }
        }
    }
}
