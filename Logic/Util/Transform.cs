using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render
{
    public class Transform
    {
        Transform(Mat4 m1)
        {
            m = m1;
            inv = m.getInverse();
        }

        public static Transform translate(Vec3 vector)
        {
            Mat4 mat = new Mat4();
            mat[0, 3] = vector.x;
            mat[1, 3] = vector.y;
            mat[2, 3] = vector.z;
            return new Transform(mat);
        }

        public static Transform rorate_x(float degree)
        {
            Mat4 mat = new Mat4();
            float cosa = (float)Math.Cos(degree);
            float sina = (float)Math.Sin(degree);
            mat[1, 2] = -sina;
            mat[1, 1] = cosa;
            mat[2, 1] = sina;
            mat[2, 2] = cosa;

            return new Transform(mat);
        }

        public static Transform rotate_y(float degree)
        {
            Mat4 mat = new Mat4();
            float cosa = (float)Math.Cos(degree);
            float sina = (float)Math.Sin(degree);
            mat[1, 2] = -sina;
            mat[1, 1] = cosa;
            mat[2, 1] = sina;
            mat[2, 2] = cosa;
            return new Transform(mat);
        }

        public static Transform rotate_z(float degree)
        {
            Mat4 mat = new Mat4();
            float cosa = (float)Math.Cos(degree);
            float sina = (float)Math.Sin(degree);
            mat[0, 0] = cosa;
            mat[0, 1] = -sina;
            mat[1, 0] = sina;
            mat[1, 1] = cosa;

            return new Transform(mat);
        }

        public static Transform rotate(Vec3 axis,float degree)
        {
            axis.normalize();
            Mat4 m1 = new Mat4();
            float cosa = (float)Math.Cos(degree);
            float sina = (float)-Math.Sin(degree);
            float ncosa = 1 - cosa;
            float xy = axis.x * axis.y;
            float xz = axis.x * axis.z;
            float yz = axis.y * axis.z;
            m1[0,0] = cosa + (axis.x * axis.x) * ncosa;
            m1[0,1] = xy * ncosa - axis.z * sina;
            m1[0,2] = xz * ncosa + axis.y * sina;
            m1[0,3] = 0;
            m1[1,0] = xy * ncosa + axis.z * sina;
            m1[1,1] = cosa + axis.y * axis.y * ncosa;
            m1[1,2] = yz * ncosa - axis.x * sina;
            m1[1,3] = 0;
            m1[2,0] = xz * ncosa - axis.y * sina;
            m1[2,1] = yz * ncosa + axis.x * sina;
            m1[2,2] = axis.z * axis.z * ncosa + cosa;
            m1[2,3] = 0;
            m1[3,0] = 0;
            m1[3,1] = 0;
            m1[3,2] = 0;
            m1[3,3] = 1;

            return new Transform(m1);
        }

        public static Transform scale(float x,float y,float z)
        {
            Mat4 mat = new Mat4();
            mat[0, 0] = x;
            mat[1, 1] = y;
            mat[2, 2] = z;
            return new Transform(mat);
        }


        /// <summary>
        /// 这个主要是用于将局部坐标系转到世界坐标系
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="x">局部坐标系x轴对应的世界坐标系的坐标</param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Transform toWorld(Point pos,Vec3 x,Vec3 y ,Vec3 z)
        {
            Vec4[] r = new Vec4[3];
            Vec4[] t = new Vec4[3];
            t[0] = new Vec4(1, 0, 0, pos[0]);
            t[1] = new Vec4(0, 1, 0, pos[1]);
            t[2] = new Vec4(0, 0, 1, pos[2]);
            r[0] = new Vec4(x.x, x.y, x.z, 0);
            r[1] = new Vec4(y.x, y.y, y.z, 0);
            r[2] = new Vec4(z.x, z.y, z.z, 0);
            Mat4 rotate = new Mat4(r[0], r[1], r[2], new Vec4(0, 0, 0, 1));
            Mat4 translate = new Mat4(t[0], t[1], t[2], new Vec4(0, 0, 0, 1));
            Mat4 m = rotate.transpose() * translate;
            return new Transform(m);
        }

        public static Transform operator *(Transform a, Transform b)
        {
            Mat4 m1 = a.m * b.m;
            return new Transform(m1);
        }

        public static Point operator *(Transform a,Point p)
        {
            Vec4 v = new Vec4(p.x, p.y, p.z, 1);
            Vec4 result = a.m * v;
            if(result.w == 0)
            {
                //报错
                return new Point();
            }
            else
            {
                float inv = 1 / result.w;
                return new Point(result.x * inv, result.y * inv, result.z * inv);
            }
        }

        public static Vec3 operator *(Transform a, Vec3 v)
        {
            Vec4 v1 = new Vec4(v.x, v.y, v.z, 0);
            Vec4 result = a.m * v1;
            return new Vec3(result.x, result.y, result.z);
        }

        public Transform inverse()
        {
            Mat4 temp = m;
            m = inv;
            inv = temp;
            return this;
        }

        public Transform getInverse()
        {
            return new Transform(inv);
        }


        Mat4 m, inv;
    }
}
