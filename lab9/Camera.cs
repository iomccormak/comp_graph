using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;

namespace lab9
{
    public class Camera
    {
        public bool IsColored { get; set; }
        public bool IsNonFrontFaces { get; set; }
        public bool IsZBuffer { get; set; }
        public bool IsLighting { get; set; }
        public bool IsTextured { get; set; }

        private Point3D _position;
        private Point3D _target;
        private Point3D _lightDirection = new Point3D(200, 200, -200);
        private Point3D _right = new Point3D(1, 0, 0);
        private Point3D _up = new Point3D(0, 1, 0);
        private Point3D _forward = new Point3D(0, 0, 1);
        private float[][] _viewMatrix;
        private float[][] _projectionMatrix;

        private Graphics _graphics;
        
        public Camera(Graphics graphics)
        {
            _position = new Point3D(0, 0, 1000);
            _target = new Point3D(0, 0, 0);
            _viewMatrix = Matrices.Identity();
            IsColored = false;
            IsNonFrontFaces = false;
            IsZBuffer = false;
            IsLighting = false;
            _graphics = graphics;
        }

        public void DrawScene(Graphics graphics, List<Polyhedron> polyhedrons, ModeView modeView, int width, int height, Bitmap texture)
        {
            float c = -_position.Z;
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
                    _projectionMatrix = Matrices.Perspective(c); // Не работает
                    break;
                case ModeView.Axonometric:
                    //ProjectionMatrix = Matrices.Axonometry(phi, psi);
                    _projectionMatrix = Matrices.Perspective(c); // А это вообще заглушка
                    break;
                case ModeView.Parallel:
                    _projectionMatrix = Matrices.Parallel();
                    break;
            }

            foreach (Polyhedron polyhedron in polyhedrons)
            {
                DrawPolyhedron(polyhedron, zBufferArray, width, height, texture);
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
                    point.ApplyMatrix(_viewMatrix);
                    point.ApplyMatrix(_projectionMatrix);
                }

                _graphics.DrawLine(
                    new Pen(colors[i], 1),
                    axes[0].X / axes[0].W, axes[0].Y / axes[0].W,
                    axes[1].X / axes[1].W, axes[1].Y / axes[1].W
                );
            }
        }

        private void DrawPolyhedron(Polyhedron polyhedron, float[,] zBufferArray, int width, int height, Bitmap texture)
        {
            List<Point3D> points = new List<Point3D>();
            
            UpdateViewMatrix();
            RecalculateNormals(polyhedron.faces, polyhedron.points);
            
            List<Face> visibleFaces = polyhedron.faces;
            if (IsNonFrontFaces)
            {
                var viewVector = _target - _position;
                viewVector.Normalize();
                visibleFaces = polyhedron.GetVisibleFaces(viewVector);
            }

            foreach (Point3D point in polyhedron.points)
            {
                Point3D p = point.Clone();
                p.ApplyMatrix(polyhedron.modelMatrix);
                p.ApplyMatrix(_viewMatrix);
                points.Add(p);
            }

            if (IsZBuffer)
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
                        RasterizeTriangle(triangle, zBufferArray, face.color, width, height);
                    }
                }
            }

            foreach (Point3D point in points)
            {
                point.ApplyMatrix(_projectionMatrix);
            }
            
            RecalculateNormals(visibleFaces, points);

            if (IsNonFrontFaces)
            {
                foreach (var face in visibleFaces)
                {
                    if (IsColored)
                    {
                        PhongLighting.ShadePhongLighting(_graphics, face, points, _lightDirection, _target - _position);
                    }
                    else
                    {
                        var indexes = face.indexes;
                        var polygonPoints = new PointF[face.indexes.Count];
                        for (int i = 0; i < face.indexes.Count; i++)
                        {
                            var point = points[face.indexes[i]];
                            polygonPoints[i] = new PointF(point.X / point.W, point.Y / point.W);
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
                                new Pen(Color.Black, 1),
                                p1.X / p1.W, p1.Y / p1.W,
                                p2.X / p2.W, p2.Y / p2.W
                            );
                        }
                    }
                }
            } 
        }
        
        public void RecalculateNormals(List<Face> faces, List<Point3D> points)
        {
            foreach (var face in faces)
            {
                face.CalculateNormal(points);
            }
        }

        private void RasterizeTexturedTriangle(List<Point3D> triangle, float[,] zBufferArray, int width, int height, Bitmap texture, List<Coordinates> coordinates)
        {
            // Получение вершин треугольника
            var p0 = triangle[0];
            var p1 = triangle[1];
            var p2 = triangle[2];

            // Получение UV-координат для треугольника из списка coordinates
            var uv0 = coordinates[triangle.IndexOf(p0)];
            var uv1 = coordinates[triangle.IndexOf(p1)];
            var uv2 = coordinates[triangle.IndexOf(p2)];

            // Вычисление границ треугольника
            int minX = (int)Math.Max(0, Math.Min(p0.X, Math.Min(p1.X, p2.X)));
            int maxX = (int)Math.Min(width - 1, Math.Max(p0.X, Math.Max(p1.X, p2.X)));
            int minY = (int)Math.Max(0, Math.Min(p0.Y, Math.Min(p1.Y, p2.Y)));
            int maxY = (int)Math.Min(height - 1, Math.Max(p0.Y, Math.Max(p1.Y, p2.Y)));

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    // Вычисление барицентрических координат для текущего пикселя
                    float u, v, w;
                    if (CalculateBarycentricCoordinates(p0, p1, p2, x, y, out u, out v, out w))
                    {
                        // Вычисление z-координаты пикселя
                        float z = u * p0.Z + v * p1.Z + w * p2.Z;

                        // Проверка глубины
                        if (z < zBufferArray[x, y])
                        {
                            zBufferArray[x, y] = z;

                            // Интерполяция UV-координат для текущего пикселя
                            float interpolatedU = u * uv0.U + v * uv1.U + w * uv2.U;
                            float interpolatedV = u * uv0.V + v * uv1.V + w * uv2.V;

                            // Выборка цвета из текстуры с учетом интерполированных UV-координат
                            int texX = Math.Max(0, Math.Min(texture.Width - 1, (int)(interpolatedU * texture.Width)));
                            int texY = Math.Max(0, Math.Min(texture.Height - 1, (int)(interpolatedV * texture.Height)));

                            // Отрисовка пикселя с цветом из текстуры
                            using (Brush brush = new SolidBrush(texture.GetPixel(texX, texY)))
                            {
                                _graphics.FillRectangle(brush, x - width / 2, y - height / 2, 1, 1);
                            }
                        }
                    }
                }
            }
        }



        // Функция для вычисления UV-координат вершин
        private List<(float u, float v)> CalculateUVForFace(List<Point3D> vertices)
        {
            // Определяем границы проекции на плоскость XY
            float minX = vertices.Min(v => v.X);
            float maxX = vertices.Max(v => v.X);
            float minY = vertices.Min(v => v.Y);
            float maxY = vertices.Max(v => v.Y);

            var uvCoordinates = new List<(float u, float v)>();

            foreach (var vertex in vertices)
            {
                // Нормализуем координаты
                float u = (vertex.X - minX) / (maxX - minX);
                float v = (vertex.Y - minY) / (maxY - minY);

                uvCoordinates.Add((u, v));
            }

            return uvCoordinates;
        }


        // Пример функции для вычисления барицентрических координат
        private bool CalculateBarycentricCoordinates(Point3D p0, Point3D p1, Point3D p2, int x, int y, out float u, out float v, out float w)
        {
            float det = (p1.Y - p2.Y) * (p0.X - p2.X) + (p2.X - p1.X) * (p0.Y - p2.Y);
            if (Math.Abs(det) < 1e-5)
            {
                u = v = w = 0;
                return false;
            }

            u = ((p1.Y - p2.Y) * (x - p2.X) + (p2.X - p1.X) * (y - p2.Y)) / det;
            v = ((p2.Y - p0.Y) * (x - p2.X) + (p0.X - p2.X) * (y - p2.Y)) / det;
            w = 1 - u - v;

            return u >= 0 && v >= 0 && w >= 0;
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

        private void RasterizeTriangle(List<Point3D> triangle, float[,] zBufferArray, Color color, int width, int height)
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
            _forward = _target - _position;
            _forward.Normalize();
            _right = _forward.CrossProduct(_up);
            _right.Normalize();
            _up = _right.CrossProduct(_forward);
            _up.Normalize();

            _viewMatrix = new float[][] {
                new float[] { _right.X, _up.X, _forward.X, 0 },
                new float[] { _right.Y, _up.Y, _forward.Y, 0 },
                new float[] { _right.Z, _up.Z, _forward.Z, 0 },
                new float[] { -_right.DotProduct(_position), -_up.DotProduct(_position), -_forward.DotProduct(_position), 1 }
            };
        }

        private void Move(float dx, float dy, float dz)
        {
            if (IsLighting)
                _lightDirection += new Point3D(dx, dy, dz);
            else
                _position += new Point3D(dx, dy, dz);
        }

        private void Rotate(float angleX, float angleY)
        {
            float radX = (float)(angleX * Math.PI / 180);
            float radY = (float)(angleY * Math.PI / 180);

            if (radX != 0)
            {
                ApplyRotation(_right, radX);
            }
            if (radY != 0)
            {
                ApplyRotation(_up, radY);
            }
        }

        private void ApplyRotation(Point3D axis, float radians)
        {
            float sin = (float)Math.Sin(radians);
            float cos = (float)Math.Cos(radians);
    
            float length = axis.Length();
            float l = axis.X / length;
            float m = axis.Y / length;
            float n = axis.Z / length;

            float[][] rotation = Matrices.RotationLine(l, m, n, sin, cos);

            if (IsLighting)
                _lightDirection.ApplyMatrix(rotation);
            else
                _position.ApplyMatrix(rotation);
        }

        public void MoveUp() => Move(0, 30, 0);
        public void MoveDown() => Move(0, -30, 0);
        public void MoveRight() => Move(30, 0, 0);
        public void MoveLeft() => Move(-30, 0, 0);
        public void MoveForward() => Move(0, 0, 30);
        public void MoveBackward() => Move(0, 0, -30);
        public void RotateUp() => Rotate(-10, 0);
        public void RotateDown() => Rotate(10, 0);
        public void RotateRight() => Rotate(0, 10);
        public void RotateLeft() => Rotate(0, -10);
    }
}