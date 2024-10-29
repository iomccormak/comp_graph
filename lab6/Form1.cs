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
        Mode _mode;

        enum Mode
        {
            None,
            Translation,
            Reflection,
            ScaleRelativeCenter,
        }

        public Form1()
        {
            InitializeComponent();
            _polyhedron = new Hexahedron();
            _pen = new Pen(Color.Black, 1);
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _graphics = Graphics.FromImage(_bitmap);
            pictureBox1.Image = _bitmap;
            radioButton1.Checked = true;
            applyButton.Enabled = false;
            comboBoxPolyhedron.SelectedIndex = 0;
            _mode = Mode.None;
            DrawPolyhedron();
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

            foreach (var point in _polyhedron.points)
            {
                point.ApplyMatrix(MatrixScale);
            }

            DrawPolyhedron();
        }

        private void Translation(float tx, float ty, float tz)
        {
            float[][] MatrixTranslation = new float[4][]
            {
                new float[4] { 1, 0, 0, 0 },
                new float[4] { 0, 1, 0, 0 },
                new float[4] { 0, 0, 1, 0 },
                new float[4] { tx, ty, tz, 1 },
            };

            foreach (var point in _polyhedron.points)
            {
                point.ApplyMatrix(MatrixTranslation);
            }

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

            foreach (var point in _polyhedron.points)
            {
                point.ApplyMatrix(reflectionMatrix);
            }

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
                    _polyhedron = new Icosahedron();
                    break;
            }
            DrawPolyhedron();
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
                    applyButton.Enabled = false;
                    break;
                case 2:
                    _mode = Mode.ScaleRelativeCenter;
                    applyButton.Enabled = true;
                    break;
                case 3:
                    _mode = Mode.Reflection;
                    applyButton.Enabled = true;
                    break;
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
        public const int EDGE_LENGHT = 150;
        public List<Point3D> points;
        public List<Face> faces;
        public List<Tuple<int, int>> edges;

        public Polyhedron() 
        { 
            points = new List<Point3D>();
            faces = new List<Face>();
            edges = new List<Tuple<int, int>>();
        }

        public void AddEdge(Point3D point1, Point3D point2)
        {
            points.Add(point1);
            points.Add(point2);
            edges.Add(new Tuple<int, int>( points.Count - 2, points.Count - 1));
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
            float l = EDGE_LENGHT / 2;

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
            float l = (float)(EDGE_LENGHT / Math.Sqrt(2));

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
            float l = EDGE_LENGHT / 2;
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

    public class Dodecahedron : Polyhedron
    {
        public Dodecahedron()
        {
            Icosahedron icosahedron = new Icosahedron();
            List<Point3D> dodecahedronPoints = new List<Point3D>();

            foreach (var face in icosahedron.faces)
            {
                Point3D p1 = icosahedron.points[face.indexes[0]];
                Point3D p2 = icosahedron.points[face.indexes[1]];
                Point3D p3 = icosahedron.points[face.indexes[2]];

                float centerX = (p1.X + p2.X + p3.X) / 3;
                float centerY = (p1.Y + p2.Y + p3.Y) / 3;
                float centerZ = (p1.Z + p2.Z + p3.Z) / 3;

                dodecahedronPoints.Add(new Point3D(centerX, centerY, centerZ));
            }

            points = dodecahedronPoints;

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
}
