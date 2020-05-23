using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Render
{
    public class Vec3
    {
        public static implicit operator Point(Vec3 a)
        {
            return new Point(a.x, a.y, a.z);
        }

        public float x, y, z;
        public Vec3()
        {
            x = y = z = 0.0f;
        }
        public Vec3(float a, float b, float c)
        {
            x = a;
            y = b;
            z = c;
        }

        public Vec3(Point point)
        {
            x = point.x;
            y = point.y;
            z = point.z;
        }

        public Vec3(Vec3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public float this[int i]
        {
            get
            {
                if (i == 0)
                    return x;
                else if (i == 1)
                    return y;
                else
                    return z;
            }
            set
            {
                if (i == 0)
                    x = value;
                else if (i == 1)
                    y = value;
                else
                    z = value;
            }
        }

        public static Vec3 operator +(Vec3 a, Vec3 b)
        {
            Vec3 p = new Vec3();
            p[0] = a[0] + b[0];
            p[1] = a[1] + b[1];
            p[2] = a[2] + b[2];
            return p;
        }

        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            Vec3 v = new Vec3();
            v[0] = a[0] - b[0];
            v[1] = a[1] - b[1];
            v[2] = a[2] - b[2];
            return v;
        }

        public static Vec3 operator *(Vec3 a, float index)
        {
            Vec3 p = new Vec3();
            p[0] = a[0] * index;
            p[1] = a[1] * index;
            p[2] = a[2] * index;
            return p;
        }
        public static Vec3 operator *(float index, Vec3 a)
        {
            Vec3 p = new Vec3();
            p[0] = a[0] * index;
            p[1] = a[1] * index;
            p[2] = a[2] * index;
            return p;
        }

        public static Vec3 operator /(Vec3 a, float index)
        {
            if (index == 0)
            {
                //报错
            }
            float inv = 1 / index;
            Vec3 p = new Vec3();
            p[0] = a[0] * inv;
            p[1] = a[1] * inv;
            p[2] = a[2] * inv;
            return p;
        }

        public float length()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        public void normalize()
        {
            float i = length();
            if (i == 0)
            {
                // 报错
            }
            float inv = 1 / i;
            x = x * inv;
            y = y * inv;
            z = z * inv;
        }

        public static float dot(Vec3 a, Vec3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static Vec3 cross(Vec3 a, Vec3 b)
        {
            return new Vec3((a.y * b.z) - (a.z * b.y), (a.z * b.x) - (a.x * b.z), (a.x * b.y) - (a.y * b.x));
        }

    }

}
