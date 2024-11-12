using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace lab7
{    
    public partial class Form1 : Form
    {
        Pen _pen;
        Bitmap _bitmap;
        Bitmap _bitmapRotationFigure;
        Graphics _graphics;
        Graphics _graphicsRotationFigure;
        Polyhedron _polyhedron;
        float[][] _modelMatrix;
        Mode _mode;
        ModeRotationFigure _modeRotationFigure;
        List<Point> _points;

        enum Mode
        {
            None,
            Translation,
            Reflection,
            ScaleRelativeCenter,
            RotateAxis,
            RotateLine,
        }

        enum ModeRotationFigure
        {
            None,
            DrawPoints,
            JarvisMarch,
        }

        public Form1()
        {
            InitializeComponent();
            InitModelMatrix();
            _polyhedron = new Hexahedron();
            _pen = new Pen(Color.Black, 1);
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _bitmapRotationFigure = new Bitmap(pictureBoxRotationFigure.Width, pictureBoxRotationFigure.Height);    
            _graphics = Graphics.FromImage(_bitmap);
            _graphicsRotationFigure = Graphics.FromImage(_bitmapRotationFigure);
            _graphicsRotationFigure.Clear(Color.White);
            pictureBox1.Image = _bitmap;
            pictureBoxRotationFigure.Image = _bitmapRotationFigure;
            applyButton.Enabled = false;
            checkBox1.Checked = false;
            comboBoxPolyhedron.SelectedIndex = 4;
            _mode = Mode.None;
            _modeRotationFigure = ModeRotationFigure.DrawPoints;
            createRotationFigureButton.Enabled = false;
            _points = new List<Point>();
            this.MouseWheel += new MouseEventHandler(pictureBox1_OnMouseWheel);
            DrawPolyhedron();
        }        

        private void InitModelMatrix()
        {
            _modelMatrix = new float[4][]
            {
                new float[4] { 1, 0, 0, 0 },
                new float[4] { 0, 1, 0, 0 },
                new float[4] { 0, 0, 1, 0 },
                new float[4] { 0, 0, 0, 1 },
            };
        }

        public void DrawPolyhedron()
        {
            _graphics.Clear(Color.White);

            if (radioButton1.Checked)
            {
                DrawPerspective();
            }
            else if (radioButton2.Checked)
            {
                DrawAxonometry();
            }

            pictureBox1.Refresh();
        }       

        public void DrawPerspective()
        {
            float c = -pictureBox1.Width;
            float offsetX = pictureBox1.Width / 2;
            float offsetY = pictureBox1.Height / 2;

            float[][] MatrixPerspective = Matrices.Perspective(c);

            List<Point3D> points = new List<Point3D>();

            foreach (var point in _polyhedron.points)
            {
                var p = point.Clone();
                p.ApplyMatrix(_modelMatrix);
                p.ApplyMatrix(MatrixPerspective);
                points.Add(p);
            }

            foreach (var face in _polyhedron.faces)
            {
                var indexes = face.indexes;

                for (int i = 0; i < indexes.Count; i++)
                {
                    Point3D p1, p2;
                    if (i == indexes.Count - 1)
                    {
                        p1 = points[indexes[0]];
                        p2 = points[indexes[i]];
                    }
                    else
                    {
                        p1 = points[indexes[i]];
                        p2 = points[indexes[i + 1]];
                    }

                    _graphics.DrawLine(
                            _pen,
                            p1.X / p1.W + offsetX, p1.Y / p1.W + offsetY,
                            p2.X / p2.W + offsetX, p2.Y / p2.W + offsetY
                            );
                }
            }

            int l = Math.Max(pictureBox1.Height, pictureBox1.Width);
            List<Point3D> Ox = new List<Point3D>() { new Point3D(0, 0, 0), new Point3D(l, 0, 0) };
            List<Point3D> Oy = new List<Point3D>() { new Point3D(0, 0, 0), new Point3D(0, l, 0) };
            List<Point3D> Oz = new List<Point3D>() { new Point3D(0, 0, 0), new Point3D(0, 0, l) };
            List<Color> colors = new List<Color>() { Color.Red, Color.Green, Color.Blue };
            var axeses = new List<List<Point3D>>() { Ox, Oy, Oz };
            for (int i = 0; i < axeses.Count; i++)
            {
                var axes = axeses[i];
                axes[0].ApplyMatrix(MatrixPerspective);
                axes[1].ApplyMatrix(MatrixPerspective);

                _graphics.DrawLine(
                                new Pen(colors[i], 2),
                                axes[0].X / axes[0].W + offsetX, axes[0].Y / axes[0].W + offsetY,
                                axes[1].X / axes[1].W + offsetX, axes[1].Y / axes[1].W + offsetY
                                );
            }

        }

        private void DrawAxonometry()
        {
            double phi = 35.26d;
            double psi = 45d;

            float offsetX = pictureBox1.Width / 2;
            float offsetY = pictureBox1.Height / 2;

            float[][] MatrixAxonometry = Matrices.Axonometry(phi, psi);

            List<Point3D> points = new List<Point3D>();

            foreach (var point in _polyhedron.points)
            {
                var p = point.Clone();
                p.ApplyMatrix(_modelMatrix);
                p.ApplyMatrix(MatrixAxonometry);
                points.Add(p);
            }

            foreach (var face in _polyhedron.faces)
            {
                var indexes = face.indexes;

                for (int i = 0; i < indexes.Count; i++)
                {
                    Point3D p1, p2;
                    if (i == indexes.Count - 1)
                    {
                        p1 = points[indexes[0]];
                        p2 = points[indexes[i]];
                    }
                    else
                    {
                        p1 = points[indexes[i]];
                        p2 = points[indexes[i + 1]];
                    }

                    _graphics.DrawLine(
                            _pen,
                            p1.X / p1.W + offsetX, p1.Y / p1.W + offsetY,
                            p2.X / p2.W + offsetX, p2.Y / p2.W + offsetY
                            );
                }
            }

            int l = Math.Max(pictureBox1.Height, pictureBox1.Width);
            List<Point3D> Ox = new List<Point3D>() { new Point3D(0, 0, 0), new Point3D(l, 0, 0) };
            List<Point3D> Oy = new List<Point3D>() { new Point3D(0, 0, 0), new Point3D(0, l, 0) };
            List<Point3D> Oz = new List<Point3D>() { new Point3D(0, 0, 0), new Point3D(0, 0, l) };
            List<Color> colors = new List<Color>() { Color.Red, Color.Green, Color.Blue };
            var axeses = new List<List<Point3D>>() { Ox, Oy, Oz };
            for (int i = 0; i < axeses.Count; i++)
            {
                var axes = axeses[i];
                axes[0].ApplyMatrix(MatrixAxonometry);
                axes[1].ApplyMatrix(MatrixAxonometry);

                _graphics.DrawLine(
                                new Pen(colors[i], 2),
                                axes[0].X / axes[0].W + offsetX, axes[0].Y / axes[0].W + offsetY,
                                axes[1].X / axes[1].W + offsetX, axes[1].Y / axes[1].W + offsetY
                                );
            }
        }       

        private new void Scale(float k)
        {
            float[][] MatrixScale = Matrices.Scale(k);

            _modelMatrix = Matrices.MultiplyMatrix(_modelMatrix, MatrixScale);

            DrawPolyhedron();
        }

        public void RotateLine(string input)
        {
            input = input.Trim();
            var parts = input.Split(' ');
            float angle = float.Parse(parts[6]);
            Point3D a = new Point3D(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
            Point3D b = new Point3D(float.Parse(parts[3]), float.Parse(parts[4]), float.Parse(parts[5]));

            b.X -= a.X;
            b.Y -= a.Y;
            b.Z -= a.Z;

            float length = (float)Math.Sqrt(b.X * b.X + b.Y * b.Y + b.Z * b.Z);
            b.X /= length;
            b.Y /= length;
            b.Z /= length;

            float l = b.X;
            float m = b.Y;
            float n = b.Z;

            angle = (float)((angle / 180D) * Math.PI);

            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);

            float[][] rotationMatrix = Matrices.RotationLine(l, m, n, sin, cos);

            _modelMatrix = Matrices.MultiplyMatrix(_modelMatrix, rotationMatrix);

            DrawPolyhedron();
        }

        private void RotateAxis(string input)
        {
            var parts = input.Split(' ');
            string axis = parts[0];
            float angle = float.Parse(parts[1]);

            if (axis == "x")
            {
                XYZRotate(angle, CreateXRotationMatrix);
            }
            else if (axis == "y")
            {
                XYZRotate(angle, CreateYRotationMatrix);
            }
            else if (axis == "z")
            {
                XYZRotate(angle, CreateZRotationMatrix);
            }

            DrawPolyhedron();
        }

        private void XYZRotate(float angle, Func<float, float[][]> createRotationMatrix)
        {
            Point3D center = CalculateCenter(_polyhedron.points);
            Translation(-center.X, -center.Y, -center.Z, false);
            XYZRotatePoint(createRotationMatrix(angle));
            Translation(center.X, center.Y, center.Z, false);
        }

        private Point3D CalculateCenter(List<Point3D> points)
        {
            float x = points.Average(p => p.X);
            float y = points.Average(p => p.Y);
            float z = points.Average(p => p.Z);
            return new Point3D(x, y, z);
        }

        private void XYZRotatePoint(float[][] rotationMatrix)
        {
            _modelMatrix = Matrices.MultiplyMatrix(_modelMatrix, rotationMatrix);
        }

        public float[][] CreateXRotationMatrix(float angle)
        {
            float rad = (float)(angle * Math.PI / 180);
            return Matrices.XRotationMatrix(rad);
        }

        public float[][] CreateYRotationMatrix(float angle)
        {
            float rad = (float)(angle * Math.PI / 180);
            return Matrices.YRotationMatrix(rad);
        }

        public float[][] CreateZRotationMatrix(float angle)
        {
            float rad = (float)(angle * Math.PI / 180);
            return Matrices.ZRotationMatrix(rad);
        }

        public void Translation(float dx, float dy, float dz, bool draw = true)
        {
            float[][] TranslationMatrix = Matrices.Translation(dx, dy, dz);

            _modelMatrix = Matrices.MultiplyMatrix(_modelMatrix, TranslationMatrix);

            if (draw)
                DrawPolyhedron();
        }

        private void Reflect(string plane)
        {
            float[][] reflectionMatrix;

            switch (plane)
            {
                case "XY":
                    reflectionMatrix = Matrices.XYreflectionMatrix();
                    break;
                case "XZ":
                    reflectionMatrix = Matrices.XZreflectionMatrix();
                    break;
                case "YZ":
                    reflectionMatrix = Matrices.YZreflectionMatrix();
                    break;
                default:
                    textBoxOutput.Text = "Неправильная плоскость для отражения.";
                    return;
            }

            _modelMatrix = Matrices.MultiplyMatrix(_modelMatrix, reflectionMatrix);

            DrawPolyhedron();
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            DrawPolyhedron();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            DrawPolyhedron();
        }

        private void comboBoxPolyhedron_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxPolyhedron.SelectedIndex)
            {
                case 0:
                    _polyhedron = new Tetrahedron();
                    break;
                case 1:
                    _polyhedron = new Hexahedron();
                    break;
                case 2:
                    _polyhedron = new Octahedron();
                    break;
                case 3:
                    _polyhedron = new Icosahedron();
                    break;
                case 4:
                    _polyhedron = new Dodecahedron();
                    break;
                case 5:                    
                    _polyhedron = new Parallelepiped();
                    break;
            }
            DrawPolyhedron();
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            switch (_mode)
            {
                case Mode.None: 
                    break;
                case Mode.ScaleRelativeCenter:
                    Scale(float.Parse(textBoxScale.Text, NumberStyles.Float, CultureInfo.InvariantCulture));
                    break;
                case Mode.Translation:
                    string[] parametrs = translationTextBox.Text.Split();
                    Translation(float.Parse(parametrs[0], NumberStyles.Float, CultureInfo.InvariantCulture),
                                float.Parse(parametrs[1], NumberStyles.Float, CultureInfo.InvariantCulture),
                                float.Parse(parametrs[2], NumberStyles.Float, CultureInfo.InvariantCulture));
                    break;
                case Mode.Reflection:
                    Reflect(reflectTextBox.Text.Trim().ToUpper());
                    break;
                case Mode.RotateAxis:
                    RotateAxis(textBoxRotateAxis.Text);
                    break;
                case Mode.RotateLine:
                    RotateLine(textBoxRotateLine.Text);
                    break;
            }
        }

        private void comboBoxAthenian_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxAthenian.SelectedIndex)
            {
                case 0:
                    _mode = Mode.Translation;
                    applyButton.Enabled = true;
                    break;
                case 1:
                    _mode = Mode.RotateLine;
                    applyButton.Enabled = true;
                    break;
                case 2:
                    _mode = Mode.ScaleRelativeCenter;
                    applyButton.Enabled = true;
                    break;
                case 3:
                    _mode = Mode.Reflection;
                    applyButton.Enabled = true;
                    break;
                case 4:
                    _mode = Mode.RotateAxis;
                    applyButton.Enabled = true;
                    break;
            }
        }
        private void textBoxScale_TextChanged(object sender, EventArgs e)
        {
            if (!float.TryParse(textBoxScale.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out float k) || k == 0)
            {
                applyButton.Enabled = false;
                textBoxOutput.Text = "Некорректный ввод коэффиента масштабирования.";
            }
            else
            {
                if (comboBoxAthenian.SelectedIndex != -1)
                {
                    applyButton.Enabled = true;
                }
                textBoxOutput.Text = string.Empty;
            }
        }

        private void textBoxTranslation_TextChanged(object sender, EventArgs e)
        {
            var parts = translationTextBox.Text.Split()
                                               .Select(part => part.Trim())
                                               .ToArray();

            if (parts.Length == 3 &&
                float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float tx) &&
                float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float ty) &&
                float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out float tz))
            {
                if (comboBoxAthenian.SelectedIndex != -1)
                {
                    applyButton.Enabled = true;
                }
                textBoxOutput.Text = string.Empty;
            }
            else
            {
                applyButton.Enabled = false;
                textBoxOutput.Text = "Введите три значения смещения, разделенные пробелом.";
            }
        }

        private void textBoxReflection_TextChanged(object sender, EventArgs e)
        {
            var text = reflectTextBox.Text.Trim().ToUpper();

            if (text == "XY" || text == "XZ" || text == "YZ")
            {
                if (comboBoxAthenian.SelectedIndex != -1)
                {
                    applyButton.Enabled = true;
                }
                textBoxOutput.Text = string.Empty;
            }
            else
            {
                applyButton.Enabled = false;
                textBoxOutput.Text = "Введите значение координатной плоскости (XY/XZ/YZ/xy/xz/yz).";
            }
        }

        private void textBoxRotateAxis_TextChanged(object sender, EventArgs e)
        {
            var parts = textBoxRotateAxis.Text.Split().Select(part => part.Trim())
                                               .ToArray();

            if (parts.Length == 2 &&
                float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float angle))
            {
                applyButton.Enabled = comboBoxAthenian.SelectedIndex != -1;
                textBoxOutput.Text = string.Empty;
                checkBox1.Checked = true;
            }
            else
            {
                applyButton.Enabled = false;
                checkBox1.Checked = false;
                textBoxOutput.Text = "Некорректный ввод оси и угла. Формат: 'ось угол' (например, 'y 4').";
            }

        }

        private void textBoxRotateLine_TextChanged(object sender, EventArgs e)
        {
            var input = textBoxRotateLine.Text.Trim();
            var parts = input.Split(' ');

            if (parts.Length == 7 && 
                parts.All(part => float.TryParse(part, NumberStyles.Float, CultureInfo.InvariantCulture, out _)))
            {
                applyButton.Enabled = comboBoxAthenian.SelectedIndex != -1;
                textBoxOutput.Text = string.Empty;
            }
            else
            {
                applyButton.Enabled = false;
                textBoxOutput.Text = "Некорректный ввод. Формат: 'x1 y1 z1 x2 y2 z2 угол' (например, '10 10 10 40 40 40 45').";
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            InitModelMatrix();
            DrawPolyhedron();
            applyButton.Focus();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            pictureBox1.Width = this.ClientSize.Width - groupBox1.Width - groupBox2.Width - 30;
            pictureBox1.Height = this.ClientSize.Height - 25;
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _graphics = Graphics.FromImage(_bitmap);
            pictureBox1.Image = _bitmap;
            DrawPolyhedron();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "OBJ Files (*.obj)|*.obj";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _polyhedron = new Polyhedron();
                _polyhedron.ParseFromOBJ(openFileDialog.FileName);
                DrawPolyhedron();
            }           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RotateAxis(textBoxRotateAxis.Text);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = checkBox1.Checked;
        }

        private void pictureBox1_OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (pictureBox1.ClientRectangle.Contains(pictureBox1.PointToClient(MousePosition)))
            {
                Scale(e.Delta > 0 ? 1.05f : 0.95f);
            }
        }

        private void rotationFigureTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxRotationFigure.Text, out int k) || k < 0)
            {
                createRotationFigureButton.Enabled = false;
                textBoxOutput.Text = "Некорректный ввод количества разбиений.";
            }
            else
            {
                createRotationFigureButton.Enabled = _points.Count > 2;
                textBoxOutput.Text = string.Empty;
            }
        }

        private void createRotationFigureButton_Click(object sender, EventArgs e)
        {
            _modeRotationFigure = ModeRotationFigure.JarvisMarch;
            JarvisMarch();
            textBoxOutput.Text = "Нажмите на экран или на кнопку \"Очистить\", чтобы снова нарисовать точки.";
            pictureBoxRotationFigure.Invalidate();
        }

        private void pictureBoxRotationFigure_MouseDown(object sender, MouseEventArgs e)
        {
            switch (_modeRotationFigure)
            {
                case ModeRotationFigure.DrawPoints:
                    if (e.Button == MouseButtons.Left)
                    {
                        DrawPoint(e);
                    }
                    break;
                case ModeRotationFigure.JarvisMarch:
                    clearRotationFigureButton_Click(sender, e);
                    break;
                default:
                    break;
            }
        }
        private void DrawPoint(MouseEventArgs e)
        {
            _points.Add(e.Location);
            if (_points.Count > 2)
            {
                createRotationFigureButton.Enabled = true;
            }
            pictureBoxRotationFigure.Invalidate();
        }

        private void JarvisMarch()
        {
            Point currentPoint = _points.OrderBy(p => p.X).First();
            List<Point> passed = new List<Point>
            {
                currentPoint
            };

            while (true)
            {
                Point nextPoint = _points[0];

                foreach (Point point in _points)
                {
                    if (point == currentPoint)
                        continue;

                    Point a = new Point(nextPoint.X - currentPoint.X, nextPoint.Y - currentPoint.Y);
                    Point b = new Point(point.X - currentPoint.X, point.Y - currentPoint.Y);
                    float product = a.X * b.Y - a.Y * b.X;

                    if (nextPoint == currentPoint || product > 0)
                        nextPoint = point;
                }

                currentPoint = nextPoint;

                if (passed[0] == currentPoint)
                    break;

                passed.Add(currentPoint);
            } 

            List<Point3D> points = new List<Point3D>();
            foreach (Point point in passed)
            {
                Point3D newPoint = new Point3D(point);
                points.Add(newPoint);
            }

            Pen pen = new Pen(Color.Black, 1);
            _graphicsRotationFigure.DrawPolygon(pen, passed.ToArray());

            int axis = radioButtonXRotationFigure.Checked ? 0 : radioButtonYRotationFigure.Checked ? 1 : 2;
            _polyhedron = new RotationPolyhedron(points, int.Parse(textBoxRotationFigure.Text, NumberStyles.Integer, CultureInfo.InvariantCulture), axis);

            DrawPolyhedron();
        } 

        private void pictureBoxRotationFigure_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black, 2);
            foreach (Point point in _points)
            {
                e.Graphics.DrawEllipse(pen, point.X, point.Y, 2, 2);
            }
        }

        private void clearRotationFigureButton_Click(object sender, EventArgs e)
        {
            _modeRotationFigure = ModeRotationFigure.DrawPoints;
            _graphicsRotationFigure.Clear(Color.White);
            _points.Clear();
            textBoxOutput.Text = "Нарисуйте больше 1 точки, чтобы найти выпуклую оболочку с помощью алгоритма Джарвиса.";
            createRotationFigureButton.Enabled = false;
            pictureBoxRotationFigure.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    public class Point3D
    {
        public float X, Y, Z, W;
        public Point3D(float _X, float _Y, float _Z)
        {
            X = _X;
            Y = _Y;
            Z = _Z;
            W = 1;
        }

        public Point3D(Point point)
        {
            X = point.X;
            Y = point.Y;
            Z = 0;
            W = 1;
        }

        public void ApplyMatrix(float[][] matrix)
        {
            float[] tempVector = new float[4] { X, Y, Z, W };
            float[] resultVector = new float[4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    resultVector[i] += matrix[j][i] * tempVector[j];
                }                    
            }

            X = resultVector[0];
            Y = resultVector[1];
            Z = resultVector[2];
            W = resultVector[3];
        }
        public Point3D Clone()
        {
            return new Point3D(this.X, this.Y, this.Z) { W = this.W };
        }
    }

    public class Face
    {
        public List<int> indexes;
        public Face(List<int> _indexes)
        {
            indexes = _indexes;
        }
    }

    public class Polyhedron
    {
        public const int EDGE_LENGTH = 200;
        public List<Point3D> points;
        public List<Face> faces;

        public Polyhedron() 
        { 
            points = new List<Point3D>();
            faces = new List<Face>();
        }

        public void ParseFromOBJ(string filePath)
        {
            points.Clear();
            faces.Clear();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (line.StartsWith("v "))
                    {
                        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length == 4 &&
                            float.TryParse(parts[1], out float x) &&
                            float.TryParse(parts[2], out float y) &&
                            float.TryParse(parts[3], out float z))
                        {
                            points.Add(new Point3D(x, y, z));
                        }
                    }
                    else if (line.StartsWith("f "))
                    {
                        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        var indexes = new List<int>();

                        for (int i = 1; i < parts.Length; i++)
                        {
                            string[] faceData = parts[i].Split('/');
                            if (int.TryParse(faceData[0], out int vertexIndex))
                            {
                                // Преобразуем индекс к базированию с нуля и проверяем его
                                int adjustedIndex = vertexIndex - 1;
                                if (adjustedIndex >= 0 && adjustedIndex < points.Count)
                                {
                                    indexes.Add(adjustedIndex);
                                }
                                else
                                {
                                    // Индекс вне диапазона, игнорируем его или выводим сообщение для отладки
                                    Console.WriteLine($"Warning: Ignoring out-of-range vertex index {vertexIndex} in file {filePath}");
                                }
                            }
                        }

                        if (indexes.Count >= 3)
                        {
                            faces.Add(new Face(indexes));
                        }
                    }
                }
            }
        }


    }
    public class Tetrahedron : Polyhedron
    {
        public Tetrahedron()
        {
            Hexahedron icosahedron = new Hexahedron();

            points = new List<Point3D>() {
                icosahedron.points[0].Clone(), icosahedron.points[6].Clone(),
                icosahedron.points[3].Clone(), icosahedron.points[5].Clone(),
            };

            faces = new List<Face>() {
                new Face(new List<int> { 0, 1, 2,}), new Face(new List<int> { 0, 1, 3,}),
                new Face(new List<int> { 0, 2, 3,}), new Face(new List<int> { 1, 2, 3,}),
            };
        }
    }
    public class Hexahedron : Polyhedron
    {
        public Hexahedron() 
        {
            float l = EDGE_LENGTH / 2;

            points = new List<Point3D>() {
                new Point3D(l, l, l), new Point3D(l, l, -l),
                new Point3D(l, -l, l), new Point3D(l, -l, -l),
                new Point3D(-l, l, l), new Point3D(-l, l, -l),
                new Point3D(-l, -l, l), new Point3D(-l, -l, -l),
            };

            faces = new List<Face>() {
                new Face(new List<int> { 0, 1, 3, 2 }), new Face(new List<int> { 0, 1, 5, 4 }),
                new Face(new List<int> { 0, 2, 6, 4 }), new Face(new List<int> { 2, 3, 7, 6 }),
                new Face(new List<int> { 1, 3, 7, 5 }), new Face(new List<int> { 4, 5, 7, 6 }),
            };
        }
    }

    public class Octahedron : Polyhedron
    {
        public Octahedron()
        {
            float l = (float)(EDGE_LENGTH / Math.Sqrt(2));

            points = new List<Point3D>() {
                new Point3D(l, 0, 0), new Point3D(-l, 0, 0),
                new Point3D(0, l, 0), new Point3D(0, -l, 0),
                new Point3D(0, 0, l), new Point3D(0, 0, -l),
            };

            faces = new List<Face>() {
                new Face(new List<int> { 0, 2, 4,}), new Face(new List<int> { 0, 3, 4,}),
                new Face(new List<int> { 0, 2, 5,}), new Face(new List<int> { 0, 3, 5,}),
                new Face(new List<int> { 1, 2, 4,}), new Face(new List<int> { 1, 3, 4,}),
                new Face(new List<int> { 1, 2, 5,}), new Face(new List<int> { 1, 3, 5,}),
            };
        }
    }

    public class Icosahedron : Polyhedron
    {
        public Icosahedron()
        {
            float l = EDGE_LENGTH / 2;
            float phi = (1 + (float)Math.Sqrt(5)) / 2; 

            points = new List<Point3D>()
            {
                new Point3D(-l,  phi * l,  0), new Point3D( l,  phi * l,  0),
                new Point3D(-l, -phi * l,  0), new Point3D( l, -phi * l,  0),
                new Point3D( 0, -l,  phi * l), new Point3D( 0,  l,  phi * l),
                new Point3D( 0, -l, -phi * l), new Point3D( 0,  l, -phi * l),
                new Point3D( phi * l,  0, -l), new Point3D( phi * l,  0,  l),
                new Point3D(-phi * l,  0, -l), new Point3D(-phi * l,  0,  l)
            };

            faces = new List<Face>()
            {
                new Face(new List<int> { 0, 11, 5 }), new Face(new List<int> { 0, 5, 1 }),
                new Face(new List<int> { 0, 1, 7 }), new Face(new List<int> { 0, 7, 10 }),
                new Face(new List<int> { 0, 10, 11 }), new Face(new List<int> { 1, 5, 9 }),
                new Face(new List<int> { 5, 11, 4 }), new Face(new List<int> { 11, 10, 2 }),
                new Face(new List<int> { 10, 7, 6 }), new Face(new List<int> { 7, 1, 8 }),
                new Face(new List<int> { 3, 9, 4 }), new Face(new List<int> { 3, 4, 2 }),
                new Face(new List<int> { 3, 2, 6 }), new Face(new List<int> { 3, 6, 8 }),
                new Face(new List<int> { 3, 8, 9 }), new Face(new List<int> { 4, 9, 5 }),
                new Face(new List<int> { 2, 4, 11 }), new Face(new List<int> { 6, 2, 10 }),
                new Face(new List<int> { 8, 6, 7 }), new Face(new List<int> { 9, 8, 1 })
            };
        }
    }

    public class Parallelepiped : Polyhedron
    {
        public Parallelepiped()
        {
            float l = EDGE_LENGTH / 2;

            points = new List<Point3D>() {
                new Point3D(l, l * 2, l), new Point3D(l, l * 2, -l),
                new Point3D(l, -l, l), new Point3D(l, -l, -l),
                new Point3D(-l, l * 2, l), new Point3D(-l, l * 2, -l),
                new Point3D(-l, -l, l), new Point3D(-l, -l, -l),
            };

            faces = new List<Face>() {
                new Face(new List<int> { 0, 1, 3, 2 }), new Face(new List<int> { 0, 1, 5, 4 }),
                new Face(new List<int> { 0, 2, 6, 4 }), new Face(new List<int> { 2, 3, 7, 6 }),
                new Face(new List<int> { 1, 3, 7, 5 }), new Face(new List<int> { 4, 5, 7, 6 }),
            };
        }
    }

    public class Dodecahedron : Polyhedron
    {
        public Dodecahedron()
        {
            float phi = (1 + (float)Math.Sqrt(5)) / 2;
            float a = EDGE_LENGTH / 2; 
            float b = a / phi;

            points = new List<Point3D>()
            {
                new Point3D( a,  a,  a), new Point3D( a,  a, -a), new Point3D( a, -a,  a), new Point3D( a, -a, -a),
                new Point3D(-a,  a,  a), new Point3D(-a,  a, -a), new Point3D(-a, -a,  a), new Point3D(-a, -a, -a),
                new Point3D( 0,  b,  phi * a), new Point3D( 0,  b, -phi * a), new Point3D( 0, -b,  phi * a), new Point3D( 0, -b, -phi * a),
                new Point3D( b,  phi * a, 0), new Point3D( b, -phi * a, 0), new Point3D(-b,  phi * a, 0), new Point3D(-b, -phi * a, 0),
                new Point3D( phi * a, 0,  b), new Point3D( phi * a, 0, -b), new Point3D(-phi * a, 0,  b), new Point3D(-phi * a, 0, -b)
            };
            faces = new List<Face>()
            {
                    new Face(new List<int> { 15, 7, 11,3, 13,  }),
                    new Face(new List<int> { 0, 8, 4, 14, 12 }), 
                    new Face(new List<int> { 0, 12, 1, 17, 16 }),
                    new Face(new List<int> { 0, 16, 2, 10, 8 }),  
                    new Face(new List<int> { 1, 12, 14, 5, 9 }),  
                    new Face(new List<int> { 4, 14, 5, 19, 18 }), 
                    new Face(new List<int> { 4, 18, 6, 10, 8 }),    
                    new Face(new List<int> { 5, 14, 12, 1, 9 }),     
                    new Face(new List<int> { 5, 9, 11, 7, 19 }),
                    new Face(new List<int> { 2, 16, 17, 3, 13 }),
                    new Face(new List<int> { 1, 12, 14, 5, 9 }),
                    new Face(new List<int> { 6, 18, 19, 7, 15 })    
               };
        }
    }

    public class RotationPolyhedron : Polyhedron
    {
        public RotationPolyhedron(List<Point3D> facePoints, int segments, int axis)
        {
            float phi = 360f / segments;

            float[][] translationMatrix = Matrices.Translation(-facePoints[0].X, -facePoints[0].Y, -facePoints[0].Z);

            float dx = facePoints[facePoints.Count - 1].X - facePoints[0].X;
            float dy = facePoints[facePoints.Count - 1].Y - facePoints[0].Y;
            float dz = facePoints[facePoints.Count - 1].Z - facePoints[0].Z;

            float[][] transformationMatrix;

            if (axis == 0)
            {
                float alpha = (float)Math.Atan2(dy, dx);
                float beta = (float)Math.Atan2(dz, Math.Sqrt(dx * dx + dy * dy));

                float[][] zRotation = Matrices.ZRotationMatrix(-alpha);
                float[][] yRotation = Matrices.YRotationMatrix(beta);

                transformationMatrix = Matrices.MultiplyMatrix(Matrices.MultiplyMatrix(translationMatrix, zRotation), yRotation);
            }
            else if (axis == 1)
            {
                float alpha = (float)Math.Atan2(dx, dy);
                float beta = (float)Math.Atan2(dz, Math.Sqrt(dx * dx + dy * dy));

                float[][] zRotation = Matrices.ZRotationMatrix(alpha);
                float[][] xRotation = Matrices.XRotationMatrix(-beta);

                transformationMatrix = Matrices.MultiplyMatrix(Matrices.MultiplyMatrix(translationMatrix, zRotation), xRotation);
            }
            else
            {
                float alpha = (float)Math.Atan2(dy, dz);
                float beta = (float)Math.Atan2(dx, Math.Sqrt(dy * dy + dz * dz));

                float[][] xRotation = Matrices.XRotationMatrix(-alpha);
                float[][] yRotation = Matrices.YRotationMatrix(beta);

                transformationMatrix = Matrices.MultiplyMatrix(Matrices.MultiplyMatrix(translationMatrix, xRotation), yRotation);
            }

            foreach (Point3D point in facePoints)
            {
                point.ApplyMatrix(transformationMatrix);
            }

            points = new List<Point3D>(facePoints);
            faces = new List<Face>();

            for (int i = 1; i < segments; i++)
            {
                float rad = (float)(i * phi * Math.PI / 180);

                float[][] rotationMatrix;
                if (axis == 0)
                    rotationMatrix = Matrices.XRotationMatrix(rad);
                else if (axis == 1)
                    rotationMatrix = Matrices.YRotationMatrix(rad);
                else
                    rotationMatrix = Matrices.ZRotationMatrix(rad);

                foreach (Point3D point in facePoints)
                {
                    Point3D newPoint = point.Clone();                    
                    newPoint.ApplyMatrix(rotationMatrix);
                    points.Add(newPoint);
                }

                int ind = (i - 1) * facePoints.Count;
                
                for (int j = 0; j < facePoints.Count - 1; j++)
                {
                    List<int> indices = new List<int>
                    {
                        ind + j,
                        ind + j + 1,
                        ind + j + 1 + facePoints.Count,
                        ind + j + facePoints.Count
                    };
                    faces.Add(new Face(indices));
                }
            }
            int indLast = (segments - 1) * facePoints.Count;

            for (int j = 0; j < facePoints.Count - 1; j++)
            {
                List<int> indices = new List<int>
                    {
                        j,
                        j + 1,
                        j + 1 + indLast,
                        j + indLast
                    };
                faces.Add(new Face(indices));
            }
        }
    }

    public static class Matrices
    {
        public static float[][] Translation(float dx, float dy, float dz)
        {
            return new float[][]
            {
                new float[] { 1,  0,  0,  0 },
                new float[] { 0,  1,  0,  0 },
                new float[] { 0,  0,  1,  0 },
                new float[] { dx, dy, dz, 1 }
            };
        }

        public static float[][] XRotationMatrix(float rad)
        {
            return new float[][]
            {
                new float[] { 1, 0, 0, 0 },
                new float[] { 0, (float)Math.Cos(rad), (float)Math.Sin(rad), 0 },
                new float[] { 0, -(float)Math.Sin(rad), (float)Math.Cos(rad), 0 },
                new float[] { 0, 0, 0, 1 }
            };
        }

        public static float[][] YRotationMatrix(float rad)
        {
            return new float[][]
            {
                new float[] { (float)Math.Cos(rad), 0, -(float)Math.Sin(rad), 0 },
                new float[] { 0, 1, 0, 0 },
                new float[] { (float)Math.Sin(rad), 0, (float)Math.Cos(rad), 0 },
                new float[] { 0, 0, 0, 1 }
            };
        }

        public static float[][] ZRotationMatrix(float rad)
        {
            return new float[][]
            {
                new float[] { (float)Math.Cos(rad), (float)Math.Sin(rad), 0, 0 },
                new float[] { -(float)Math.Sin(rad), (float)Math.Cos(rad), 0, 0 },
                new float[] { 0, 0, 1, 0 },
                new float[] { 0, 0, 0, 1 }
            };
        }

        public static float[][] XYreflectionMatrix()
        {
            return new float[4][]
            {
                new float[4] { 1, 0,  0, 0 },
                new float[4] { 0, 1,  0, 0 },
                new float[4] { 0, 0, -1, 0 },
                new float[4] { 0, 0,  0, 1 },
            };
        }

        public static float[][] XZreflectionMatrix()
        {
            return new float[4][]
            {
                new float[4] { 1,  0, 0, 0 },
                new float[4] { 0, -1, 0, 0 },
                new float[4] { 0,  0, 1, 0 },
                new float[4] { 0,  0, 0, 1 },
            };
        }

        public static float[][] YZreflectionMatrix()
        {
            return new float[4][]
            {
                new float[4] { -1, 0, 0, 0 },
                new float[4] { 0,  1, 0, 0 },
                new float[4] { 0,  0, 1, 0 },
                new float[4] { 0,  0, 0, 1 },
            };
        }

        public static float[][] RotationLine(float l, float m, float n, float sin, float cos)
        {
            return new float[4][]
            {
                    new float[4] { l*l + cos*(1 - l*l), l*(1 - cos)*m + n * sin, l*(1 - cos)*n - m * sin, 0},
                    new float[4] { l*(1 - cos)*m - n * sin, m*m + cos*(1 - m*m), m*(1 - cos)*n + l*sin, 0 },
                    new float[4] { l*(1 - cos)*n + m * sin, m*(1 - cos)*n - l*sin, n*n + cos*(1 - n*n), 0 },
                    new float[4] { 0,                       0,                     0,                   1 }
            };
        }

        public static float[][] Scale(float k)
        {
            return new float[4][]
            {
                new float[4] { k, 0, 0, 0 },
                new float[4] { 0, k, 0, 0 },
                new float[4] { 0, 0, k, 0 },
                new float[4] { 0, 0, 0, 1 },
            };
        }

        public static float[][] Perspective(float c)
        {
            return new float[4][]
            {
                new float[4] { 1, 0, 0,    0 },
                new float[4] { 0, 1, 0,    0 },
                new float[4] { 0, 0, 0, -1/c },
                new float[4] { 0, 0, 0,    1 },
            };
        }

        public static float[][] Axonometry(double phi, double psi)
        {
            return new float[4][]
            {
                new float[4] { (float)Math.Cos(psi), (float)Math.Sin(phi) * (float)Math.Sin(psi), 0, 0 },
                new float[4] { 0, (float)Math.Cos(phi), 0, 0 },
                new float[4] { (float)Math.Sin(psi), -(float)Math.Sin(phi) * (float)Math.Cos(psi), 0, 0 },
                new float[4] { 0, 0, 0, 1 },
            };
        }

        public static float[][] MultiplyMatrix(float[][] matrixA, float[][] matrixB)
        {
            float[][] result = new float[4][]
            {
                new float[4] { 0, 0, 0, 0 },
                new float[4] { 0, 0, 0, 0 },
                new float[4] { 0, 0, 0, 0 },
                new float[4] { 0, 0, 0, 0 },
            };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        result[i][j] += matrixA[i][k] * matrixB[k][j];
                    }
                }
            }

            return result;
        }
    }
}
