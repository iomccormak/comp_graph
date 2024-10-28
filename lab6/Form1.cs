using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab6
{    
    public partial class Form1 : Form
    {
        Pen _pen;
        Bitmap _bitmap;
        Graphics _graphics;
        Polyhedron _polyhedron;

        public Form1()
        {
            InitializeComponent();
            _polyhedron = new Hexahedron();
            _pen = new Pen(Color.Black, 1);
            _bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _graphics = Graphics.FromImage(_bitmap);
            pictureBox1.Image = _bitmap;
            radioButton1.Checked = true;
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
            float c = -pictureBox1.Width * 0.8f;
            float offsetX = pictureBox1.Width / 2;
            float offsetY = pictureBox1.Height / 2;

            float[][] MatrixPerspective = new float[4][]
            {
                new float[4] { 1, 0, 0,    0 },
                new float[4] { 0, 1, 0,    0 },
                new float[4] { 0, 0, 0, -1/c },
                new float[4] { 0, 0, 0,    1 },
            };

            foreach (var edge in _polyhedron.edges)
            {
                var p1 = _polyhedron.points[edge.Item1].Clone();
                var p2 = _polyhedron.points[edge.Item2].Clone();

                p1.ApplyMatrix(MatrixPerspective);
                p2.ApplyMatrix(MatrixPerspective);

                _graphics.DrawLine(
                    _pen,
                    p1.X / p1.W + offsetX, p1.Y / p1.W + offsetY,
                    p2.X / p2.W + offsetX, p2.Y / p2.W + offsetY
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

            foreach (var edge in _polyhedron.edges)
            {
                var p1 = _polyhedron.points[edge.Item1].Clone();
                var p2 = _polyhedron.points[edge.Item2].Clone();

                p1.ApplyMatrix(MatrixAxonometry);
                p2.ApplyMatrix(MatrixAxonometry);

                _graphics.DrawLine(
                    _pen,
                    p1.X / p1.W + offsetX, p1.Y / p1.W + offsetY,
                    p2.X / p2.W + offsetX, p2.Y / p2.W + offsetY
                    );
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            DrawPolyhedron();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            DrawPolyhedron();
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

    public class Polyhedron
    {
        public const int EDGE_LENGHT = 100;
        public List<Point3D> points;
        public List<Tuple<int, int>> edges;

        public Polyhedron() 
        { 
            points = new List<Point3D>();
            edges = new List<Tuple<int, int>>();
        }

        public void AddEdge(Point3D point1, Point3D point2)
        {
            points.Add(point1);
            points.Add(point2);
            edges.Add(new Tuple<int, int>( points.Count - 2, points.Count - 1));
        }       
    }

    public class Hexahedron : Polyhedron
    {
        public Hexahedron() 
        {
            float l = EDGE_LENGHT / 2;

            List<Point3D> points = new List<Point3D>() {
                new Point3D(l, l, l), new Point3D(l, l, -l),
                new Point3D(l, -l, l), new Point3D(l, -l, -l),
                new Point3D(-l, l, l), new Point3D(-l, l, -l),
                new Point3D(-l, -l, l), new Point3D(-l, -l, -l),
            };

            List<Tuple<int, int>> edges = new List<Tuple<int, int>>() {
                new Tuple<int, int>( 0, 1 ), new Tuple<int, int>( 0, 2 ),
                new Tuple<int, int>( 0, 4 ), new Tuple<int, int>( 1, 3 ),
                new Tuple<int, int>( 1, 5 ), new Tuple<int, int>( 2, 3 ),
                new Tuple<int, int>( 2, 6 ), new Tuple<int, int>( 3, 7 ),
                new Tuple<int, int>( 4, 5 ), new Tuple<int, int>( 4, 6 ),
                new Tuple<int, int>( 5, 7 ), new Tuple<int, int>( 6, 7 ),
            };

            foreach (var edge in edges)
            {
                AddEdge(points[edge.Item1], points[edge.Item2]);
            }
        }
    }
}
