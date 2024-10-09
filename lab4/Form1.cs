using System;
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
    enum Mode
    {
        None,
        Move_Polygon,
        Turn_Around_PointF,
        Turn_Around_Center,
        Scaling_Relative_To_PointF,
        Scaling_Relative_To_Center,
    }

    public partial class Form1 : Form
    {
        private List<List<PointF>> polygons = new List<List<PointF>>();
        private List<PointF> currentPolygon = new List<PointF>();
        private Bitmap _bitmap;
        private Graphics _graphics;
        private Pen _pen;
        private Mode _mode;
        private int intFlag = 0;
        private List<PointF> tempEdge = new List<PointF>();
        private Color polygonColor = Color.Black;
        private Color selectedPolygonColor = Color.Red;
        PointF tempPointF; 
        public Form1()
        {
            InitializeComponent();
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _graphics = Graphics.FromImage(_bitmap);
            _graphics.Clear(Color.White);
            _pen = new Pen(Color.Black, 2);
            tempPointF = new PointF();
            pictureBox1.Image = _bitmap;
            comboBoxAthenian.SelectedIndex = 0;
            comboBoxPolygon.SelectedIndex = 0;
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
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                if (polygons.Count > 0)
                {
                    outputTextBox.Text = "Выберите точку.";
                }
                else
                {
                    outputTextBox.Text = "Нет полигонов";
                }
            }
            else if (!radioButton4.Checked)
            {
                //if (polygons.Count > 0 && intFlag == 2)
                //{
                //    polygons.RemoveAt(polygons.Count - 1);
                //}
                intFlag = 0;
                currentPolygon.Clear();
                pictureBox1.Invalidate();
            }               
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                outputTextBox.Text = "Нарисуйте отрезок и точку с любой стороны от него.";
            }  
            else if (!radioButton4.Checked)
            {
                if (polygons.Count > 0 && intFlag >= 2)
                {
                   polygons.RemoveAt(polygons.Count - 1);
                }                
            }
            intFlag = 0;
            currentPolygon.Clear();
            pictureBox1.Invalidate();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            polygons.Clear();
            currentPolygon.Clear();
            comboBoxPolygon.Items.Clear();
            comboBoxPolygon.Refresh();
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
                if (intFlag == 0)
                {
                    DrawPolygon(e);
                    intFlag = 1;

                    comboBoxPolygon.Enabled = true;
                    checkButton.Enabled = true;
                    outputTextBox.Text = "Выберите полигон. После чего нажмите ПРОВЕРИТЬ.";
                }
                else if (intFlag == 2)
                {
                    intFlag = 0;
                    comboBoxPolygon.SelectedIndex = 0;
                    outputTextBox.Text = "";
                    currentPolygon.Clear();
                    StopDrawPolygon();
                    pictureBox1.Invalidate();
                }
            } 
            else if(radioButton4.Checked)
            {
               CheckSidePointF(e);
            }
        }

        private void CheckSidePointF(MouseEventArgs e)
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
                PointF PointF = currentPolygon[0];
                List<PointF> edge = polygons[polygons.Count - 1];
                PointF b = new PointF(PointF.X - edge[1].X, PointF.Y - edge[1].Y);
                PointF a = new PointF(edge[0].X - edge[1].X, edge[0].Y - edge[1].Y);
                double result = b.Y * a.X - b.X * a.Y;
                if (result > 0)
                {
                    outputTextBox.Text = $"Точка ({PointF.X},{PointF.Y}) находится слева от ребра.";
                }
                else if (result < 0)
                {
                    outputTextBox.Text = $"Точка ({PointF.X},{PointF.Y}) находится справа от ребра.";
                }
                else
                {
                    outputTextBox.Text = $"Точка ({PointF.X},{PointF.Y}) находится на ребре.";
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
                polygons.Add(new List<PointF>(currentPolygon));
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

        private void comboBoxAthenian_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxAthenian.SelectedIndex)
            {
                case 0:
                    _mode = Mode.None;
                    applyButton.Enabled = false;
                    break;
                case 1:
                    _mode = Mode.Move_Polygon;
                    applyButton.Enabled = true;
                    outputTextBox.Text = "Введите координаты (x,y) через пробел";
                    break;
                case 2:
                    _mode = Mode.Turn_Around_PointF;
                    applyButton.Enabled = true;
                    outputTextBox.Text = "Введите координаты (x,y) и угол поворота через пробелы";
                    break;
                case 3:
                    _mode = Mode.Turn_Around_Center;
                    applyButton.Enabled = true;
                    outputTextBox.Text = "Введите угол поворота";
                    break;
                case 4:
                    _mode = Mode.Scaling_Relative_To_PointF;
                    applyButton.Enabled = true;
                    outputTextBox.Text = "Введите координаты (x,y) и коэффициент масштабирования через пробелы";
                    break;
                case 5:
                    _mode = Mode.Scaling_Relative_To_Center;
                    applyButton.Enabled = true;
                    outputTextBox.Text = "Введите коэффициент масштабирования";
                    break;
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (_mode != Mode.None)
            {
                switch (_mode)
                {
                    case Mode.Move_Polygon:
                        string[] s = textBoxInput.Text.Split(' ');
                        int dx = 0, dy = 0;
                       
                        if (comboBoxPolygon.SelectedIndex == -1 || s.Length != 2 || !int.TryParse(s[0], out dx) || !int.TryParse(s[1], out dy))
                        {
                            outputTextBox.Text = "Ошибка входных данных";
                            return;
                        }

                        for (int i = 0; i < polygons[comboBoxPolygon.SelectedIndex - 1].Count; i++)
                            polygons[comboBoxPolygon.SelectedIndex - 1][i] = MovePointF(polygons[comboBoxPolygon.SelectedIndex - 1][i], dx, dy);
                        comboBoxAthenian.SelectedIndex = 0;
                        comboBoxAthenian.Refresh();
                        comboBoxPolygon.SelectedIndex = 0;
                        comboBoxAthenian.Refresh();
                        textBoxInput.Text = "";
                        outputTextBox.Text = "";
                        pictureBox1.Invalidate();
                        break;
                    case Mode.Turn_Around_PointF:
                        string[] input = textBoxInput.Text.Split(' ');
                        if (comboBoxPolygon.SelectedIndex == -1 || input.Length != 3)
                        {
                            outputTextBox.Text = "Ошибка входных данных. Введите x, y и угол поворота.";
                            return;
                        }
                        int PointFX, PointFY, turn;
                        if (!int.TryParse(input[0], out PointFX) || !int.TryParse(input[1], out PointFY) || !int.TryParse(input[2], out turn))
                        {
                            outputTextBox.Text = "Ошибка ввода: некорректные данные.";
                            return;
                        }
                        PointF rotationPointF = new PointF(PointFX, PointFY);
                        for (int i = 0; i < polygons[comboBoxPolygon.SelectedIndex - 1].Count; i++)
                        {
                            polygons[comboBoxPolygon.SelectedIndex - 1][i] = RotatePointF(polygons[comboBoxPolygon.SelectedIndex - 1][i], rotationPointF, turn);
                        }
                        comboBoxAthenian.SelectedIndex = 0;
                        comboBoxAthenian.Refresh();
                        comboBoxPolygon.SelectedIndex = 0;
                        comboBoxAthenian.Refresh();
                        textBoxInput.Text = "";
                        outputTextBox.Text = "";
                        pictureBox1.Invalidate();
                        break;
                    case Mode.Turn_Around_Center:
                        if (comboBoxPolygon.SelectedIndex == -1 || !int.TryParse(textBoxInput.Text, out turn))
                        {
                            outputTextBox.Text = "Ошибка входных данных";
                            return;
                        }
                        PointF tempP = PolygonCenter(polygons[comboBoxPolygon.SelectedIndex - 1]);
                        for (int i = 0; i < polygons[comboBoxPolygon.SelectedIndex - 1].Count; i++)
                            polygons[comboBoxPolygon.SelectedIndex - 1][i] = RotatePointF(polygons[comboBoxPolygon.SelectedIndex - 1][i], tempP, turn);
                        comboBoxAthenian.SelectedIndex = 0;
                        comboBoxAthenian.Refresh();
                        comboBoxPolygon.SelectedIndex = 0;
                        comboBoxAthenian.Refresh();
                        textBoxInput.Text = "";
                        outputTextBox.Text = "";
                        pictureBox1.Invalidate();
                        break;
                    case Mode.Scaling_Relative_To_Center:
                        float k = 0f;
                        if (comboBoxPolygon.SelectedIndex == 0 || !float.TryParse(textBoxInput.Text, out k))
                        {
                            outputTextBox.Text = "Ошибка входных данных";
                            return;
                        }

                        PointF center = PolygonCenter(polygons[comboBoxPolygon.SelectedIndex - 1]);

                        for (int i = 0; i < polygons[comboBoxPolygon.SelectedIndex - 1].Count; i++)
                            polygons[comboBoxPolygon.SelectedIndex - 1][i] = ScalePointF(polygons[comboBoxPolygon.SelectedIndex - 1][i], center, k);
                        comboBoxAthenian.SelectedIndex = 0;
                        comboBoxAthenian.Refresh();
                        comboBoxPolygon.SelectedIndex = 0;
                        comboBoxAthenian.Refresh();
                        textBoxInput.Text = "";
                        outputTextBox.Text = "";
                        pictureBox1.Invalidate();
                        break;
                    case Mode.Scaling_Relative_To_PointF:
                        string[] scaleInput = textBoxInput.Text.Split(' ');
                        if (comboBoxPolygon.SelectedIndex == -1 || scaleInput.Length > 4)
                        {
                            outputTextBox.Text = "Ошибка входных данных. Введите x, y и коэффициент масштабирования.";
                            return;
                        }

                        int scaleX, scaleY;
                        float scaleFactor;
                        if (!int.TryParse(scaleInput[0], out scaleX) || !int.TryParse(scaleInput[1], out scaleY) || !float.TryParse(scaleInput[2], out scaleFactor))
                        {
                            outputTextBox.Text = "Ошибка ввода: некорректные данные.";
                            return;
                        }

                        PointF scalePointF = new PointF(scaleX, scaleY);
                        for (int i = 0; i < polygons[comboBoxPolygon.SelectedIndex - 1].Count; i++)
                        {
                            polygons[comboBoxPolygon.SelectedIndex - 1][i] = ScalePointF(polygons[comboBoxPolygon.SelectedIndex - 1][i], scalePointF, scaleFactor);
                        }

                        comboBoxAthenian.SelectedIndex = 0;
                        comboBoxAthenian.Refresh();
                        comboBoxPolygon.SelectedIndex = 0;
                        comboBoxAthenian.Refresh();
                        textBoxInput.Text = "";
                        outputTextBox.Text = "";
                        pictureBox1.Invalidate();
                        break;
                }
            }

        }


        private void checkButton_Click(object sender, EventArgs e)
        {
            if (comboBoxPolygon.SelectedIndex == 0)
            {
                outputTextBox.Text = "Ошибка входных данных";
                return;
            }

            int cnt = 0;
            List<PointF> l = new List<PointF>(polygons[comboBoxPolygon.SelectedIndex - 1]);
            float yPoint = currentPolygon[0].Y;

            if (l.Count > 2)
            {
                for (int i = 0; i < l.Count; i++)
                {
                    PointF c = l[i];
                    PointF d = l[(i + 1) % l.Count];

                    if (c.Y == d.Y)
                        continue;

                    PointF lower = c.Y < d.Y ? c : d;
                    PointF upper = c.Y < d.Y ? d : c;

                    if (yPoint < upper.Y && yPoint >= lower.Y)
                    {
                        float intersectionX = (yPoint - lower.Y) * (upper.X - lower.X) / (upper.Y - lower.Y) + lower.X;
                        if (intersectionX > currentPolygon[0].X)
                        {
                            cnt++;
                        }
                    }
                }
            }

            if (cnt % 2 == 0)
                outputTextBox.Text = "Точка не лежит внутри полигона.";
            else
                outputTextBox.Text = "Точка лежит внутри полигона.";

            outputTextBox.Text += " Нажмите на экран, чтобы продолжить.";
            comboBoxPolygon.Enabled = false;
            checkButton.Enabled = false;
            intFlag = 2;
        }



        private PointF MovePointF(PointF polygonPointF, int dx, int dy)
        {
            double[][] Matrix = new double[3][]
            {
                    new double[3] { 1,   0, 0 },
                    new double[3] { 0,   1, 0 },
                    new double[3] { dx, dy, 1 }
            };
            double[] offsetVector = new double[3] { polygonPointF.X, polygonPointF.Y, 1 };
            double[] resultVector = Multiplydouble(Matrix, offsetVector);
            return new PointF((float)resultVector[0], (float)resultVector[1]);
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

        private double[] Multiplydouble(double[][] Matrix, double[] array)
        {
            double[] resultVector = new double[3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    resultVector[i] += Matrix[j][i] * array[j];
            }
            return resultVector;
        }

        private PointF RotatePointF(PointF polygonPointF, PointF PointFofRotate, int rotateAngle)
        {
            double PointFA, PointFB;
            double angle = (rotateAngle / 180D) * Math.PI;

            PointFA = -PointFofRotate.X * Math.Cos(angle) + PointFofRotate.Y * Math.Sin(angle) + PointFofRotate.X;
            PointFB = -PointFofRotate.X * Math.Sin(angle) - PointFofRotate.Y * Math.Cos(angle) + PointFofRotate.Y;

            double[] offsetVector = new double[3] { polygonPointF.X, polygonPointF.Y, 1 };
            double[][] Matrix = new double[3][]
            {
                new double[3] {  Math.Cos(angle),   Math.Sin(angle), 0 },
                new double[3] { -Math.Sin(angle),   Math.Cos(angle), 0 },
                new double[3] { PointFA, PointFB, 1 }
            };
            double[] resultVector = Multiplydouble(Matrix, offsetVector);
            return new PointF((float)resultVector[0], (float)resultVector[1]);
        }

        private PointF PolygonCenter(List<PointF> polygon)
        {
            float a = 0;
            float cx = 0;
            float cy = 0;

            for (int i = 0; i < polygon.Count; i++)
            {
                float temp = polygon[i].X * polygon[(i + 1) % polygon.Count].Y - polygon[(i + 1) % polygon.Count].X * polygon[i].Y;
                a += temp;
                cx += (polygon[i].X + polygon[(i + 1) % polygon.Count].X) * temp;
                cy += (polygon[i].Y + polygon[(i + 1) % polygon.Count].Y) * temp;
            }
            a /= 2;

            return new PointF(cx / (6 * a), cy / (6 * a));
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
        private PointF ScalePointF(PointF polygonPointF, PointF randomPointF, float k)
        {
            double[] offsetVector = new double[3] { polygonPointF.X - randomPointF.X, polygonPointF.Y - randomPointF.Y, 1 };
            double[][] Matrix = new double[3][]
            {
                new double[3] {  k,   0, 0 },
                new double[3] { 0,   k, 0 },
                new double[3] { 0, 0, 1 }
            };
            double[] resultVector = Multiplydouble(Matrix, offsetVector);
            return new PointF((float)resultVector[0] + randomPointF.X, (float)resultVector[1] + randomPointF.Y);
        }
        private PointF? FindIntersection(PointF a, PointF b, PointF c, PointF d, out string message)
        {
            PointF n = new PointF(-(d.Y - c.Y), d.X - c.X);

            float denominator = n.X * (b.X - a.X) + n.Y * (b.Y - a.Y);
            if (denominator == 0)
            {
                message = "Прямые параллельны.";
                return null;
            }

            float t = -(float)(n.X * (a.X - c.X) + n.Y * (a.Y - c.Y)) / denominator;

            PointF res = new PointF(a.X + (int)(t * (b.X - a.X)), a.Y + (int)(t * (b.Y - a.Y)));

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
            PointF a = tempEdge[0];
            PointF b = tempEdge[1];
            PointF c = tempEdge[2];
            PointF d = tempEdge[3];

            string message;
            PointF? intersection = FindIntersection(a, b, c, d, out message);

            if (intersection.HasValue)
            {
                _graphics.DrawRectangle(new Pen(selectedPolygonColor, 2), intersection.Value.X, intersection.Value.Y, 2, 2);
            }

            outputTextBox.Text = message + " Нажмите на экран, чтобы продолжить.";
            intFlag = 5;

            pictureBox1.Invalidate();
        }

        
    }
}