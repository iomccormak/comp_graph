using System;

namespace lab8
{
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

        public static float[][] Parallel()
        {
            return new float[4][]
            {
                new float[4] { 1, 0, 0, 0 },
                new float[4] { 0, 1, 0, 0 },
                new float[4] { 0, 0, 0, 0 }, 
                new float[4] { 0, 0, 0, 1 },
            };
        }


        public static float[][] Identity()
        {
            return new float[][]
            {
                new float[] {1, 0, 0, 0},
                new float[] {0, 1, 0, 0},
                new float[] {0, 0, 1, 0},
                new float[] {0, 0, 0, 1},
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