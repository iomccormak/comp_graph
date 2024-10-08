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
    enum Mode
    {
        None,
        Move_Polygon,
        Turn_Around_Point,
        Turn_Around_Center,
        Scaling_Relative_To_Point,
        Scaling_Relative_To_Center
    }

    public partial class Form1 : Form
    {
        private List<List<Point>> polygons = new List<List<Point>>();
        private List<Point> currentPolygon = new List<Point>();
        private Bitmap _bitmap;
        private Graphics _graphics;
        private Pen _pen;
        private Mode _mode;

        public Form1()
        {
            InitializeComponent();
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _graphics = Graphics.FromImage(_bitmap);
            _graphics.Clear(Color.White);
            _pen = new Pen(Color.Black, 2);
            pictureBox1.Image = _bitmap;
            comboBoxAthenian.SelectedIndex = 0;
            comboBoxPolygon.SelectedIndex = 0;
            //pictureBox1.MouseDown += new MouseEventHandler(pictureBox1_MouseDown);
            //pictureBox1.Paint += new PaintEventHandler(pictureBox1_Paint);
        } 

        private void ClearButton_Click(object sender, EventArgs e)
        {
            polygons.Clear();
            currentPolygon.Clear();
            comboBoxPolygon.Items.Clear();
            comboBoxPolygon.Refresh();
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
                comboBoxPolygon.Items.Add($"Polygon {polygons.Count}");
                comboBoxPolygon.Refresh();
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

        private void comboBoxAthenian_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxAthenian.SelectedIndex)
            {
                case 0:
                    _mode = Mode.None;
                    break;
                case 1:
                    _mode = Mode.Move_Polygon;
                    break;
                case 2:
                    _mode = Mode.Turn_Around_Point;
                    break;
                case 3:
                    _mode = Mode.Turn_Around_Center;
                    break;
                case 4:
                    _mode = Mode.Scaling_Relative_To_Point;
                    break;
                case 5:
                    _mode = Mode.Scaling_Relative_To_Center;
                    break;
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (_mode == Mode.None)
            {
                //Ваш код
            }
            else
            {
                switch (_mode)
                {
                    case Mode.Move_Polygon:
                        string[] s = textBoxInput.Text.Split(' ');
                        int dx = 0, dy = 0;
                        if (comboBoxPolygon.SelectedIndex == -1 || s.Length > 2 || !int.TryParse(s[0], out dx) || !int.TryParse(s[1], out dy))
                        {
                            textBoxOutput.Text = "Ошибка входных данных";
                            return;
                        }

                        for (int i = 0; i < polygons[comboBoxPolygon.SelectedIndex - 1].Count; i++)
                            polygons[comboBoxPolygon.SelectedIndex - 1][i] = Movepoint(polygons[comboBoxPolygon.SelectedIndex - 1][i], dx, dy);
                        comboBoxAthenian.SelectedIndex = 0;
                        comboBoxAthenian.Refresh();
                        comboBoxPolygon.SelectedIndex = 0;
                        comboBoxAthenian.Refresh();
                        textBoxInput.Text = "";
                        textBoxOutput.Text = "";
                        pictureBox1.Invalidate();
                        break;
                    case Mode.Turn_Around_Point:
                        string[] input = textBoxInput.Text.Split(' ');
                        if (comboBoxPolygon.SelectedIndex == -1 || input.Length != 3)
                        {
                            textBoxOutput.Text = "Ошибка входных данных. Введите x, y и угол поворота.";
                            return;
                        }
                        int pointX, pointY, turn;
                        if (!int.TryParse(input[0], out pointX) || !int.TryParse(input[1], out pointY) || !int.TryParse(input[2], out turn))
                        {
                            textBoxOutput.Text = "Ошибка ввода: некорректные данные.";
                            return;
                        }
                        Point rotationPoint = new Point(pointX, pointY);
                        for (int i = 0; i < polygons[comboBoxPolygon.SelectedIndex - 1].Count; i++)
                        {
                            polygons[comboBoxPolygon.SelectedIndex - 1][i] = RotatePoint(polygons[comboBoxPolygon.SelectedIndex - 1][i], rotationPoint, turn);
                        }
                        comboBoxAthenian.SelectedIndex = 0;
                        comboBoxAthenian.Refresh();
                        comboBoxPolygon.SelectedIndex = 0;
                        comboBoxAthenian.Refresh();
                        textBoxInput.Text = "";
                        textBoxOutput.Text = "";
                        pictureBox1.Invalidate();
                        break;
                    case Mode.Turn_Around_Center:
                        if (comboBoxPolygon.SelectedIndex == -1 || !int.TryParse(textBoxInput.Text, out turn))
                        {
                            textBoxOutput.Text = "Ошибка входных данных";
                            return;
                        }
                        Point tempP = PolygonCenter(polygons[comboBoxPolygon.SelectedIndex - 1]);
                        for (int i = 0; i < polygons[comboBoxPolygon.SelectedIndex - 1].Count; i++)
                            polygons[comboBoxPolygon.SelectedIndex - 1][i] = RotatePoint(polygons[comboBoxPolygon.SelectedIndex - 1][i], tempP, turn);
                        comboBoxAthenian.SelectedIndex = 0;
                        comboBoxAthenian.Refresh();
                        comboBoxPolygon.SelectedIndex = 0;
                        comboBoxAthenian.Refresh();
                        textBoxInput.Text = "";
                        textBoxOutput.Text = "";
                        pictureBox1.Invalidate();
                        break;
                    case Mode.Scaling_Relative_To_Center:
                        float k = 0f;
                        if (comboBoxPolygon.SelectedIndex == 0 || !float.TryParse(textBoxInput.Text, out k))
                        {
                            textBoxOutput.Text = "Ошибка входных данных";
                            return;
                        }

                        Point center = PolygonCenter(polygons[comboBoxPolygon.SelectedIndex - 1]);

                        for (int i = 0; i < polygons[comboBoxPolygon.SelectedIndex - 1].Count; i++)
                            polygons[comboBoxPolygon.SelectedIndex - 1][i] = ScalePoint(polygons[comboBoxPolygon.SelectedIndex - 1][i], center, k);
                        comboBoxAthenian.SelectedIndex = 0;
                        comboBoxAthenian.Refresh();
                        comboBoxPolygon.SelectedIndex = 0;
                        comboBoxAthenian.Refresh();
                        textBoxInput.Text = "";
                        textBoxOutput.Text = "";
                        pictureBox1.Invalidate();
                        break;
                    case Mode.Scaling_Relative_To_Point:
                        string[] scaleInput = textBoxInput.Text.Split(' ');
                        if (comboBoxPolygon.SelectedIndex == -1 || scaleInput.Length > 4)
                        {
                            textBoxOutput.Text = "Ошибка входных данных. Введите x, y и коэффициент масштабирования.";
                            return;
                        }

                        int scaleX, scaleY;
                        float scaleFactor;
                        if (!int.TryParse(scaleInput[0], out scaleX) || !int.TryParse(scaleInput[1], out scaleY) || !float.TryParse(scaleInput[2], out scaleFactor))
                        {
                            textBoxOutput.Text = "Ошибка ввода: некорректные данные.";
                            return;
                        }

                        Point scalePoint = new Point(scaleX, scaleY);
                        for (int i = 0; i < polygons[comboBoxPolygon.SelectedIndex - 1].Count; i++)
                        {
                            polygons[comboBoxPolygon.SelectedIndex - 1][i] = ScalePoint(polygons[comboBoxPolygon.SelectedIndex - 1][i], scalePoint, scaleFactor);
                        }

                        comboBoxAthenian.SelectedIndex = 0;
                        comboBoxAthenian.Refresh();
                        comboBoxPolygon.SelectedIndex = 0;
                        comboBoxAthenian.Refresh();
                        textBoxInput.Text = "";
                        textBoxOutput.Text = "";
                        pictureBox1.Invalidate();
                        break;


                }
            }
        }
        private Point Movepoint(Point polygonpoint, int dx, int dy)
        {
            int[][] Matrix = new int[3][]
            {
                    new int[3] { 1,   0, 0 },
                    new int[3] { 0,   1, 0 },
                    new int[3] { dx, dy, 1 }
            };
            int[] offsetVector = new int[3] { polygonpoint.X, polygonpoint.Y, 1 };
            int[] resultVector = Multiplyint(Matrix, offsetVector);
            return new Point((int)resultVector[0], (int)resultVector[1]);
        }

         private int[] Multiplyint(int[][] Matrix, int[] array)
        {
            int[] resultVector = new int[3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    resultVector[i] += Matrix[j][i] * array[j];
            }
            return resultVector;
        }
        private double[] Multiplydouble(double[][] Matrix, int[] array)
        {
            double[] resultVector = new double[3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    resultVector[i] += Matrix[j][i] * array[j];
            }
            return resultVector;
        }
        private Point RotatePoint(Point polygonpoint, Point PointofRotate, int rotateAngle)
        {
            double pointA, pointB;
            double angle = (rotateAngle / 180D) * Math.PI;

            pointA = -PointofRotate.X * Math.Cos(angle) + PointofRotate.Y * Math.Sin(angle) + PointofRotate.X;
            pointB = -PointofRotate.X * Math.Sin(angle) - PointofRotate.Y * Math.Cos(angle) + PointofRotate.Y;

            int[] offsetVector = new int[3] { polygonpoint.X, polygonpoint.Y, 1 };
            double[][] Matrix = new double[3][]
            {
                new double[3] {  Math.Cos(angle),   Math.Sin(angle), 0 },
                new double[3] { -Math.Sin(angle),   Math.Cos(angle), 0 },
                new double[3] { pointA, pointB, 1 }
            };
            double[] resultVector = Multiplydouble(Matrix, offsetVector);
            return new Point((int)resultVector[0], (int)resultVector[1]);
        }
        private Point PolygonCenter(List<Point> polygon)
        {
            int a = 0;
            int cx = 0;
            int cy = 0;

            for (int i = 0; i < polygon.Count; i++)
            {
                int temp = polygon[i].X * polygon[(i + 1) % polygon.Count].Y - polygon[(i + 1) % polygon.Count].X * polygon[i].Y;
                a += temp;
                cx += (polygon[i].X + polygon[(i + 1) % polygon.Count].X) * temp;
                cy += (polygon[i].Y + polygon[(i + 1) % polygon.Count].Y) * temp;
            }
            a /= 2;

            return new Point(cx / (6 * a), cy / (6 * a));
        }
        private Point ScalePoint(Point polygonpoint, Point randompoint, float k)
        {
            int[] offsetVector = new int[3] { polygonpoint.X - randompoint.X, polygonpoint.Y - randompoint.Y, 1 };
            double[][] Matrix = new double[3][]
            {
                new double[3] {  k,   0, 0 },
                new double[3] { 0,   k, 0 },
                new double[3] { 0, 0, 1 }
            };
            double[] resultVector = Multiplydouble(Matrix, offsetVector);
            return new Point((int)resultVector[0] + randompoint.X, (int)resultVector[1] + randompoint.Y);
        }
    }
}
