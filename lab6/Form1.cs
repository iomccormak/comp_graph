using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace lab6
{    
    public partial class Form1 : Form
    {
        Pen _pen;
        Bitmap _bitmap;
        Graphics _graphics;
        Polyhedron _polyhedron;
        float[][] _modelMatrix;
        Mode _mode;

        enum Mode
        {
            None,
            Translation,
            Reflection,
            ScaleRelativeCenter,
            RotateAxis,
            RotateLine,
        }

        public Form1()
        {
            InitializeComponent();
            InitModelMatrix();
            _polyhedron = new Hexahedron();
            _pen = new Pen(Color.Black, 1);
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _graphics = Graphics.FromImage(_bitmap);
            pictureBox1.Image = _bitmap;
            radioButton1.Checked = true;
            applyButton.Enabled = false;
            comboBoxPolyhedron.SelectedIndex = 4;
            _mode = Mode.None;
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

            float[][] MatrixPerspective = new float[4][]
            {
                new float[4] { 1, 0, 0,    0 },
                new float[4] { 0, 1, 0,    0 },
                new float[4] { 0, 0, 0, -1/c },
                new float[4] { 0, 0, 0,    1 },
            };

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

            List<Point3D> Ox = new List<Point3D>() { new Point3D(0, 0, 0), new Point3D(1080, 0, 0) };
            List<Point3D> Oy = new List<Point3D>() { new Point3D(0, 0, 0), new Point3D(0, 1080, 0) };
            List<Point3D> Oz = new List<Point3D>() { new Point3D(0, 0, 0), new Point3D(0, 0, 1080) };
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

            float[][] MatrixAxonometry = new float[4][]
            {
                new float[4] { (float)Math.Cos(psi),  (float)Math.Sin(phi) * (float)Math.Sin(psi), 0, 0 },
                new float[4] {                    0,                         (float)Math.Cos(phi), 0, 0 },
                new float[4] { (float)Math.Sin(psi), -(float)Math.Sin(phi) * (float)Math.Cos(psi), 0, 0 },
                new float[4] {                    0,                                            0, 0, 1 },
            };

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

            List<Point3D> Ox = new List<Point3D>() { new Point3D(0, 0, 0), new Point3D(1080, 0, 0) };
            List<Point3D> Oy = new List<Point3D>() { new Point3D(0, 0, 0), new Point3D(0, 1080, 0) };
            List<Point3D> Oz = new List<Point3D>() { new Point3D(0, 0, 0), new Point3D(0, 0, 1080) };
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

        private new void Scale(float k)
        {
            float[][] MatrixScale = new float[4][]
            {
                new float[4] { k, 0, 0, 0 },
                new float[4] { 0, k, 0, 0 },
                new float[4] { 0, 0, k, 0 },
                new float[4] { 0, 0, 0, 1 },
            };

            _modelMatrix = MultiplyMatrix(_modelMatrix, MatrixScale);

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

            float[][] rotationMatrix = new float[4][]
            {
                    new float[4] { l*l + cos*(1 - l*l), l*(1 - cos)*m + n * sin, l*(1 - cos)*n - m * sin, 0},
                    new float[4] { l*(1 - cos)*m - n * sin, m*m + cos*(1 - m*m), m*(1 - cos)*n + l*sin, 0 },
                    new float[4] { l*(1 - cos)*n + m * sin, m*(1 - cos)*n - l*sin, n*n + cos*(1 - n*n), 0 },
                    new float[4] { 0,                       0,                     0,                   1 }
            };

            _modelMatrix = MultiplyMatrix(_modelMatrix, rotationMatrix);

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
            _modelMatrix = MultiplyMatrix(_modelMatrix, rotationMatrix);
        }

        private float[][] CreateXRotationMatrix(float angle)
        {
            float rad = (float)(angle * Math.PI / 180);
            return new float[][]
            {
                new float[] { 1, 0, 0, 0 },
                new float[] { 0, (float)Math.Cos(rad), (float)Math.Sin(rad), 0 },
                new float[] { 0, -(float)Math.Sin(rad), (float)Math.Cos(rad), 0 },
                new float[] { 0, 0, 0, 1 }
            };
        }

        private float[][] CreateYRotationMatrix(float angle)
        {
            float rad = (float)(angle * Math.PI / 180);
            return new float[][]
            {
                new float[] { (float)Math.Cos(rad), 0, -(float)Math.Sin(rad), 0 },
                new float[] { 0, 1, 0, 0 },
                new float[] { (float)Math.Sin(rad), 0, (float)Math.Cos(rad), 0 },
                new float[] { 0, 0, 0, 1 }
            };
        }

        private float[][] CreateZRotationMatrix(float angle)
        {
            float rad = (float)(angle * Math.PI / 180);
            return new float[][]
            {
                new float[] { (float)Math.Cos(rad), (float)Math.Sin(rad), 0, 0 },
                new float[] { -(float)Math.Sin(rad), (float)Math.Cos(rad), 0, 0 },
                new float[] { 0, 0, 1, 0 },
                new float[] { 0, 0, 0, 1 }
            };
        }


        public void Translation(float dx, float dy, float dz, bool draw = true)
        {
            float[][] TranslationMatrix = new float[4][]
            {
                    new float[4] { 1,  0,  0,  0 },
                    new float[4] { 0,  1,  0,  0 },
                    new float[4] { 0,  0,  1,  0 },
                    new float[4] { dx, dy, dz, 1 }
            };

            _modelMatrix = MultiplyMatrix(_modelMatrix, TranslationMatrix);

            if (draw)
                DrawPolyhedron();
        }

        private void Reflect(string plane)
        {
            float[][] reflectionMatrix;

            switch (plane)
            {
                case "XY":
                    reflectionMatrix = new float[4][]
                    {
                new float[4] { 1, 0,  0, 0 },
                new float[4] { 0, 1,  0, 0 },
                new float[4] { 0, 0, -1, 0 },
                new float[4] { 0, 0,  0, 1 },
                    };
                    break;
                case "XZ":
                    reflectionMatrix = new float[4][]
                    {
                new float[4] { 1,  0, 0, 0 },
                new float[4] { 0, -1, 0, 0 },
                new float[4] { 0,  0, 1, 0 },
                new float[4] { 0,  0, 0, 1 },
                    };
                    break;
                case "YZ":
                    reflectionMatrix = new float[4][]
                    {
                new float[4] { -1, 0, 0, 0 },
                new float[4] { 0,  1, 0, 0 },
                new float[4] { 0,  0, 1, 0 },
                new float[4] { 0,  0, 0, 1 },
                    };
                    break;
                default:
                    textBoxOutput.Text = "Неправильная плоскость для отражения.";
                    return;
            }

            _modelMatrix = MultiplyMatrix(_modelMatrix, reflectionMatrix);

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
                    Scale(float.Parse(textBoxScale.Text));
                    break;
                case Mode.Translation:
                    string[] parametrs = translationTextBox.Text.Split();
                    Translation(float.Parse(parametrs[0]), float.Parse(parametrs[1]), float.Parse(parametrs[2]));
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
            if (!float.TryParse(textBoxScale.Text, out float k) || k == 0)
            {
                applyButton.Enabled = false;
                textBoxOutput.Text = "Неккоректный ввод коэффиента масштабирования.";
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
                float.TryParse(parts[0], out float tx) &&
                float.TryParse(parts[1], out float ty) &&
                float.TryParse(parts[2], out float tz))
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
                float.TryParse(parts[1], out float angle))
            {
                applyButton.Enabled = comboBoxAthenian.SelectedIndex != -1;
                textBoxOutput.Text = string.Empty;
            }
            else
            {
                applyButton.Enabled = false;
                textBoxOutput.Text = "Некорректный ввод оси и угла. Формат: 'ось угол' (например, 'y 4').";
            }

        }

        private void textBoxRotateLine_TextChanged(object sender, EventArgs e)
        {
            var input = textBoxRotateLine.Text.Trim();
            var parts = input.Split(' ');

            if (parts.Length == 7 && parts.All(part => float.TryParse(part, out _)))
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

        private void button1_Click(object sender, EventArgs e)
        {
            InitModelMatrix();
            DrawPolyhedron();
            applyButton.Focus();
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
        public const int EDGE_LENGTH = 150;
        public List<Point3D> points;
        public List<Face> faces;

        public Polyhedron() 
        { 
            points = new List<Point3D>();
            faces = new List<Face>();
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
            float a = 100; 
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





}
