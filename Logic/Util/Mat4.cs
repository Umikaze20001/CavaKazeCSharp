using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render
{
    class Mat4
    {
        float[,] a = new float[4, 4];
        public Mat4(Vec4 v, Vec4 b, Vec4 c, Vec4 d)
        {
		    for (int i = 0; i< 4; ++i) {
			    a[i,0] = v[i];
			    a[i,1] = b[i];
			    a[i,2] = c[i];
			    a[i,3] = d[i];
            }
		}

        public Mat4(float m1, float m2, float m3, float m4, float m5, float m6, float m7, float m8, float m9, float m10, float m11, float m12,
            float m13,float m14,float m15,float m16)
        {
            a[0, 0] = m1;
            a[0, 1] = m2;
            a[0, 2] = m3;
            a[0, 3] = m4;
            a[1, 0] = m5;
            a[1, 1] = m6;
            a[1, 2] = m7;
            a[1, 3] = m8;
            a[2, 0] = m9;
            a[2, 1] = m10;
            a[2, 2] = m11;
            a[2, 3] = m12;
            a[3, 0] = m13;
            a[3, 1] = m14;
            a[3, 2] = m15;
            a[3, 3] = m16;
        }

        public Mat4()
        {
            for(int i = 0; i < 4; ++i)
            {
                for(int j = 0; j < 4; j++)
                {
                    if (i == j)
                        a[i, j] = 1;
                    else
                        a[i, j] = 0;
;               }
            }
        }

        public Mat4(float[] numbers)
        {
            for(int i = 0; i < 4; ++i)
            {
                for(int j = 0; j < 4; ++j)
                {
                    a[i, j] = numbers[4 * i + j];
                }
            }
        }

        Mat4(Vec4[] vecs)
        {
            for(int i = 0; i < 4; ++i)
            {
                for(int j = 0; j < 4; ++j)
                {
                    a[i, j] = vecs[i][j];
                }
            }
        }

        public float this[int i, int j]
        {
            get
            {
                return a[i, j];
            }
            set
            {
                a[i, j] = value;
            }
        }

        public Vec4 row(int i)
        {
            return new Vec4(a[i, 0], a[i, 1], a[i, 2], a[i, 3]);
        } 

        public Vec4 column(int j)
        {
            return new Vec4(a[0, j], a[1, j], a[2, j], a[3, j]);
        }

        public static Mat4 operator+(Mat4 a, Mat4 b)
        {
            Mat4 mat = new Mat4();
            for(int i = 0; i < 4; ++i)
            {
                for(int j = 0; j < 0; ++j)
                {
                    mat[i, j] = a[i, j] + b[i, j];
                }
            }
            return mat;
        }

        public static Mat4 operator-(Mat4 a, Mat4 b)
        {
            Mat4 mat = new Mat4();
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 0; ++j)
                {
                    mat[i, j] = a[i, j] - b[i, j];
                }
            }
            return mat;
        }

        public static Mat4 operator*(Mat4 a,Mat4 b)
        {
            Mat4 mat = new Mat4();
            for(int i = 0; i < 4;++i)
                for(int j = 0; j < 4; ++j)
                {
                    mat[i, j] = Vec4.dot(a.row(i), b.column(j));
                }
            return mat;
        }

        public static Mat4 operator*(Mat4 a,float b)
        {
            Mat4 mat = new Mat4();
            for(int i = 0; i < 4; ++i)
            {
                for(int j = 0; j < 4; ++j)
                {
                    mat[i, j] *= b;
                }
            }
            return mat;
        }

        public static Mat4 operator/(Mat4 a, float b)
        {
            Mat4 mat = new Mat4();
            if(b == 0)
            {
                //报错
            }
            float inv = 1 / b;
            mat = a * inv;
            return mat;
        }

        public static Mat4 operator*(float b, Mat4 a)
        {
            Mat4 mat = new Mat4();
            for (int i = 0; i < 4; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    mat[i, j] *= b;
                }
            }
            return mat;
        }

        public static Vec4 operator*(Mat4 a,Vec4 v)
        {
            Vec4 result = new Vec4();
            for(int i = 0; i < 4; ++i)
            {
                result[i] = Vec4.dot(a.row(i), v);
            }
            return result;
        }

        public static Vec4 operator*(Vec4 v,Mat4 a)
        {
            Vec4 result = new Vec4();
            for(int i = 0; i < 4; ++i)
            {
                result[i] = Vec4.dot(a.column(i), v);
            }
            return result;
        }

        public Mat4 transpose()
        {
            for(int i = 0; i < 4; ++i)
            {
                for(int j = i; j < 4; ++j)
                {
                    a[i, j] = a[i, j] + a[j, i];
                    a[j, i] = a[i, j] - a[j, i];
                    a[i, j] -= a[j, i]; 
                }
            }
            return this;
        }

        public Mat4 getTranspose()
        {
            Mat4 mat = new Mat4();
            for(int i = 0; i < 4; ++i)
            {
                for(int j = 0; j < 4; ++j)
                {
                    mat[i, j] = a[j, i];
                }
            }
            return mat;
        }

        public Mat4 inverse()
        {
            Mat4 mat = this.getInverse();
            for(int i = 0; i < 4; ++i)
            {
                for(int j = 0; j < 4; ++j)
                {
                    a[i, j] = mat[i, j];
                }
            }
            return this;
        }

        public Mat4 getInverse()
        {
            float f01 = a[2,2] * a[3,3] - a[2,3] * a[3,2];
            float f02 = a[2,3] * a[3,1] - a[2,1] * a[3,3];
            float f03 = a[2,1] * a[3,2] - a[2,2] * a[3,1];
            float f04 = a[2,3] * a[3,0] - a[2,0] * a[3,3];
            float f05 = a[2,0] * a[3,2] - a[2,2] * a[3,0];
            float f06 = a[2,0] * a[3,1] - a[2,1] * a[3,0];
            float f07 = a[2,3] * a[3,1] - a[2,1] * a[3,3];
            float f08 = a[1,2] * a[3,3] - a[1,3] * a[3,2];
            float f09 = a[1,3] * a[3,1] - a[1,1] * a[3,3];
            float f10 = a[1,1] * a[3,2] - a[1,2] * a[3,1];
            float f11 = a[1,3] * a[3,0] - a[1,0] * a[3,3];
            float f12 = a[1,0] * a[3,2] - a[1,2] * a[3,0];
            float f13 = a[1,0] * a[3,1] - a[1,1] * a[3,0];
            float f14 = a[1,2] * a[2,3] - a[1,3] * a[2,2];
            float f15 = a[1,3] * a[2,1] - a[1,1] * a[2,3];
            float f16 = a[1,1] * a[2,2] - a[1,2] * a[2,1];
            float f17 = a[1,3] * a[2,0] - a[1,0] * a[2,3];
            float f18 = a[1,0] * a[2,2] - a[1,2] * a[2,0];
            float f19 = a[1,0] * a[2,1] - a[1,1] * a[2,0];

            Mat4 adjoint = new Mat4();
            adjoint[0,0] = a[1,1] * f01 + a[1,2] * f02 + a[1,3] * f03;
            adjoint[1,0] = -(a[1,0] * f01 + a[1,2] * f04 + a[1,3] * f05);
            adjoint[2,0] = a[1,0] * -f02 + a[1,1] * f04 + a[1,3] * f06;
            adjoint[3,0] = -(a[1,0] * f03 + a[1,1] * -f05 + a[1,2] * f06);
            adjoint[0,1] = -(a[0,1] * f01 + a[0,2] * f07 + a[0,3] * f03);
            adjoint[1,1] = a[0,0] * f01 + a[0,2] * f04 + a[0,3] * f05;
            adjoint[2,1] = -(a[0,0] * -f07 + a[0,1] * f04 + a[0,3] * f06);
            adjoint[3,1] = a[0,0] * f03 + a[0,1] * -f05 + a[0,2] * f06;
            adjoint[0,2] = a[0,1] * f08 + a[0,2] * f09 + a[0,3] * f10;
            adjoint[1,2] = -(a[0,0] * f08 + a[0,2] * f11 + a[0,3] * f12);
            adjoint[2,2] = a[0,0] * -f09 + a[0,1] * f11 + a[0,3] * f13;
            adjoint[3,2] = -(a[0,0] * f10 + a[0,1] * -f12 + a[0,2] * f13);
            adjoint[0,3] = -(a[0,1] * f14 + a[0,2] * f15 + a[0,3] * f16);
            adjoint[1,3] = a[0,0] * f14 + a[0,2] * f17 + a[0,3] * f18;
            adjoint[2,3] = -(a[0,0] * -f15 + a[0,1] * f17 + a[0,3] * f19);
            adjoint[3,3] = a[0,0] * f16 + a[0,1] * -f18 + a[0,2] * f19;
            float det = a[0,0] * adjoint[0,0] + a[0,1] * adjoint[1,0] + a[0,2] * adjoint[2,0] + a[0,3] * adjoint[3,0];
            float invdet = 1.0f / det;
            return adjoint * invdet;

        }
    }
}
