using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace lab8
{
    public class Camera
    {
        public Point3D Position {  get; set; }
        public Point3D Target { get; set; }
        public float[][] ViewMatrix { get; set; }
        public float[][] ProjectionMatrix { get; set; }

        private Point3D right = new Point3D(1, 0, 0);
        private Point3D up = new Point3D(0, 1, 0);
        private Point3D forward = new Point3D(0, 0, 1);

        private Graphics _graphics;
        private Pen _pen;

        public Camera(Graphics graphics, Pen pen)
        {
            Position = new Point3D(0, 0, 1000);
            Target = new Point3D(0, 0, 0);
            ViewMatrix = Matrices.Identity();
            _pen = pen;
            _graphics = graphics;
        }

        public void DrawScene(Graphics graphics, List<Polyhedron> polyhedrons, ModeView modeView, bool checkBoxNonFrontFaces, bool zBuffer, int width, int height, bool faceColor)
        {
            float c = -Position.Z;
            //double phi = 35.26d;
            //double psi = 45d;
            _graphics = graphics;
            _graphics.Clear(Color.White);

            
            float[,] zBufferArray = new float[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    zBufferArray[i, j] = float.MaxValue;
                }
            }

            switch (modeView)
            {
                case ModeView.Perspective:
                    ProjectionMatrix = Matrices.Perspective(c); // Не работает
                    break;
                case ModeView.Axonometry:
                    //ProjectionMatrix = Matrices.Axonometry(phi, psi);
                    ProjectionMatrix = Matrices.Perspective(c); // А это вообще заглушка
                    break;
                case ModeView.Parallel:
                    ProjectionMatrix = Matrices.Parallel();
                    break;
            }

            foreach (Polyhedron polyhedron in polyhedrons)
            {
                DrawPolyhedron(polyhedron, checkBoxNonFrontFaces, zBuffer, zBufferArray, width, height, faceColor);
            }
            DrawAxis();
        }

        private void DrawAxis()
        {
            int l = 100;
            List<Point3D> Ox = new List<Point3D>() { new Point3D(0, 0, 0), new Point3D(l, 0, 0) };
            List<Point3D> Oy = new List<Point3D>() { new Point3D(0, 0, 0), new Point3D(0, l, 0) };
            List<Point3D> Oz = new List<Point3D>() { new Point3D(0, 0, 0), new Point3D(0, 0, l) };
            List<Color> colors = new List<Color>() { Color.Red, Color.Green, Color.Blue };
            var axeses = new List<List<Point3D>>() { Ox, Oy, Oz };
            for (int i = 0; i < axeses.Count; i++)
            {
                var axes = axeses[i];
                foreach (var point in axes)
                {
                    point.ApplyMatrix(ViewMatrix);
                    point.ApplyMatrix(ProjectionMatrix);
                }

                _graphics.DrawLine(
                    new Pen(colors[i], 1),
                    axes[0].X / axes[0].W, axes[0].Y / axes[0].W,
                    axes[1].X / axes[1].W, axes[1].Y / axes[1].W
                );
            }
        }


        private void DrawPolyhedron(Polyhedron polyhedron, bool checkBoxNonFrontFaces, bool zBuffer, float[,] zBufferArray, int width, int height, bool faceColor)
        {
            List<Point3D> points = new List<Point3D>();

            List<Face> visibleFaces = polyhedron.faces;
            if (checkBoxNonFrontFaces)
            {
                var viewVector = Target - Position;
                viewVector.Normalize();
                visibleFaces = polyhedron.GetVisibleFaces(viewVector);
            }

            foreach (Point3D point in polyhedron.points)
            {
                Point3D p = point.Clone();
                p.ApplyMatrix(polyhedron.modelMatrix);
                p.ApplyMatrix(ViewMatrix);
                points.Add(p);
            }

            if (zBuffer)
            {
                foreach (var face in polyhedron.faces)
                {
                    var facePoints = face.indexes.Select(i => points[i]).ToList();

                    var screenPoints = facePoints.Select(p =>
                        new Point3D(p.X + width / 2, p.Y + height / 2, p.Z)
                    ).ToList();

                    var triangles = Triangulate(screenPoints);

                    foreach (var triangle in triangles)
                    {
                        RasterizeTriangle(triangle, facePoints, zBufferArray, face.faceColor, width, height);
                    }
                }
            }

            UpdateViewMatrix();

            foreach (Point3D point in points)
            {
                point.ApplyMatrix(ProjectionMatrix);
            }

            if (checkBoxNonFrontFaces)
            {
                foreach (var face in visibleFaces)
                {
                    var indexes = face.indexes;
                    var polygonPoints = new PointF[face.indexes.Count];
                    for (int i = 0; i < face.indexes.Count; i++)
                    {
                        var point = points[face.indexes[i]];
                        polygonPoints[i] = new PointF(point.X / point.W, point.Y / point.W);
                    }

                    using (Brush brush = new SolidBrush(face.faceColor))
                    {
                        _graphics.FillPolygon(brush, polygonPoints);
                    }

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
                            p1.X / p1.W, p1.Y / p1.W,
                            p2.X / p2.W, p2.Y / p2.W
                        );
                    }
                }
            }
                
            
        }

        private static List<List<Point3D>> Triangulate(List<Point3D> polygonPoints)
        {
            List<List<Point3D>> triangles = new List<List<Point3D>>();
            for (int i = 2; i < polygonPoints.Count; i++)
            {
                triangles.Add(new List<Point3D>
                {
                    polygonPoints[0],
                    polygonPoints[i - 1],
                    polygonPoints[i]
                });
            }

            return triangles;
        }


        private void RasterizeTriangle(List<Point3D> triangle, List<Point3D> facePoints, float[,] zBufferArray, Color color, int width, int height)
        {
            var sortedPoints = triangle.OrderBy(p => p.Y).ToList();
            var top = sortedPoints[0];
            var mid = sortedPoints[1];
            var bottom = sortedPoints[2];

            for (float y = top.Y; y <= mid.Y; y += 0.5f)
            {
                float x1 = FindXbyY(y, top, mid);
                float z1 = FindZbyY(y, top, mid);

                float x2 = FindXbyY(y, top, bottom);
                float z2 = FindZbyY(y, top, bottom);

                FillScanline((int)y, x1, z1, x2, z2, zBufferArray, color, width, height);
            }

            for (float y = mid.Y; y <= bottom.Y; y += 0.5f)
            {
                float x1 = FindXbyY(y, mid, bottom);
                float z1 = FindZbyY(y, mid, bottom);

                float x2 = FindXbyY(y, top, bottom);
                float z2 = FindZbyY(y, top, bottom);

                FillScanline((int)y, x1, z1, x2, z2, zBufferArray, color, width, height);
            }
        }

        private float FindXbyY(float y, Point3D p1, Point3D p2)
        {
            if (p1.Y == p2.Y) return p1.X;
            return p1.X + (p2.X - p1.X) * (y - p1.Y) / (p2.Y - p1.Y);
        }


        private float FindZbyY(float y, Point3D p1, Point3D p2)
        {
            if (p1.Y == p2.Y) return p1.Z;
            return p1.Z + (p2.Z - p1.Z) * (y - p1.Y) / (p2.Y - p1.Y);
        }


        private void FillScanline(int y, float x1, float z1, float x2, float z2, float[,] zBufferArray, Color color, int width, int height)
        {
            if (x1 > x2)
            {
                (x1, x2) = (x2, x1);
                (z1, z2) = (z2, z1);
            }

            for (float x = x1; x <= x2; x += 0.5f)
            {
                float z = z1 + (z2 - z1) * (x - x1) / (x2 - x1);
                int ix = (int)x, iy = y;

                if (ix >= 0 && ix < width && iy >= 0 && iy < height && z < zBufferArray[ix, iy])
                {
                    zBufferArray[ix, iy] = z;
                    _graphics.FillRectangle(new SolidBrush(color), ix - width / 2, iy - height / 2, 1, 1);
                }
            }
        }



        private void UpdateViewMatrix()
        {
            forward = Target - Position;
            forward.Normalize();
            right = forward.CrossProduct(up);
            right.Normalize();
            up = right.CrossProduct(forward);
            up.Normalize();

            ViewMatrix = new float[][] {
                new float[] { right.X, up.X, forward.X, 0 },
                new float[] { right.Y, up.Y, forward.Y, 0 },
                new float[] { right.Z, up.Z, forward.Z, 0 },
                new float[] { -right.DotProduct(Position), -up.DotProduct(Position), -forward.DotProduct(Position), 1 }
            };
        }

        public void Move(float dx, float dy, float dz)
        {
            Position.ApplyMatrix(Matrices.Translation(dx, dy, dz));
        }

        public void Rotate(float angleX, float angleY)
        {
            float radX = (float)(angleX * Math.PI / 180);
            float radY = (float)(angleY * Math.PI / 180);

            float sin;
            float cos;
            Point3D point;

            if (radX != 0)
            {
                point = right;
                sin = (float)Math.Sin(radX);
                cos = (float)Math.Cos(radX);
            } 
            else
            {
                point = up;
                sin = (float)Math.Sin(radY);
                cos = (float)Math.Cos(radY);
            }

            float length = (float)Math.Sqrt(point.X * point.X + point.Y * point.Y + point.Z * point.Z);
            float l = point.X / length;
            float m = point.Y / length;
            float n = point.Z / length;
            float[][] rotation = Matrices.RotationLine(l, m, n, sin, cos);
            Position.ApplyMatrix(rotation);
        }

        public void MoveUp() => Move(0, -30, 0);
        public void MoveDown() => Move(0, 30, 0);
        public void MoveRight() => Move(30, 0, 0);
        public void MoveLeft() => Move(-30, 0, 0);
        public void MoveForward() => Move(0, 0, 30);
        public void MoveBackward() => Move(0, 0, -30);
        public void RotateUp() => Rotate(10, 0);
        public void RotateDown() => Rotate(-10, 0);
        public void RotateRight() => Rotate(0, 10);
        public void RotateLeft() => Rotate(0, -10);
    }
}