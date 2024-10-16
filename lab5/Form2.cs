using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab5
{
    public partial class Form2 : Form
    {
        Pen _pen;
        Bitmap _bitmap;
        Graphics _graphics;
        Random _random;

        int _depth;
        float _angle;
        float _initialAngle;
        string _axiom;
        Dictionary<char, string> _rules;
        Dictionary<string, string> _files;

        struct TreeFractalPoint
        {
            public PointF point;
            public int depth;

            public TreeFractalPoint(PointF point, int depth)
            {
                this.point = point;
                this.depth = depth;
            }
        }

        public Form2()
        {
            InitializeComponent();
            LoadFilesToComboBox();            
            InitializeFields();
            DrawFractal();
        }
        void InitializeFields()
        {
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
                ReadLSystem(_files[comboBox1.Text]);
            }
            _depth = trackBar1.Value;
            if (_bitmap != null)
            {
                _bitmap.Dispose();
            }
            _random = new Random();
            _pen = new Pen(Color.Black, 1);
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _graphics = Graphics.FromImage(_bitmap);
            pictureBox1.Image = _bitmap;
        }
        void DrawFractal()
        {
            if (comboBox1.Items.Count > 0)
            {
                ReadLSystem(_files[comboBox1.Text]);
            }

            _graphics.Clear(Color.White);

            string fractalString = FractalString(_axiom, _depth);

            if (comboBox1.Text.StartsWith("Tree"))
            {
                TreeFractalPoint[] points = TreeFractalPoints(new TreeFractalPoint(new PointF(0, 0), 0), fractalString);
                points = ScaleTreeFractalPoints(points);
                float maxDepth = points.Max(p => p.depth);

                for (int i = 0; i < points.Length - 1; i++)
                {
                    // Интерполяция толщины линии
                    float w = Interpolation(points[i].depth, 0, maxDepth, 10, 1);

                    // Интерполяция цвета
                    Byte R = (Byte)Interpolation(points[i].depth, 0, maxDepth, Color.SaddleBrown.R, Color.LimeGreen.R);
                    Byte G = (Byte)Interpolation(points[i].depth, 0, maxDepth, Color.SaddleBrown.G, Color.LimeGreen.G);
                    Byte B = (Byte)Interpolation(points[i].depth, 0, maxDepth, Color.SaddleBrown.B, Color.LimeGreen.B);

                    Pen treePen = new Pen(Color.FromArgb(R, G, B), w);

                    // Рисуем линии с измененной шириной
                    _graphics.DrawLine(treePen, points[i].point.X, points[i].point.Y, points[i + 1].point.X, points[i + 1].point.Y);
                }
            }
            else
            {
                PointF[] points = FractalPoints(new PointF(0, 0), fractalString);
                points = ScaleFractalPoints(points);

                for (int i = 0; i < points.Length - 1; i++)
                {
                    _graphics.DrawLine(_pen, points[i].X, points[i].Y, points[i + 1].X, points[i + 1].Y);
                }
            }

            pictureBox1.Invalidate();
        }

        float Interpolation(float x, float min1, float max1, float min2, float max2)
        {
            if (Math.Abs(max1 - min1) < 1e-5)
                return min2;

            return (x - min1) / (max1 - min1) * (max2 - min2) + min2;
        }
        TreeFractalPoint[] ScaleTreeFractalPoints(TreeFractalPoint[] points)
        {
            float minX = points.Min(p => p.point.X) - 10;
            float minY = points.Min(p => p.point.Y) - 10;
            float maxX = points.Max(p => p.point.X) + 10;
            float maxY = points.Max(p => p.point.Y) + 10;

            float scaleX = pictureBox1.Width / (maxX - minX);
            float scaleY = pictureBox1.Height / (maxY - minY);
            float scale = Math.Min(scaleX, scaleY);

            TreeFractalPoint[] scaledPoints = new TreeFractalPoint[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                scaledPoints[i].point.X = (points[i].point.X - minX) * scale + (pictureBox1.Width - (maxX - minX) * scale) / 2;
                scaledPoints[i].point.Y = (points[i].point.Y - minY) * scale + (pictureBox1.Height - (maxY - minY) * scale) / 2;
            }

            return scaledPoints;
        }
        PointF[] ScaleFractalPoints(PointF[] points)
        {
            float minX = points.Min(p => p.X) - 10;
            float minY = points.Min(p => p.Y) - 10;
            float maxX = points.Max(p => p.X) + 10;
            float maxY = points.Max(p => p.Y) + 10;

            float scaleX = pictureBox1.Width / (maxX - minX);
            float scaleY = pictureBox1.Height / (maxY - minY);
            float scale = Math.Min(scaleX, scaleY);

            PointF[] scaledPoints = new PointF[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                scaledPoints[i].X = (points[i].X - minX) * scale + (pictureBox1.Width - (maxX - minX) * scale) / 2;
                scaledPoints[i].Y = (points[i].Y - minY) * scale + (pictureBox1.Height - (maxY - minY) * scale) / 2;
            }

            return scaledPoints;
        }
        TreeFractalPoint[] TreeFractalPoints(TreeFractalPoint startPoint, string fractalString)
        {
            List<TreeFractalPoint> points = new List<TreeFractalPoint>();
            TreeFractalPoint currentPoint = startPoint;
            float currentAngle = _initialAngle;
            int length = 10;
            int depth = 0;
            points.Add(currentPoint);

            Stack<TreeFractalPoint> pointStack = new Stack<TreeFractalPoint>();
            Stack<float> angleStack = new Stack<float>();

            foreach (char ch in fractalString)
            {
                if (ch == '+')
                {
                    currentAngle += _angle + _random.Next(0, 20);
                }
                else if (ch == '-')
                {
                    currentAngle -= _angle - _random.Next(0, 20);
                }
                else if (ch == '[')
                {
                    pointStack.Push(new TreeFractalPoint(currentPoint.point, currentPoint.depth));
                    angleStack.Push(currentAngle);
                }
                else if (ch == ']')
                {
                    if (pointStack.Count > 0)
                    {
                        currentPoint = pointStack.Pop();
                        depth = currentPoint.depth;
                    }
                    if (angleStack.Count > 0)
                    {
                        currentAngle = angleStack.Pop();
                    }
                    points.Add(new TreeFractalPoint(currentPoint.point, currentPoint.depth));
                }
                else if (ch == '@')
                {
                    depth++;
                }

                if (char.IsLetter(ch))
                {                    
                    currentPoint.point.X += length;
                    currentPoint = new TreeFractalPoint(
                        RotatePointF(currentPoint.point, points[points.Count - 1].point, currentAngle),
                        depth
                    );
                    points.Add(currentPoint);
                }
            }

            return points.ToArray();
        }

        PointF[] FractalPoints(PointF startPoint, string fractalString)
        {
            List<PointF> points = new List<PointF>();
            PointF currentPoint = startPoint;
            float currentAngle = _initialAngle;
            int length = 10;
            points.Add(currentPoint);

            Stack<PointF> pointStack = new Stack<PointF>();
            Stack<float> angleStack = new Stack<float>();

            foreach (char ch in fractalString)
            {
                if (ch == '+')
                {
                    currentAngle += _angle;
                }
                else if (ch == '-')
                {
                    currentAngle -= _angle;
                }
                else if (ch == '[')
                {
                    pointStack.Push(currentPoint);
                    angleStack.Push(currentAngle);
                }
                else if (ch == ']')
                {
                    if (pointStack.Count > 0)
                    {
                        currentPoint = pointStack.Pop();
                    }
                    if (angleStack.Count > 0)
                    {
                        currentAngle = angleStack.Pop();
                    }
                    currentPoint = RotatePointF(currentPoint, points[points.Count - 1], currentAngle);
                    points.Add(currentPoint);
                }

                if (char.IsLetter(ch))
                {
                    currentPoint.X += length;
                    currentPoint = RotatePointF(currentPoint, points[points.Count - 1], currentAngle);
                    points.Add(currentPoint);                    
                }
            }

            return points.ToArray();
        }
        private System.Drawing.PointF RotatePointF(System.Drawing.PointF polygonPoint, System.Drawing.PointF PointofRotate, float rotateAngle)
        {
            double PointA, PointB;
            double angle = (rotateAngle / 180D) * Math.PI;

            PointA = -PointofRotate.X * Math.Cos(angle) + PointofRotate.Y * Math.Sin(angle) + PointofRotate.X;
            PointB = -PointofRotate.X * Math.Sin(angle) - PointofRotate.Y * Math.Cos(angle) + PointofRotate.Y;

            double[] offsetVector = new double[3] { polygonPoint.X, polygonPoint.Y, 1 };
            double[][] Matrix = new double[3][]
            {
                new double[3] {  Math.Cos(angle),   Math.Sin(angle), 0 },
                new double[3] { -Math.Sin(angle),   Math.Cos(angle), 0 },
                new double[3] { PointA, PointB, 1 }
            };

            double[] resultVector = Multiplydouble(Matrix, offsetVector);

            return new System.Drawing.PointF((float)resultVector[0], (float)resultVector[1]);
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
        string FractalString(string str, int depth)
        {
            if (depth == 0)
            {
                return str;
            }

            string result = "";
            foreach (char ch in str)
            {
                if (char.IsLetter(ch))
                {
                    result += FractalString(_rules[ch], depth - 1);
                } 
                else
                {
                    result += ch;
                }
            }

            return result;
        }
        private void ReadLSystem(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            string[] s = lines[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            _axiom = s[0];
            _angle = float.Parse(s[1]);
            _initialAngle = float.Parse(s[2]);
            _rules = new Dictionary<char, string>();
            for (int i = 1; i < lines.Length; i++)
            {
                _rules.Add(lines[i][0], lines[i].Substring(3));
            }
        }
        private void LoadFilesToComboBox()
        {
            string projectDirectory = Directory.GetParent(Application.StartupPath).Parent.FullName;
            string directoryPath = Path.Combine(projectDirectory, "Examples");

            if (Directory.Exists(directoryPath))
            {
                string[] files = Directory.GetFiles(directoryPath, "*.txt");

                comboBox1.Items.Clear();
                _files = new Dictionary<string, string>();

                foreach (string file in files)
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                    comboBox1.Items.Add(fileNameWithoutExtension);

                    _files[fileNameWithoutExtension] = file;
                }
            }
            else
            {
                MessageBox.Show("Директория не найдена.");
            }
        }        
        private void Clear()
        {
            if (_graphics != null)
            {
                _graphics.Clear(Color.White);
            }
            else
            {
                InitializeFields();
                _graphics.Clear(Color.White);
            }

            pictureBox1.Invalidate();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            _depth = trackBar1.Value; 
            Clear();
            DrawFractal();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear();
            DrawFractal();
        }
        private void Form2_SizeChanged(object sender, EventArgs e)
        {
            pictureBox1.Width = this.Width - pictureBox1.Location.X - 30; 
            pictureBox1.Height = this.Height - pictureBox1.Location.Y - 50;
            _depth = trackBar1.Value;
            if (_bitmap != null)
            {
                _bitmap.Dispose();
            }
            _pen = new Pen(Color.Black, 1);
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _graphics = Graphics.FromImage(_bitmap);
            pictureBox1.Image = _bitmap;
            Clear();
            DrawFractal();
        }
    }
}
