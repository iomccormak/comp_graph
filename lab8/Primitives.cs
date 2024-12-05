using System;
using System.Collections.Generic;
using System.Drawing;

namespace lab8
{
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
        public static Point3D operator -(Point3D p1, Point3D p2)
        {
            return new Point3D(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }
        public static Point3D operator +(Point3D p1, Point3D p2)
        {
            return new Point3D(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }
        public Point3D CrossProduct(Point3D other)
        {
            return new Point3D(
                this.Y * other.Z - this.Z * other.Y,
                this.Z * other.X - this.X * other.Z,
                this.X * other.Y - this.Y * other.X
            );
        }
        public float DotProduct(Point3D other)
        {
            return this.X * other.X + this.Y * other.Y + this.Z * other.Z;
        }
        public void Normalize()
        {
            float length = (float)Math.Sqrt(X * X + Y * Y + Z * Z);
            if (length > 0)
            {
                X /= length;
                Y /= length;
                Z /= length;
            }
        }
    }

    public class Face
    {
        public List<int> indexes;
        public Point3D normal;
        public Color faceColor { get; set; }

        public Face(List<int> _indexes)
        {
            indexes = _indexes;
            normal = null;
            faceColor = Color.Red;
        }

        public Face(List<int> _indexes, Color color)
        {
            indexes = _indexes;
            normal = null;
            faceColor = color;
        }

        public Point3D CalculateNormal(List<Point3D> points)
        {
            Point3D A = points[indexes[0]];
            Point3D B = points[indexes[1]];
            Point3D C = points[indexes[2]];

            Point3D center = new Point3D(
                (A.X + B.X + C.X) / 3,
                (A.Y + B.Y + C.Y) / 3,
                (A.Z + B.Z + C.Z) / 3
            );

            Point3D AB = B - A;
            Point3D AC = C - A;
            normal = AB.CrossProduct(AC);
            normal.Normalize();

            Point3D centerToFace = new Point3D(
                (A.X + B.X + C.X) / 3,
                (A.Y + B.Y + C.Y) / 3,
                (A.Z + B.Z + C.Z) / 3
            );

            if (normal.DotProduct(centerToFace) < 0)
            {
                normal = new Point3D(-normal.X, -normal.Y, -normal.Z);
            }

            return normal;
        }
    }
}