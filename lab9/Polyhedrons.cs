using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace lab9
{
    public class Polyhedron
    {
        public const int EDGE_LENGTH = 200;
        public List<Point3D> points;
        public List<Face> faces;
        public List<Coordinates> coordinates;  
        
        public float[][] modelMatrix;

        public Polyhedron()
        {
            points = new List<Point3D>();
            faces = new List<Face>();
            coordinates = new List<Coordinates>();
            modelMatrix = Matrices.Identity();
        }

        public void SaveToFileInProjectFolder(float[][] matrix = null)
        {
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string modelsDirectory = Path.Combine(projectDirectory, "Models");
            if (!Directory.Exists(modelsDirectory))
            {
                Directory.CreateDirectory(modelsDirectory);
            }

            string filePath = Path.Combine(modelsDirectory, "3D_Model.obj");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var vertex in points)
                {
                    Point3D transformedVertex = vertex.Clone();
                    if (matrix != null)
                    {
                        transformedVertex.ApplyMatrix(matrix);
                    }

                    writer.WriteLine($"v {transformedVertex.X.ToString(CultureInfo.InvariantCulture)} " +
                                     $"{transformedVertex.Y.ToString(CultureInfo.InvariantCulture)} " +
                                     $"{transformedVertex.Z.ToString(CultureInfo.InvariantCulture)}");
                }

                foreach (var face in faces)
                {
                    Point3D transformedNormal = face.normal.Clone();
                    if (matrix != null)
                    {
                        transformedNormal = TransformNormal(transformedNormal, matrix);
                        transformedNormal.Normalize();
                    }

                    writer.WriteLine($"vn {transformedNormal.X.ToString(CultureInfo.InvariantCulture)} " +
                                     $"{transformedNormal.Y.ToString(CultureInfo.InvariantCulture)} " +
                                     $"{transformedNormal.Z.ToString(CultureInfo.InvariantCulture)}");
                }

                int normalIndex = 1;
                foreach (var face in faces)
                {
                    writer.Write("f");
                    foreach (var index in face.indexes)
                    {
                        writer.Write($" {index + 1}//{normalIndex}");
                    }

                    writer.WriteLine();
                    normalIndex++;
                }
            }
        }

        public void ParseFromOBJ(string filePath)
        {
            points = new List<Point3D>();
            faces = new List<Face>();
            coordinates = new List<Coordinates>();
            List<Point3D> normals = new List<Point3D>();

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
                            float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float x) &&
                            float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out float y) &&
                            float.TryParse(parts[3], NumberStyles.Float, CultureInfo.InvariantCulture, out float z))
                        {
                            points.Add(new Point3D(x, y, z));
                        }
                    }
                    else if (line.StartsWith("vn "))
                    {
                        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length == 4 &&
                            float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float x) &&
                            float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out float y) &&
                            float.TryParse(parts[3], NumberStyles.Float, CultureInfo.InvariantCulture, out float z))
                        {
                            normals.Add(new Point3D(x, y, z));
                        }
                    }
                    else if (line.StartsWith("vt "))
                    {
                        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length == 3 &&
                            float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float u) &&
                            float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out float v))
                        {
                            coordinates.Add(new Coordinates(u, v));
                        }
                    }
                    else if (line.StartsWith("f "))
                    {
                        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        var indexes = new List<int>();
                        var normalIndexes = new List<int>();
                        var textureIndexes = new List<int>();

                        for (int i = 1; i < parts.Length; i++)
                        {
                            string[] faceData = parts[i].Split('/');
                            if (int.TryParse(faceData[0], out int vertexIndex))
                            {
                                int adjustedIndex = vertexIndex - 1;
                                if (adjustedIndex >= 0 && adjustedIndex < points.Count)
                                {
                                    indexes.Add(adjustedIndex);
                                }
                            }

                            if (faceData.Length > 1 && int.TryParse(faceData[1], out int textureIndex))
                            {
                                int adjustedTextureIndex = textureIndex - 1;
                                if (adjustedTextureIndex >= 0 && adjustedTextureIndex < coordinates.Count)
                                {
                                    textureIndexes.Add(adjustedTextureIndex);
                                }
                            }

                            if (faceData.Length > 2 && int.TryParse(faceData[2], out int normalIndex))
                            {
                                int adjustedNormalIndex = normalIndex - 1;
                                if (adjustedNormalIndex >= 0 && adjustedNormalIndex < normals.Count)
                                {
                                    normalIndexes.Add(adjustedNormalIndex);
                                }
                            }

                            
                        }

                        if (indexes.Count >= 3)
                        {
                            Face face = new Face(indexes, textureIndexes);
                            if (normalIndexes.Count > 0)
                            {
                                face.normal = normals[normalIndexes[0]];
                            }

                            faces.Add(face);
                        }
                    }
                }
            }

            Console.WriteLine(
                $"Parsed {points.Count} points, {normals.Count} normals, and {faces.Count} faces from {filePath}");
        }

        private Point3D TransformNormal(Point3D normal, float[][] worldMatrix)
        {
            float[] transformed = new float[3];
            for (int i = 0; i < 3; i++)
            {
                transformed[i] =
                    worldMatrix[0][i] * normal.X +
                    worldMatrix[1][i] * normal.Y +
                    worldMatrix[2][i] * normal.Z;
            }

            Point3D transformedNormal = new Point3D(transformed[0], transformed[1], transformed[2]);
            transformedNormal.Normalize();
            return transformedNormal;
        }

        public List<Face> GetVisibleFaces(Point3D viewVector)
        {
            List<Face> visibleFaces = new List<Face>();

            foreach (var face in faces)
            {
                if (face.normal == null) face.CalculateNormal(points);
                var normal = face.normal;

                Point3D transformedNormal = TransformNormal(normal, modelMatrix);
                float dotProduct = transformedNormal.DotProduct(viewVector);

                if (dotProduct < 0)
                {
                    visibleFaces.Add(face);
                }
            }

            return visibleFaces;
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
                new Face(new List<int> { 15, 7, 11 ,3, 13,  }),
                new Face(new List<int> { 0, 8, 4, 14, 12 }),
                new Face(new List<int> { 0, 12, 1, 17, 16 }),
                new Face(new List<int> { 0, 16, 2, 10, 8 }),
                new Face(new List<int> { 1, 12, 14, 5, 9 }),
                new Face(new List<int> { 1, 9, 5, 14, 12, }),
                new Face(new List<int> { 2, 16, 17, 3, 13 }),
                new Face(new List<int> { 4, 14, 5, 19, 18 }),
                new Face(new List<int> { 4, 18, 6, 10, 8 }),
                new Face(new List<int> { 5, 9, 11, 7, 19 }),
                new Face(new List<int> { 6, 18, 19, 7, 15 }),
                new Face(new List<int> { 2, 10 ,6, 15, 13}),
                new Face(new List<int> { 3, 17, 1, 9, 11 })
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

    public class FunctionalPolyhedron : Polyhedron
    {
        public FunctionalPolyhedron(float x0, float x1, float y0, float y1, float step, Func<float, float, float> G)
        {
            Create(x0, x1, y0, y1, step, G);
        }

        private void Create(float x0, float x1, float y0, float y1, float step, Func<float, float, float> G)
        {
            int cntX = (int)Math.Floor((x1 - x0) / step);
            int cntY = (int)Math.Floor((y1 - y0) / step);

            for (int x_i = 0; x_i < cntX; x_i++)
            {
                for (int y_i = 0; y_i < cntY; y_i++)
                {
                    float x = x0 + step * x_i;
                    float y = y0 + step * y_i;
                    float z = G(x, y);
                    points.Add(new Point3D(x * 15, z * 15, y * 15));
                }
            }

            for (int i = 0; i < cntY - 1; i++)
            {
                for (int j = 0; j < cntX - 1; j++)
                {
                    int topLeft = i * cntX + j;
                    int topRight = i * cntX + (j + 1);
                    int bottomLeft = (i + 1) * cntX + j;
                    int bottomRight = (i + 1) * cntX + (j + 1);
                    faces.Add(new Face(new List<int> { topLeft, topRight, bottomRight, bottomLeft }));
                }
            }
        }
    }
}