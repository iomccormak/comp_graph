using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace lab9
{
    public static class PhongLighting
    {
        public static void ShadePhongLighting(Graphics graphics, Face face, List<Point3D> points, Point3D lightDirection, Point3D viewDirection)
        {
            var triangles = TriangulateFace(face);

            List<Point3D> vertexNormals = ComputeVertexNormals(face, points);

            foreach (var triangle in triangles)
            {
                Point3D A = points[triangle[0]];
                Point3D B = points[triangle[1]];
                Point3D C = points[triangle[2]];

                Point3D normalA = vertexNormals[triangle[0]].Normalize();
                Point3D normalB = vertexNormals[triangle[1]].Normalize();
                Point3D normalC = vertexNormals[triangle[2]].Normalize();

                PointF[] trianglePoints = {
                    new PointF(A.X / A.W, A.Y / A.W),
                    new PointF(B.X / B.W, B.Y / B.W),
                    new PointF(C.X / C.W, C.Y / C.W)
                };

                RectangleF bounds = GetBoundingBox(trianglePoints);

                for (int y = (int)bounds.Top; y <= (int)bounds.Bottom; y++)
                {
                    for (int x = (int)bounds.Left; x <= (int)bounds.Right; x++)
                    {
                        PointF point = new PointF(x, y);

                        if (!PointInTriangle(point, trianglePoints)) 
                            continue;
                    
                        Point3D interpolatedNormal = InterpolateNormal(
                            point, 
                            trianglePoints, 
                            new Point3D[] { normalA, normalB, normalC }
                            ).Normalize();

                        Color pixelColor = CalculateColor(
                            interpolatedNormal, 
                            lightDirection - new Point3D(point),
                            viewDirection,
                            face.color
                        );

                        graphics.FillRectangle(new SolidBrush(pixelColor), x, y, 1, 1);
                    }
                }
            }
        }
        
        private static List<Point3D> ComputeVertexNormals(Face face, List<Point3D> points)
        {
            List<Point3D> vertexNormals = new List<Point3D>(new Point3D[points.Count]);
            List<int> counts = new List<int>(new int[points.Count]);

            for (int i = 0; i < vertexNormals.Count; i++)
            {
                vertexNormals[i] = new Point3D(0, 0, 0);
            }

            foreach (var triangle in TriangulateFace(face))
            {
                Point3D A = points[triangle[0]];
                Point3D B = points[triangle[1]];
                Point3D C = points[triangle[2]];

                Point3D edge1 = B - A;
                Point3D edge2 = C - A;
                Point3D faceNormal = edge1.CrossProduct(edge2).Normalize();

                for (int i = 0; i < 3; i++)
                {
                    vertexNormals[triangle[i]] += faceNormal;
                    counts[triangle[i]]++;
                }
            }

            for (int i = 0; i < vertexNormals.Count; i++)
            {
                if (counts[i] > 0)
                {
                    vertexNormals[i] /= counts[i]; 
                    vertexNormals[i] = vertexNormals[i].Normalize(); 
                }
            }

            return vertexNormals;
        }
        
        public static Color CalculateColor(Point3D normal, Point3D lightDirection, Point3D viewDirection, Color faceColor)
        {
            Point3D n2 = normal.Normalize(); 
            Point3D l2 = lightDirection.Normalize(); 
            Point3D v2 = viewDirection.Normalize(); 

            Point3D reflect = n2 * 2 * n2.DotProduct(l2) - l2;
            reflect.Normalize();

            float lightIntensity = 1.5f;

            float kA = 0.2f; 
            float kD = 0.7f; 
            float kS = 0.5f; 
            float kE = 12.0f; 

            float ambient = kA * 0.2f; 
            float diffuse = kD * Math.Max(l2.DotProduct(n2), 0.0f);
            float specular = kS * (float)Math.Pow(Math.Max(reflect.DotProduct(v2), 0.0f), kE);
            float light = ambient + lightIntensity * (diffuse + specular);
            
            /*float diff = 0.2f + Math.Max(n2.DotProduct(l2), 0.0f);*/
            
            int r, g, b;
            if (light < 0.4f)
            {
                r = Clamp((int)(faceColor.R * 0.3f), 0, 255);
                g = Clamp((int)(faceColor.G * 0.3f), 0, 255);
                b = Clamp((int)(faceColor.B * 0.3f), 0, 255);
            }
            else if (light < 0.7f)
            {
                r = faceColor.R;
                g = faceColor.G;
                b = faceColor.B;
            }
            else
            {
                r = Clamp((int)(faceColor.R * 1.3f), 0, 255);
                g = Clamp((int)(faceColor.G * 1.3f), 0, 255);
                b = Clamp((int)(faceColor.B * 1.3f), 0, 255);
            }
            
            // Раскоментируй, если захотелось посмотреть на обычного Фонга
            /*r = Clamp((int)(faceColor.R * light), 0, 255);
            g = Clamp((int)(faceColor.G * light), 0, 255);
            b = Clamp((int)(faceColor.B * light), 0, 255);*/
            
            return Color.FromArgb(r, g, b);
        }
        
        private static List<int[]> TriangulateFace(Face face)
        {
            var indexes = face.indexes;
            var triangles = new List<int[]>();

            for (int i = 1; i < indexes.Count - 1; i++)
            {
                triangles.Add(new int[] { indexes[0], indexes[i], indexes[i + 1] });
            }

            return triangles;
        }

        private static RectangleF GetBoundingBox(PointF[] points)
        {
            float minX = points.Min(p => p.X);
            float minY = points.Min(p => p.Y);
            float maxX = points.Max(p => p.X);
            float maxY = points.Max(p => p.Y);

            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        private static bool PointInTriangle(PointF p, PointF[] vertices)
        {
            float sign(PointF p1, PointF p2, PointF p3)
            {
                return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
            }

            float d1 = sign(p, vertices[0], vertices[1]);
            float d2 = sign(p, vertices[1], vertices[2]);
            float d3 = sign(p, vertices[2], vertices[0]);

            bool hasNeg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            bool hasPos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(hasNeg && hasPos);
        }

        private static Point3D InterpolateNormal(PointF p, PointF[] points, Point3D[] normals)
        {
            float detT = (points[1].Y - points[2].Y) * (points[0].X - points[2].X) +
                         (points[2].X - points[1].X) * (points[0].Y - points[2].Y);

            float l1 = ((points[1].Y - points[2].Y) * (p.X - points[2].X) +
                        (points[2].X - points[1].X) * (p.Y - points[2].Y)) / detT;

            float l2 = ((points[2].Y - points[0].Y) * (p.X - points[2].X) +
                        (points[0].X - points[2].X) * (p.Y - points[2].Y)) / detT;

            float l3 = 1 - l1 - l2;

            return normals[0] * l1 + normals[1] * l2 + normals[2] * l3;
        }
        
        private static int Clamp(int value, int min, int max)
        {
            return Math.Max(min, Math.Min(max, value));
        }
    }
}